using Commerce.Domain;
using Commerce.Domain.Pagination;
using Commerce.Persistence.Context;
using Commerce.Persistence.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Commerce.Persistence
{
    public class CategoryPersist : ICategoryPersist
    {
        private readonly ShopContext _context;
        public CategoryPersist(ShopContext context)
        {
            _context = context;
        }

        public async Task<Category> GetByIdAsync(int categoryId)
        {
            IQueryable<Category> query = _context.Categories.AsNoTracking();

            query = query.Where(c => c.Id == categoryId);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<PagedModel<Category>> GetAsync(int shopId, string name, bool? isActive, int limit, int page, CancellationToken cancellationToken)
        {
            IQueryable<Category> query = _context.Categories.AsNoTracking();

            query = query.OrderBy(c => c.Name)
                         .Where(c => c.ShopId == shopId);

            FilterByActive(ref query, isActive);
            SearchByName(ref query, name);
            var pagedQuery = await query.OrderBy(p => p.Name)
                                        .PaginateAsync(page, limit, cancellationToken);

            return pagedQuery;
        }

        private void SearchByName(ref IQueryable<Category> query, string name)
        {
            if (!query.Any() || string.IsNullOrWhiteSpace(name))
                return;
            query = query.Where(p => p.Name.ToLower().Contains(name.Trim().ToLower()));
        }

        private void FilterByActive(ref IQueryable<Category> query, bool? isActive)
        {
            if (!query.Any() || !isActive.HasValue)
                return;
            query = query.Where(p => p.IsActive == isActive);
        }
    }
}
