using System.Collections.Generic;
using System.ComponentModel;

namespace Commerce.Application.Dtos
{
    [DisplayName("Address")]
    public class AddressDto
    {
        public int Id { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Telephone { get; set; }
        public string Mobile { get; set; }
        public int CustomerId { get; set; }
        public CustomerDto Customer { get; set; }
    }
}