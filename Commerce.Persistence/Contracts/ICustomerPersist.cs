using Commerce.Domain;
using Commerce.Domain.Pagination;
using System.Threading;
using System.Threading.Tasks;

namespace Commerce.Persistence.Contracts
{
    public interface ICustomerPersist
    {
        Task<Customer> GetByIdAsync(int customerId);
        Task<PagedModel<Customer>> GetAsync(int shopId, string name, int limit, int page, CancellationToken cancellationToken);
    }
}
