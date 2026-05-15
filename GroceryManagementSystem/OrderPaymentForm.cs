using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace GroceryManagementSystem
{
    public partial class OrderPaymentForm : Form
    {
        // Variables to catch the data passed from the Customer Dashboard
        DataTable finalCart;
        int finalTotal;
        int currentCustomerID = 1; // Using '3' because that is the ID of your seeded Customer!

        // Modify this constructor to accept the cart data
        public OrderPaymentForm(DataTable cartData, int totalAmount)
        {
            InitializeComponent();
            finalCart = cartData;
            finalTotal = totalAmount;
            // Assuming you add a label named lblGrandTotal in the designer
            lblGrandTotal.Text = "Total to Pay: ₹" + finalTotal; 
        }

        private void btnPlaceOrder_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtAddress.Text) || cmbPaymentMethod.SelectedItem == null)
            {
                MessageBox.Show("Please enter an address and select a payment method.");
                return;
            }

            try
            {
                DatabaseConnection db = new DatabaseConnection();

                // 1. Generate a new OrderID (Find the highest current ID and add 1)
                DataTable idTable = db.ExecuteQuery("SELECT ISNULL(MAX(OrderID), 0) + 1 AS NewID FROM Orders");
                int newOrderID = Convert.ToInt32(idTable.Rows[0]["NewID"]);

                // 2. Insert into the main Orders table
                string orderQuery = "INSERT INTO Orders (OrderID, CustomerID, TotalPrice, Status, PaymentMethod, PaymentStatus) " +
                                    "VALUES (@oid, @cid, @total, 'Pending', @payMethod, 'Paid')";

                SqlParameter[] orderParams = new SqlParameter[]
                {
                    new SqlParameter("@oid", newOrderID),
                    new SqlParameter("@cid", currentCustomerID),
                    new SqlParameter("@total", finalTotal),
                    new SqlParameter("@payMethod", cmbPaymentMethod.SelectedItem.ToString())
                };
                db.ExecuteNonQuery(orderQuery, orderParams);

                // 3. Loop through the cart and save EACH item into OrderProduct
                // 3. Loop through the cart and save EACH item into OrderProduct
                foreach (DataRow row in finalCart.Rows)
                {
                    // --- QUERY 1: Save to OrderProduct ---
                    string opQuery = "INSERT INTO OrderProduct (OrderID, ProductID, Quantity) VALUES (@oid, @pid, @qty)";
                    SqlParameter[] opParams = new SqlParameter[]
                    {
                        new SqlParameter("@oid", newOrderID),
                        new SqlParameter("@pid", Convert.ToInt32(row["ProductID"])),
                        new SqlParameter("@qty", Convert.ToInt32(row["Quantity"]))
                    };
                    db.ExecuteNonQuery(opQuery, opParams); // Uses opParams

                    // --- QUERY 2: Update Merchant Inventory ---
                    string stockQuery = "UPDATE Product SET Quantity = Quantity - @qty WHERE ProductID = @pid";

                    // FIX: Create a BRAND NEW array of parameters for this second query!
                    SqlParameter[] stockParams = new SqlParameter[]
                    {
                        new SqlParameter("@pid", Convert.ToInt32(row["ProductID"])),
                        new SqlParameter("@qty", Convert.ToInt32(row["Quantity"]))
                    };
                    db.ExecuteNonQuery(stockQuery, stockParams); // Uses stockParams
                }

                // 4. Success!
                MessageBox.Show("Order Placed Successfully! Your Order ID is #" + newOrderID);

                // Close this form and exit or go back to login
                CustomerDashboard dashboard = new CustomerDashboard();
                dashboard.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error placing order: " + ex.Message);
            }
        }

    }
}