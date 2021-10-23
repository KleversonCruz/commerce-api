using Microsoft.EntityFrameworkCore;
using Commerce.Domain;
using Commerce.Persistence.Context;
using Commerce.Persistence.Contracts;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Commerce.Domain.Pagination;

namespace Commerce.Persistence
{
    public class ShopPersist : IShopPersist
    {
        private readonly ShopContext _context;
        public ShopPersist(ShopContext context)
        {
            _context = context;
        }

        public async Task<Domain.Shop> GetByIdAsync(int shopId)
        {
            IQueryable<Domain.Shop> query = _context.Shops.AsNoTracking()
                .Include(c => c.Categories);

            query = query.Where(loja => loja.Id == shopId);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<PagedModel<Shop>> GetAsync(string name, int limit, int page, CancellationToken cancellationToken)
        {
            IQueryable<Domain.Shop> query = _context.Shops.AsNoTracking();

            SearchByName(ref query, name);

            var pagedQuery = await query.OrderBy(p => p.Name)
                                        .PaginateAsync(page, limit, cancellationToken);

            return pagedQuery;
        }

        private void SearchByName(ref IQueryable<Domain.Shop> query, string name)
        {
            if (!query.Any() || string.IsNullOrWhiteSpace(name))
                return;
            query = query.Where(p => p.Name.ToLower().Contains(name.Trim().ToLower()));
        }
    }
}
