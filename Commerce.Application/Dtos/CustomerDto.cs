using System.Collections.Generic;
using System.ComponentModel;

namespace Commerce.Application.Dtos
{
    [DisplayName("Customer")]
    public class CustomerDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Telephone { get; set; }
        public string Email { get; set; }
        public IEnumerable<AddressDto> Addresses { get; set; }
    }
}