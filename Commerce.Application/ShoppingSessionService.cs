using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Commerce.Domain.Contracts;
using Commerce.Domain;
using Commerce.Domain.Identity;
using Commerce.Persistence.Contracts;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Commerce.Application.Dtos;

namespace Commerce.Application
{
    public class ShoppingSessionService : IShoppingSessionService
    {
        private readonly ICommonPersist _commonPersist;
        private readonly IShoppingSessionPersist _shoppingSessionPersist;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<User> _userManager;

        public ShoppingSessionService(ICommonPersist commonPersist, IShoppingSessionPersist shoppingSessionPersist, IMapper mapper, IHttpContextAccessor httpContextAccessor, UserManager<User> userManager)
        {
            _commonPersist = commonPersist;
            _shoppingSessionPersist = shoppingSessionPersist;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        public async Task<ShoppingSessionDto> AddAsync(ShoppingSessionDto model)
        {
            try
            {
                var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = await _userManager.FindByIdAsync(userId);

                model.CustomerId = (int)user.CustomerId;
                var cart = _mapper.Map<ShoppingSession>(model);

                _commonPersist.Add(cart);
                if (await _commonPersist.SaveChangesAsync())
                {
                    var carrinhoRetorno = await _shoppingSessionPersist.GetByIdAsync(cart.Id);
                    return _mapper.Map<ShoppingSessionDto>(carrinhoRetorno);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Message: {ex.Message} InnerException: {ex.InnerException}");
            }
        }

        public async Task<ShoppingSessionDto> UpdateAsync(int shopId, ShoppingSessionDto model)
        {
            try
            {
                var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = await _userManager.FindByIdAsync(userId);

                var cart = await _shoppingSessionPersist.GetByCustomerAndShopAsync(shopId, (int)user.CustomerId);
                if (cart == null) return null;

                foreach (var item in cart.CartItems)
                {
                    _commonPersist.Delete(item);
                }

                model.Id = cart.Id;
                _mapper.Map(model, cart);

                _commonPersist.Update(cart);
                if (await _commonPersist.SaveChangesAsync())
                {
                    var cartReturn = await _shoppingSessionPersist.GetByIdAsync(cart.Id);
                    return _mapper.Map<ShoppingSessionDto>(cartReturn);
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

                var cart = await _shoppingSessionPersist.GetByCustomerAndShopAsync(shopId, (int)user.CustomerId);
                if (cart == null) throw new Exception("Carrinho não encontrado.");

                _commonPersist.Delete(cart);
                return await _commonPersist.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Message: {ex.Message} InnerException: {ex.InnerException}");
            }
        }

        public async Task<ShoppingSessionDto> GetByCustomerAndShopAsync(int shopId)
        {
            try
            {
                var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = await _userManager.FindByIdAsync(userId);

                var cart = await _shoppingSessionPersist.GetByCustomerAndShopAsync(shopId, (int)user.CustomerId);
                if (cart == null) return null;

                return _mapper.Map<ShoppingSessionDto>(cart);
            }
            catch (Exception ex)
            {
                throw new Exception($"Message: {ex.Message} InnerException: {ex.InnerException}");
            }
        }
    }
}
