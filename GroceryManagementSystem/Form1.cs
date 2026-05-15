using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace GroceryManagementSystem
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {

                // 1. Initialize custom database connection helper
                DatabaseConnection db = new DatabaseConnection();

                // 2. The query to find the exact user
                string query = "SELECT * FROM [User] WHERE Username = @Username AND Password = @Password AND Role = @Role";

                // 3. Setup parameters securely, using .Trim() to strip accidental spaces
                SqlParameter[] parameters = new SqlParameter[]
                {
            new SqlParameter("@Username", textBox1.Text.Trim()),
            new SqlParameter("@Password", textBox2.Text.Trim()),
            new SqlParameter("@Role", textBox3.Text.Trim())
                };

                // 4. Execute the query using helper class
                DataTable result = db.ExecuteQuery(query, parameters);

                // 5. If Rows.Count is greater than 0, a matching user was found
                if (result.Rows.Count > 0)
                {
                    // 1. Get the role from the database result
                    string userRole = result.Rows[0]["Role"].ToString();

                    // 2. Route the user based on their role
                    if (userRole == "Admin")
                    {
                        AdminDashboard adminDash = new AdminDashboard();
                        adminDash.Show(); // Open the Admin screen
                        this.Hide();      // Hide the Login screen
                    }
                    else if (userRole == "Merchant")
                    {
                        MerchantDashboard merchantDash = new MerchantDashboard();
                        merchantDash.Show(); // Open the Merchant screen
                        this.Hide();         // Hide the Login screen
                    }
                    else if (userRole == "Customer")
                    {
                        CustomerDashboard customerDash = new CustomerDashboard();
                        customerDash.Show(); // Open the Customer screen
                        this.Hide();         // Hide the Login screen
                    }
                }
                else
                {
                    MessageBox.Show("Invalid Credentials!");
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

       
    }
}
