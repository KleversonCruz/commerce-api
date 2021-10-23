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
    public class OrderPersist : IOrderPersist
    {
        private readonly ShopContext _context;
        public OrderPersist(ShopContext context)
        {
            _context = context;
        }
        public async Task<Order> GetByCustomerAndShopAsync(int shopId, int customerId)
        {
            IQueryable<Order> query = _context.Orders.AsNoTracking()
                .Include(i => i.OrderItems)
                .ThenInclude(p => p.Product);

            query = query.Where(o => o.ShopId == shopId
                                  && o.CustomerId == customerId);

            var order = await query.FirstOrDefaultAsync();
            calcTotal(ref order);
            return order;
        }

        public async Task<Order> GetByIdAsync(int orderId)
        {
            IQueryable<Order> query = _context.Orders.AsNoTracking()
                .Include(i => i.OrderItems)
                .ThenInclude(p => p.Product);

            query = query.Where(e => e.Id == orderId);

            var order = await query.FirstOrDefaultAsync();
            calcTotal(ref order);

            return order;
        }

        private void calcTotal(ref Order order)
        {
            if (order == null)
                return;
            order.Total = order.OrderItems.Sum(i => i.Product.Price * i.Quantity);
        }
    }
}
