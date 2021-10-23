using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Commerce.Application.Dtos
{
    [DisplayName("Category")]
    public class CategoryDto
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Desc { get; set; }
        public string ImageUrl { get; set; }
        public bool IsActive { get; set; }
        public int ShopId { get; set; }
        public IFormFile ImageFile { get; set; }
    }
}