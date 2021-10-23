using Commerce.Application;
using Commerce.Application.Dtos;
using System.Threading;
using System.Threading.Tasks;

namespace Commerce.Domain.Contracts
{
    public interface ICustomerService
    {
        Task<CustomerDto> AddAsync(CustomerDto model);
        Task<CustomerDto> UpdateAsync(int id, CustomerDto model);
        Task<bool> DeleteAsync(int customerId);

        Task<CustomerDto> GetByIdAsync(int customerId);
        Task<PagedModelDto> GetAsync(int shopId, string name, int limit, int page, CancellationToken cancellationToken);
    }
}
