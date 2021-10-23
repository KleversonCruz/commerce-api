using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Commerce.Application.Dtos
{
    [DisplayName("OrderItem")]
    public class OrderItemDto
    {
        public int Quantity { get; set; }
        public int ProductId { get; set; }
        public ProductDto Product { get; set; }
        public int OrderId { get; set; }
        public OrderDto Order { get; set; }
    }
}