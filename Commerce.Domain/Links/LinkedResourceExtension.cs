using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commerce.Domain.Links
{
    public static class LinkedResourceExtension
    {
        public static void AddResourceLink(this ILinkedResource resources,
                  LinkedResourceType resourceType,
                  string routeUrl)
        {
            resources.Links ??= new Dictionary<LinkedResourceType, LinkedResource>();
            resources.Links[resourceType] = new LinkedResource(routeUrl);
        }
    }

}
