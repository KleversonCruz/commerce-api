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
    public class ProductService : IProductService
    {
        private readonly ICommonPersist _commonPersist;
        private readonly IProductPersist _productPersist;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<User> _userManager;

        public ProductService(ICommonPersist commonPersist, IProductPersist productPersist, IMapper mapper, IHttpContextAccessor httpContextAccessor, UserManager<User> userManager)
        {
            _commonPersist = commonPersist;
            _productPersist = productPersist;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        public async Task<ProductDto> AddAsync(ProductDto model)
        {
            try
            {
                var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = await _userManager.FindByIdAsync(userId);

                model.ShopId = (int)user.ShopId;
                var product = _mapper.Map<Product>(model);

                _commonPersist.Add(product);
                if (await _commonPersist.SaveChangesAsync())
                {
                    var ProductRetorno = await _productPersist.GetByIdAsync(product.Id);
                    return _mapper.Map<ProductDto>(ProductRetorno);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Message: {ex.Message} InnerException: {ex.InnerException}");
            }
        }

        public async Task<ProductDto> UpdateAsync(int productId, ProductDto model)
        {
            try
            {
                var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = await _userManager.FindByIdAsync(userId);
                model.ShopId = (int)user.ShopId;

                var product = await _productPersist.GetByIdAsync(productId);
                if (product == null) return null;

                model.Id = product.Id;
                _mapper.Map(model, product);

                _commonPersist.Update(product);
                if (await _commonPersist.SaveChangesAsync())
                {
                    var ProductRetorno = await _productPersist.GetByIdAsync(product.Id);
                    return _mapper.Map<ProductDto>(ProductRetorno);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Message: {ex.Message} InnerException: {ex.InnerException}");
            }
        }

        public async Task<bool> DeleteAsync(int productId)
        {
            try
            {
                var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = await _userManager.FindByIdAsync(userId);

                var Product = await _productPersist.GetByIdAsync(productId);
                if (Product == null) throw new Exception("Product não encontrado.");

                _commonPersist.Delete<Product>(Product);
                return await _commonPersist.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Message: {ex.Message} InnerException: {ex.InnerException}");
            }
        }

        public async Task<ProductDto> GetByIdAsync(int productId)
        {
            try
            {
                var Product = await _productPersist.GetByIdAsync(productId);
                if (Product == null) return null;

                var resultado = _mapper.Map<ProductDto>(Product);

                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception($"Message: {ex.Message} InnerException: {ex.InnerException}");
            }
        }

        public async Task<PagedModelDto> GetAsync(int shopId, string name, int categoryId, bool? isActive, int limit, int page, CancellationToken cancellationToken)
        {
            try
            {
                var products = await _productPersist.GetAsync(shopId, name, categoryId, isActive, limit, page, cancellationToken);
                if (products == null) return null;

                return new ProductListDto
                {
                    CurrentPage = products.CurrentPage,
                    TotalPages = products.TotalPages,
                    TotalItems = products.TotalItems,
                    Items = products.Items.ToList()
                };

            }
            catch (Exception ex)
            {
                throw new Exception($"Message: {ex.Message} InnerException: {ex.InnerException}");
            }
        }
    }
}
