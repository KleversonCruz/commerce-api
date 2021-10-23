using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Commerce.Application.Dtos
{
    public class ShoppingSessionDto
    {
        public int Id { get; set; }
        public double Total { get; set; }
        public int CustomerId { get; set; }
        public int ShopId { get; set; }
        public IEnumerable<CartItemDto> CartItems { get; set; }
    }
}