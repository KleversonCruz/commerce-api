using Commerce.Domain.Contracts;
using Commerce.Domain;
using Commerce.Persistence.Contracts;
using System;
using System.Threading.Tasks;
using Commerce.Application.Dtos;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Commerce.Domain.Identity;
using System.Security.Claims;

namespace Commerce.Application
{
    public class OrderService : IOrderService
    {
        private readonly ICommonPersist _commonPersist;
        private readonly IOrderPersist _orderPersist;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<User> _userManager;
        public OrderService(ICommonPersist commonPersist, IOrderPersist orderPersist, IMapper mapper, IHttpContextAccessor httpContextAccessor, UserManager<User> userManager)
        {
            _orderPersist = orderPersist;
            _commonPersist = commonPersist;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }
        public async Task<OrderDto> AddAsync(OrderDto model)
        {
            try
            {
                var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = await _userManager.FindByIdAsync(userId);

                model.CustomerId = (int)user.CustomerId;
                var order = _mapper.Map<Order>(model);

                _commonPersist.Add(order);
                if (await _commonPersist.SaveChangesAsync())
                {
                    var orderReturn = await _orderPersist.GetByIdAsync(order.Id);
                    return _mapper.Map<OrderDto>(orderReturn);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Message: {ex.Message} InnerException: {ex.InnerException}");
            }
        }

        public async Task<OrderDto> UpdateAsync(int id, OrderDto model)
        {
            try
            {
                var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = await _userManager.FindByIdAsync(userId);

                var order = await _orderPersist.GetByIdAsync(id);
                if (order != null)

                foreach (var item in order.OrderItems)
                {
                    _commonPersist.Delete(item);
                }

                model.Id = order.Id;
                _mapper.Map(model, order);

                _commonPersist.Update(order);
                if (await _commonPersist.SaveChangesAsync())
                {
                    var orderReturn = await _orderPersist.GetByIdAsync(order.Id);
                    return _mapper.Map<OrderDto>(orderReturn);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Message: {ex.Message} InnerException: {ex.InnerException}");
            }
        }

        public async Task<bool> DeleteAsync(int shopId)
        {
            try
            {
                var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = await _userManager.FindByIdAsync(userId);

                var order = await _orderPersist.GetByCustomerAndShopAsync(shopId, (int)user.CustomerId);
                if (order == null) throw new Exception("Pedido não encontrado.");

                _commonPersist.Delete(order);
                return await _commonPersist.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Message: {ex.Message} InnerException: {ex.InnerException}");
            }
        }

        public async Task<OrderDto> GetByCustomerAndShopAsync(int shopId)
        {
            try
            {
                var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = await _userManager.FindByIdAsync(userId);

                var order = await _orderPersist.GetByCustomerAndShopAsync(shopId, (int)user.CustomerId);
                if (order == null) return null;

                return _mapper.Map<OrderDto>(order);
            }
            catch (Exception ex)
            {
                throw new Exception($"Message: {ex.Message} InnerException: {ex.InnerException}");
            }
        }
    }
}
