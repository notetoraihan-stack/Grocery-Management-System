using System;

namespace GroceryManagementSystem.Models
{
    public class Admin : User
    {
        public int AdminID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}
