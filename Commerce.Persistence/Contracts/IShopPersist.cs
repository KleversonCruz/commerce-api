using Commerce.Domain;
using Commerce.Domain.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Commerce.Persistence.Contracts
{
    public interface IShopPersist
    {
        Task<Shop> GetByIdAsync(int shopId);
        Task<PagedModel<Shop>> GetAsync(string name, int limit, int page, CancellationToken cancellationToken);
    }
}
