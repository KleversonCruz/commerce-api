using AutoMapper;
using Commerce.Application.Dtos;
using Commerce.Domain;
using Commerce.Domain.Contracts;
using Commerce.Domain.Identity;
using Commerce.Persistence.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Commerce.Application
{
    public class ClienteService : ICustomerService
    {
        private readonly ICommonPersist _commonPersist;
        private readonly ICustomerPersist _customerPersist;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<User> _userManager;

        public ClienteService(ICommonPersist commonPersist, ICustomerPersist customerPersist, IMapper mapper, IHttpContextAccessor httpContextAccessor, UserManager<User> userManager)
        {
            _commonPersist = commonPersist;
            _customerPersist = customerPersist;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        public async Task<CustomerDto> AddAsync(CustomerDto model)
        {
            try
            {
                var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = await _userManager.FindByIdAsync(userId);
                var cliente = _mapper.Map<Customer>(model);

                _commonPersist.Add(cliente);
                if (await _commonPersist.SaveChangesAsync())
                {
                    var clienteRetorno = await _customerPersist.GetByIdAsync(cliente.Id);
                    return _mapper.Map<CustomerDto>(clienteRetorno);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Message: {ex.Message} InnerException: {ex.InnerException}");
            }
        }

        public async Task<CustomerDto> UpdateAsync(int customerId, CustomerDto model)
        {
            try
            {
                var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = await _userManager.FindByIdAsync(userId);

                var cliente = await _customerPersist.GetByIdAsync(customerId);
                if (cliente == null) return null;

                model.Id = cliente.Id;
                _mapper.Map(model, cliente);

                _commonPersist.Update(cliente);
                if (await _commonPersist.SaveChangesAsync())
                {
                    var clienteRetorno = await _customerPersist.GetByIdAsync(cliente.Id);
                    return _mapper.Map<CustomerDto>(clienteRetorno);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Message: {ex.Message} InnerException: {ex.InnerException}");
            }
        }

        public async Task<bool> DeleteAsync(int customerId)
        {
            try
            {
                var cliente = await _customerPersist.GetByIdAsync(customerId);
                if (cliente == null) throw new Exception("Cliente não encontrado.");

                _commonPersist.Delete(cliente);
                return await _commonPersist.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Message: {ex.Message} InnerException: {ex.InnerException}");
            }
        }

        public async Task<CustomerDto> GetByIdAsync(int customerId)
        {
            try
            {
                var cliente = await _customerPersist.GetByIdAsync(customerId);
                if (cliente == null) return null;

                var resultado = _mapper.Map<CustomerDto>(cliente);

                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception($"Message: {ex.Message} InnerException: {ex.InnerException}");
            }
        }

        public async Task<PagedModelDto> GetAsync(int shopId, string name, int limit, int page, CancellationToken cancellationToken)
        {
            try
            {
                var customers = await _customerPersist.GetAsync(shopId, name, limit, page, cancellationToken);
                if (customers == null) return null;

                return new CustomerListDto
                {
                    CurrentPage = customers.CurrentPage,
                    TotalPages = customers.TotalPages,
                    TotalItems = customers.TotalItems,
                    Items = customers.Items.ToList()
                };

            }
            catch (Exception ex)
            {
                throw new Exception($"Message: {ex.Message} InnerException: {ex.InnerException}");
            }
        }
    }
}
