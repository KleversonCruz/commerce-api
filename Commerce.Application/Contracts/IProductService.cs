using Commerce.Application.Dtos;
using Commerce.Domain;
using System.Threading;
using System.Threading.Tasks;
using Commerce.Application;

namespace Commerce.Domain.Contracts
{
    public interface IProductService
    {
        Task<ProductDto> AddAsync(ProductDto model);
        Task<ProductDto> UpdateAsync(int ProductId, ProductDto model);
        Task<bool> DeleteAsync(int ProductId);
        Task<PagedModelDto> GetAsync(int shopId, string name, int categoryId, bool? isActive, int limit, int page, CancellationToken cancellationToken);
        Task<ProductDto> GetByIdAsync(int ProductId);
    }
}
