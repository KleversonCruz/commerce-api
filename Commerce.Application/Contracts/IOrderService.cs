using Commerce.Application.Dtos;
using Commerce.Domain;
using System.Threading.Tasks;

namespace Commerce.Domain.Contracts
{
    public interface IOrderService
    {
        Task<OrderDto> AddAsync(OrderDto model);
        Task<OrderDto> UpdateAsync(int shopId, OrderDto model);
        Task<bool> DeleteAsync(int shopId);
        Task<OrderDto> GetByCustomerAndShopAsync(int shopId);
    }
}
