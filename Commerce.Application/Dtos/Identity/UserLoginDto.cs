using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commerce.Application.Dtos
{
    [DisplayName("UserLogin")]
    public class UserLoginDto
    {
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public string Password { get; set; }
    }
}
