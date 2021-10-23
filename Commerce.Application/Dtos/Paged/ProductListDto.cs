using Commerce.Domain;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Commerce.Application
{
    public record ProductListDto : PagedModelDto
    {
        public List<Product> Items { get; init; }

    }

}