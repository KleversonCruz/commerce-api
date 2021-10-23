using Microsoft.AspNetCore.Http;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Commerce.Application.Dtos
{
    [DisplayName("Product")]
    public class ProductDto
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public string Desc { get; set; }
        public double Price { get; set; }
        public bool IsActive { get; set; }
        public int? CategoryId { get; set; }
        public CategoryDto Category { get; set; }
        public int ShopId { get; set; }
        public string ImageUrl { get; set; }
        public IFormFile ImageFile { get; set; }
    }
}