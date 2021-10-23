using Commerce.Domain;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Commerce.Application
{
    public record CategoryListDto : PagedModelDto
    {
        public List<Category> Items { get; init; }

    }

}