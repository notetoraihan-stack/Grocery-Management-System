using System;

namespace GroceryManagementSystem.Models
{
    public class Product
    {
        public int ProductID { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
        public string Category { get; set; }
        public int MerchantID { get; set; }
    }
}
