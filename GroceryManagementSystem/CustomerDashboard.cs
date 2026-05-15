using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GroceryManagementSystem
{
    public partial class CustomerDashboard : Form
    {
        // 1. Create a "Virtual Cart" to hold items before checkout
        DataTable virtualCart = new DataTable();
        int currentProductID = 0;
        string currentProductName = "";
        int currentProductPrice = 0;
        int cartTotal = 0;

        public CustomerDashboard()
        {
            InitializeComponent();
            SetupCart();
            LoadShopProducts();
            LoadOrderHistory();
        }

        // --- SETUP THE VIRTUAL CART ---
        private void SetupCart()
        {
            virtualCart.Columns.Add("ProductID", typeof(int));
            virtualCart.Columns.Add("Name", typeof(string));
            virtualCart.Columns.Add("Price", typeof(int));
            virtualCart.Columns.Add("Quantity", typeof(int));
            virtualCart.Columns.Add("SubTotal", typeof(int));

            dgvCart.DataSource = virtualCart;
        }

        // --- LOAD AVAILABLE GROCERIES ---
        private void LoadShopProducts()
        {
            try
            {
                DatabaseConnection db = new DatabaseConnection();
                // Only load products that actually have stock!
                DataTable dt = db.ExecuteQuery("SELECT ProductID, Name, Price, Quantity AS Stock, Category FROM Product WHERE Quantity > 0");
                dgvShop.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading shop: " + ex.Message);
            }
        }

        // --- SELECT A PRODUCT FROM THE SHOP ---
        private void dgvShop_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvShop.Rows[e.RowIndex];
                currentProductID = Convert.ToInt32(row.Cells["ProductID"].Value);
                currentProductName = row.Cells["Name"].Value.ToString();
                currentProductPrice = Convert.ToInt32(row.Cells["Price"].Value);

                // Optional: Update a label so the user knows what they clicked
                // lblSelectedProduct.Text = "Selected: " + currentProductName;
            }
        }

        // --- ADD TO CART BUTTON ---
        private void btnAddCart_Click(object sender, EventArgs e)
        {
            if (currentProductID == 0)
            {
                MessageBox.Show("Please select a product from the shop first!");
                return;
            }

            int qty = (int)numQuantity.Value;
            int subTotal = currentProductPrice * qty;

            // Add the item to our virtual cart table
            virtualCart.Rows.Add(currentProductID, currentProductName, currentProductPrice, qty, subTotal);

            // Update the Total Price label
            cartTotal += subTotal;
            lblTotal.Text = "Total: ₹" + cartTotal.ToString(); // Using ₹ as per your report mockup

            // Reset selection
            currentProductID = 0;
            numQuantity.Value = 1;
        }

        // --- CHECKOUT BUTTON ---
        private void btnCheckout_Click(object sender, EventArgs e)
        {
            if (virtualCart.Rows.Count == 0)
            {
                MessageBox.Show("Your cart is empty!");
                return;
            }

            // Open the Payment form and pass the cart and the total!
            OrderPaymentForm paymentForm = new OrderPaymentForm(virtualCart, cartTotal);
            paymentForm.Show();
            this.Hide();
        }

        // --- LOAD PAST ORDERS ---
        private void LoadOrderHistory()
        {
            try
            {
                DatabaseConnection db = new DatabaseConnection();
                // Fetch the customer's orders from the database
                string query = "SELECT OrderID, TotalPrice, Status, PaymentMethod FROM Orders WHERE CustomerID = 1";
                DataTable dt = db.ExecuteQuery(query);

                // Bind it to your new grid!
                dgvOrders.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading order history: " + ex.Message);
            }
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to log out?", "Confirm Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                // 1. Create a fresh instance of the Login form
                Form1 loginForm = new Form1();

                // 2. Show the Login form
                loginForm.Show();

                // 3. Completely close the current dashboard
                this.Close();
            }
        }
    }
}
