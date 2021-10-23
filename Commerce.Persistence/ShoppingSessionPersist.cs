using Microsoft.EntityFrameworkCore;
using Commerce.Domain;
using Commerce.Persistence.Context;
using Commerce.Persistence.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commerce.Persistence
{
    public class ShoppingSessionPersist : IShoppingSessionPersist
    {
        private readonly ShopContext _context;
        public ShoppingSessionPersist(ShopContext context)
        {
            _context = context;
        }

        public async Task<ShoppingSession> GetByCustomerAndShopAsync(int shopId, int customerId)
        {
            IQueryable<ShoppingSession> query = _context.ShoppingSessions.AsNoTracking()
                .Include(c => c.CartItems)
                .ThenInclude(p => p.Product);

            query = query.Where(e => e.ShopId == shopId
                                  && e.CustomerId == customerId);

            var session = await query.FirstOrDefaultAsync();
            calcTotal(ref session);
            return session;
        }

        public async Task<ShoppingSession> GetByIdAsync(int ShoppingSessionId)
        {
            IQueryable<ShoppingSession> query = _context.ShoppingSessions.AsNoTracking()
                .Include(c => c.CartItems)
                .ThenInclude(p => p.Product);

            query = query.AsNoTracking().OrderBy(e => e.Id)
                         .Where(e => e.Id == ShoppingSessionId);

            var session = await query.FirstOrDefaultAsync();
            calcTotal(ref session);

            return session;
        }

        private void calcTotal(ref ShoppingSession session)
        {
            if (session == null)
                return;
            session.Total = session.CartItems.Sum(i => i.Product.Price * i.Quantity);
        }

    }
}
