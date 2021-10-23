using Commerce.Application.Dtos;
using Commerce.Domain;
using System.Threading.Tasks;
using Commerce.Application;
using System.Threading;

namespace Commerce.Domain.Contracts
{
    public interface IShopService
    {
        Task<ShopDto> AddAsync(ShopDto model);
        Task<ShopDto> UpdateAsync(int id, ShopDto model);
        Task<bool> DeleteAsync(int id);

        Task<PagedModelDto> GetAsync(string name, int limit, int page, CancellationToken cancellationToken);
        Task<ShopDto> GetByIdAsync(int id);
    }
}
