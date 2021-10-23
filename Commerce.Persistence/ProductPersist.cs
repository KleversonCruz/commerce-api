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
    public class ProductPersist : IProductPersist
    {
        private readonly ShopContext _context;
        public ProductPersist(ShopContext context)
        {
            _context = context;
        }

        public async Task<Product> GetByIdAsync(int productId)
        {
            IQueryable<Product> query = _context.Products.AsNoTracking()
                .Include(c => c.Category);

            query = query.Where(p => p.Id == productId);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<PagedModel<Product>> GetAsync(int shopId, string name, int categoryId, bool? isActive, int limit, int page, CancellationToken cancellationToken)
        {
            IQueryable<Product> query = _context.Products.AsNoTracking()
                .Include(c => c.Category);

            query = query.OrderBy(p => p.Name)
                         .Where(p => p.ShopId == shopId);

            FilterByActive(ref query, isActive);
            SearchByName(ref query, name);
            FilterByCategory(ref query, categoryId);

            var pagedQuery = await query.OrderBy(p => p.Name)
                                        .PaginateAsync(page, limit, cancellationToken);

            return pagedQuery;
        }

        private void SearchByName(ref IQueryable<Product> query, string name)
        {
            if (!query.Any() || string.IsNullOrWhiteSpace(name))
                return;
            query = query.Where(p => p.Name.ToLower().Contains(name.Trim().ToLower()));
        }

        private void FilterByCategory(ref IQueryable<Product> query, int categoryId)
        {
            if (!query.Any() || categoryId == 0)
                return;
            query = query.Where(p => p.CategoryId == categoryId);
        }

        private void FilterByActive(ref IQueryable<Product> query, bool? isActive)
        {
            if (!query.Any() || !isActive.HasValue)
                return;
            query = query.Where(p => p.IsActive == isActive);
        }
    }
}
