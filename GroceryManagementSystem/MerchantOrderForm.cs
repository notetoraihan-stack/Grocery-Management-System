using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace GroceryManagementSystem
{
    public partial class MerchantOrderForm : Form
    {
        int selectedOrderID = 0;

        public MerchantOrderForm()
        {
            InitializeComponent();
            LoadMerchantOrders();
        }

        // --- LOAD ONLY THIS MERCHANT'S ORDERS ---
        private void LoadMerchantOrders()
        {
            try
            {
                DatabaseConnection db = new DatabaseConnection();
                // We use INNER JOIN to link the tables. 
                // We use 'MerchantID = 1' because that is your seeded test merchant.
                string query = @"SELECT DISTINCT o.OrderID, o.CustomerID, o.TotalPrice, o.Status, o.PaymentMethod 
                                 FROM Orders o
                                 INNER JOIN OrderProduct op ON o.OrderID = op.OrderID
                                 INNER JOIN Product p ON op.ProductID = p.ProductID
                                 WHERE p.MerchantID = 1";

                DataTable dt = db.ExecuteQuery(query);
                dgvOrders.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading orders: " + ex.Message);
            }
        }


        // --- UPDATE STATUS ---
        private void btnUpdateStatus_Click(object sender, EventArgs e)
        {
            if (selectedOrderID == 0 || cmbStatus.SelectedItem == null)
            {
                MessageBox.Show("Please select an order and a new status.");
                return;
            }

            try
            {
                DatabaseConnection db = new DatabaseConnection();
                string query = "UPDATE Orders SET Status = @status WHERE OrderID = @oid";
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@status", cmbStatus.SelectedItem.ToString()),
                    new SqlParameter("@oid", selectedOrderID)
                };

                db.ExecuteNonQuery(query, parameters);
                MessageBox.Show("Order Status Updated to " + cmbStatus.SelectedItem.ToString() + "!");
                LoadMerchantOrders(); // Refresh the grid
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating status: " + ex.Message);
            }
        }

        // --- DELETE ORDER ---
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectedOrderID == 0)
            {
                MessageBox.Show("Please select an order to delete.");
                return;
            }

            DialogResult dialogResult = MessageBox.Show("Are you sure you want to completely delete this order?", "Delete Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dialogResult == DialogResult.Yes)
            {
                try
                {
                    DatabaseConnection db = new DatabaseConnection();

                    // CRITICAL: You MUST delete the items from OrderProduct first because of Foreign Keys!
                    string opQuery = "DELETE FROM OrderProduct WHERE OrderID = @oid";
                    string oQuery = "DELETE FROM Orders WHERE OrderID = @oid";

                    SqlParameter[] param1 = new SqlParameter[] { new SqlParameter("@oid", selectedOrderID) };
                    SqlParameter[] param2 = new SqlParameter[] { new SqlParameter("@oid", selectedOrderID) };

                    db.ExecuteNonQuery(opQuery, param1);
                    db.ExecuteNonQuery(oQuery, param2);

                    MessageBox.Show("Order Deleted.");
                    selectedOrderID = 0; // Reset
                    LoadMerchantOrders(); // Refresh the grid
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error deleting order: " + ex.Message);
                }
            }
        }

        private void dgvOrders_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvOrders.Rows[e.RowIndex];
                selectedOrderID = Convert.ToInt32(row.Cells["OrderID"].Value);
                cmbStatus.Text = row.Cells["Status"].Value.ToString();
            }
        }
    }
}