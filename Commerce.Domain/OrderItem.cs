using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Commerce.Domain
{
    public class OrderItem
    {
        public int Quantity { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }

        [Timestamp]
        public byte[] ModifiedAt { get; set; }
    }
}
