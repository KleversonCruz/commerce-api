using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Commerce.Application.Dtos
{
    public class UserDto
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string Password { get; set; }
        public int? ShopId { get; set; }
        public ShopDto Shop { get; set; }

        public int? CustomerId { get; set; }
        public CustomerDto Customer { get; set; }

        public List<UserRoleDto> UserRoles { get; set; }
    }
}
