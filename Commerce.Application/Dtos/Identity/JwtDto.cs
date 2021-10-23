using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Commerce.Application.Dtos
{
    public class JwtDto
    {
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string Token { get; set; }
    }
}
