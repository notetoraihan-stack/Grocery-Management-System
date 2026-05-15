using System;

namespace GroceryManagementSystem.Models
{
    public class Customer : User
    {
        public int CustomerID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
    }
}
