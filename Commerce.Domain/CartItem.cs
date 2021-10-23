using System;

namespace Commerce.Domain
{
    public class CartItem
    {
        public int Quantity { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int ShoppingSessionId { get; set; }
        public ShoppingSession ShoppingSession { get; set; }
    }
}
