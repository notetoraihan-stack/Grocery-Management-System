using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GroceryManagementSystem
{
    public partial class MerchantDashboard : Form
    {
        public MerchantDashboard()
        {
            InitializeComponent();
        }


        private void button1_Click_1(object sender, EventArgs e)
        {
            try
            {
                // 1. Initialize it empty so it uses the perfect string  saved in the class!
                DatabaseConnection dbCon = new DatabaseConnection();

                // 2. The query
                string query = "SELECT * FROM Product";

                // 3. Execute and bind to the grid
                DataTable dt = dbCon.ExecuteQuery(query);
                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading products: " + ex.Message);
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Ensure the user clicked an actual row, not the header at the very top
            if (e.RowIndex >= 0)
            {
                // Grab the specific row that was clicked
                DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];

                // Fill the text boxes with the data from that row's cells!
                textBox1.Text = row.Cells["ProductID"].Value.ToString();
                textBox2.Text = row.Cells["Name"].Value.ToString();
                textBox3.Text = row.Cells["Price"].Value.ToString();
                textBox4.Text = row.Cells["Quantity"].Value.ToString();
                textBox5.Text = row.Cells["Category"].Value.ToString();
                textBox6.Text = row.Cells["MerchantID"].Value.ToString();
            }
        }


        private void RefreshGrid()
        {
            DatabaseConnection db = new DatabaseConnection();
            DataTable dt = db.ExecuteQuery("SELECT * FROM Product");
            dataGridView1.DataSource = dt;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                DatabaseConnection db = new DatabaseConnection();
                string query = "INSERT INTO Product (ProductID, Name, Price, Quantity, Category, MerchantID) " +
                               "VALUES (@id, @name, @price, @qty, @cat, @mid)";

                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@id", textBox1.Text.Trim()),
                    new SqlParameter("@name", textBox2.Text.Trim()),
                    new SqlParameter("@price", textBox3.Text.Trim()),
                    new SqlParameter("@qty", textBox4.Text.Trim()),
                    new SqlParameter("@cat", textBox5.Text.Trim()),
                    new SqlParameter("@mid", textBox6.Text.Trim()) // Use '1' for seeded merchant
                };

                db.ExecuteNonQuery(query, parameters);
                MessageBox.Show("Product Added Successfully!");
                RefreshGrid(); // Automatically update the grid!
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Adding Product: " + ex.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                DatabaseConnection db = new DatabaseConnection();
                // We update based on the ProductID
                string query = "UPDATE Product SET Name=@name, Price=@price, Quantity=@qty, Category=@cat, MerchantID=@mid WHERE ProductID=@id";

                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@id", textBox1.Text.Trim()),
                    new SqlParameter("@name", textBox2.Text.Trim()),
                    new SqlParameter("@price", textBox3.Text.Trim()),
                    new SqlParameter("@qty", textBox4.Text.Trim()),
                    new SqlParameter("@cat", textBox5.Text.Trim()),
                    new SqlParameter("@mid", textBox6.Text.Trim())
                };

                int rowsAffected = db.ExecuteNonQuery(query, parameters);
                if (rowsAffected > 0)
                    MessageBox.Show("Product Updated Successfully!");
                else
                    MessageBox.Show("No product found with that ID.");

                RefreshGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Updating Product: " + ex.Message);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                // Deleting only requires the ProductID!
                DatabaseConnection db = new DatabaseConnection();
                string query = "DELETE FROM Product WHERE ProductID=@id";

                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@id", textBox1.Text.Trim())
                };

                int rowsAffected = db.ExecuteNonQuery(query, parameters);
                if (rowsAffected > 0)
                    MessageBox.Show("Product Deleted Successfully!");
                else
                    MessageBox.Show("No product found with that ID.");

                RefreshGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Deleting Product: " + ex.Message);
            }
        }

        private void label5_Click(object sender, EventArgs e)
        {

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

        private void btnManageOrders_Click(object sender, EventArgs e)
        {
            MerchantOrderForm orderForm = new MerchantOrderForm();
            orderForm.ShowDialog();
        }
    }
}
