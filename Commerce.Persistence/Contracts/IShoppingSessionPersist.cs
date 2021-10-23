using Commerce.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commerce.Persistence.Contracts
{
    public interface IShoppingSessionPersist
    {
        Task<ShoppingSession> GetByCustomerAndShopAsync(int idLoja, int idCliente);
        Task<ShoppingSession> GetByIdAsync(int idCarrinho);
    }
}
