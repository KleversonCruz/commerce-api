using Microsoft.EntityFrameworkCore;
using Commerce.Domain;
using Commerce.Persistence.Context;
using Commerce.Persistence.Contracts;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using Commerce.Domain.Pagination;

namespace Commerce.Persistence
{
    public class CustomerPersist : ICustomerPersist
    {
        private readonly ShopContext _context;
        public CustomerPersist(ShopContext context)
        {
            _context = context;
        }
        public async Task<Customer> GetByIdAsync(int customerId)
        {
            IQueryable<Customer> query = _context.Customers.AsNoTracking()
                .Include(a => a.Addresses);

            query = query.OrderBy(c => c.Id)
                         .Where(c => c.Id == customerId);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<PagedModel<Customer>> GetAsync(int shopId, string name, int limit, int page, CancellationToken cancellationToken)
        {
            IQueryable<Customer> query = _context.Shops.AsNoTracking()
                .Where(s => s.Id == shopId).SelectMany(c => c.Customers)
                .Include(a => a.Addresses);

            SearchByName(ref query, name);

            var pagedQuery = await query.OrderBy(p => p.FirstName)
                                        .PaginateAsync(page, limit, cancellationToken);

            return pagedQuery;
        }

        private void SearchByName(ref IQueryable<Customer> query, string name)
        {
            if (!query.Any() || string.IsNullOrWhiteSpace(name))
                return;
            query = query.Where(p => p.FirstName.ToLower().Contains(name.Trim().ToLower()) ||
                                     p.LastName.ToLower().Contains(name.Trim().ToLower()));
        }

    }
}
