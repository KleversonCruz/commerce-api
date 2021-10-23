using Commerce.Domain;
using Commerce.Domain.Pagination;
using System.Threading;
using System.Threading.Tasks;

namespace Commerce.Persistence.Contracts
{
    public interface ICategoryPersist
    {
        Task<Category> GetByIdAsync(int categoryId);
        Task<PagedModel<Category>> GetAsync(int shopId, string name, bool? isActive, int limit, int page, CancellationToken cancellationToken);
    }
}
