using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Commerce.Domain.Identity
{
    public class Role: IdentityRole<int>
    {
        public List<UserRole> UserRoles { get; set; }
    }
}