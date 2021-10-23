using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Commerce.Domain.Identity
{
    public class User : IdentityUser<int>
    {
        public List<UserRole> UserRoles { get; set; }
        public int? ShopId { get; set; }
        public Shop Shop { get; set; }
        public int? CustomerId { get; set; }
        public Customer Customer { get; set; }
    }
}
