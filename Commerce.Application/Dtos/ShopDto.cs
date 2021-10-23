using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Commerce.Application.Dtos
{
    [DisplayName("Shop")]
    public class ShopDto
    {

        public int Id { get; set; }

        [Required]
        public string Cnpj { get; set; }

        [Required]
        public string Name { get; set; }
        public string Desc { get; set; }

        [Required]
        public string Email { get; set; }

        public List<CategoryDto> Categories { get; set; }
        public string ImageUrl { get; set; }

        public IFormFile ImageFile { get; set; }
        public string ColorTheme { get; set; }
    }
}
