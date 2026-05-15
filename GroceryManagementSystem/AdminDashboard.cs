using System;
using System.Data;
using System.Windows.Forms;

namespace GroceryManagementSystem
{
    public partial class AdminDashboard : Form
    {
        public AdminDashboard()
        {
            InitializeComponent();
            LoadSystemData();
        }

        private void LoadSystemData()
        {
            try
            {
                DatabaseConnection db = new DatabaseConnection();

                // 1. Load all accounts in the system (Hiding the password column for security)
                string userQuery = "SELECT UserID, Username, Role FROM [User]";
                DataTable usersTable = db.ExecuteQuery(userQuery);
                dgvUsers.DataSource = usersTable;

                // 2. Load every order placed by any customer
                string orderQuery = "SELECT OrderID, CustomerID, TotalPrice, Status, PaymentMethod FROM Orders";
                DataTable ordersTable = db.ExecuteQuery(orderQuery);
                dgvAllOrders.DataSource = ordersTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading admin data: " + ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
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