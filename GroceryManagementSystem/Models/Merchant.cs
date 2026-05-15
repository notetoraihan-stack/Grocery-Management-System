using System;

namespace GroceryManagementSystem.Models
{
    public class Merchant : User
    {
        public int MerchantID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string ShopAddress { get; set; }
    }
}
