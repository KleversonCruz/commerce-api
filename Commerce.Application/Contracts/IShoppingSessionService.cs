using Commerce.Application.Dtos;
using Commerce.Domain;
using System.Threading.Tasks;

namespace Commerce.Domain.Contracts
{
    public interface IShoppingSessionService
    {
        Task<ShoppingSessionDto> AddAsync(ShoppingSessionDto model);
        Task<ShoppingSessionDto> UpdateAsync(int shopId, ShoppingSessionDto model);
        Task<bool> DeleteAsync(int shopId);
        Task<ShoppingSessionDto> GetByCustomerAndShopAsync(int lojaID);
    }
}
