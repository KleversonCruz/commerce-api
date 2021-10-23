using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Commerce.Application.Dtos
{
    [DisplayName("UserRegister")]
    public class UserRegisterDto
    {

        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        public CustomerDto Customer { get; set; }
    }
}
