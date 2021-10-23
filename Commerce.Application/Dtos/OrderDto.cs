using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Commerce.Application.Dtos
{
    [DisplayName("Order")]
    public class OrderDto
    {
        public int Id { get; set; }
        public double Total { get; set; }
        public int CustomerId { get; set; }
        public CustomerDto Customer { get; set; }
        public IEnumerable<OrderItemDto> OrderItems { get; set; }
        public int ShopId { get; set; }
    }
}