using System.Collections.Generic;

namespace Commerce.Domain
{
    public class ShoppingSession
    {
        public int Id { get; set; }
        public double Total { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        public int ShopId { get; set; }
        public Shop Shop { get; set; }
        public ICollection<CartItem> CartItems { get; set; }
    }
}
