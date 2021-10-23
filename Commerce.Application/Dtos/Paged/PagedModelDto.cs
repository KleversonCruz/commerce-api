using Commerce.Domain;
using Commerce.Domain.Links;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Commerce.Application
{
    public record PagedModelDto: ILinkedResource
    {
        public int CurrentPage { get; init; }

        public int TotalItems { get; init; }

        public int TotalPages { get; init; }


        public IDictionary<LinkedResourceType, LinkedResource> Links { get; set; }

    }

}