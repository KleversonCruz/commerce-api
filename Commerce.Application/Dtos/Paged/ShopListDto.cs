using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Commerce.Application
{
    public record ShopListDto : PagedModelDto
    {
        public List<Domain.Shop> Items { get; init; }

    }

}