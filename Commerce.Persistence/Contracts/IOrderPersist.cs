using Commerce.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commerce.Persistence.Contracts
{
    public interface IOrderPersist
    {
        Task<Order> GetByCustomerAndShopAsync(int shopId, int customerId);
        Task<Order> GetByIdAsync(int OrderId);
    }
}
