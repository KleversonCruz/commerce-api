namespace Commerce.Application.Dtos
{
    public class CartItemDto
    {
        public int Quantity { get; set; }
        public int ProductId { get; set; }
        public ProductDto Product { get; set; }
        public int ShoppingSessionId { get; set; }
    }
}