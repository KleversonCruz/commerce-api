using Commerce.Domain;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Commerce.Application
{
    public record CustomerListDto : PagedModelDto
    {
        public List<Customer> Items { get; init; }

    }

}