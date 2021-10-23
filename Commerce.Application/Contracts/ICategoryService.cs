using Commerce.Application;
using Commerce.Application.Dtos;
using Commerce.Domain;
using System.Threading;
using System.Threading.Tasks;

namespace Commerce.Domain.Contracts
{
    public interface ICategoryService
    {
        Task<CategoryDto> AddAsync(CategoryDto model);
        Task<CategoryDto> UpdateAsync(int categoryId, CategoryDto model);
        Task<bool> DeleteAsync(int categoryId);
        Task<CategoryDto> GetByIdAsync(int shopId);
        Task<PagedModelDto> GetAsync(int shopId, string name, bool? isActive, int limit, int page, CancellationToken cancellationToken);
    }
}
