using Commerce.Domain;
using Commerce.Domain.Pagination;
using System.Threading;
using System.Threading.Tasks;

namespace Commerce.Persistence.Contracts
{
    public interface IProductPersist
    {
        Task<Product> GetByIdAsync(int ProductId);
        Task<PagedModel<Product>> GetAsync(int shopId, string name, int categoryId, bool? isActive, int limit, int page, CancellationToken cancellationToken);
    }
}
