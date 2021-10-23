using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Commerce.Domain.Contracts;
using Commerce.Application.Dtos;
using Commerce.Domain;
using Commerce.Domain.Identity;
using Commerce.Persistence.Contracts;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;

namespace Commerce.Application
{
    public class CategoryService : ICategoryService
    {
        private readonly ICommonPersist _commonPersist;
        private readonly ICategoryPersist _categoryPersist;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<User> _userManager;

        public CategoryService(ICommonPersist commonPersist, ICategoryPersist categoryPersist, IMapper mapper, IHttpContextAccessor httpContextAccessor, UserManager<User> userManager)
        {
            _commonPersist = commonPersist;
            _categoryPersist = categoryPersist;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }
        public async Task<CategoryDto> AddAsync(CategoryDto model)
        {
            try
            {
                var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = await _userManager.FindByIdAsync(userId);

                model.ShopId = (int)user.ShopId;
                var category = _mapper.Map<Category>(model);

                _commonPersist.Add(category);
                if (await _commonPersist.SaveChangesAsync())
                {
                    var categoriaRetorno = await _categoryPersist.GetByIdAsync(category.Id);
                    return _mapper.Map<CategoryDto>(categoriaRetorno);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Message: {ex.Message} InnerException: {ex.InnerException}");
            }
        }

        public async Task<CategoryDto> UpdateAsync(int categoriaId, CategoryDto model)
        {
            try
            {
                var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = await _userManager.FindByIdAsync(userId);
                model.ShopId = (int)user.ShopId;

                var category = await _categoryPersist.GetByIdAsync(categoriaId);
                if (category == null) return null;

                model.Id = category.Id;
                _mapper.Map(model, category);

                _commonPersist.Update(category);
                if (await _commonPersist.SaveChangesAsync())
                {
                    var categoryResult = await _categoryPersist.GetByIdAsync(category.Id);
                    return _mapper.Map<CategoryDto>(categoryResult);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Message: {ex.Message} InnerException: {ex.InnerException}");
            }
        }

        public async Task<bool> DeleteAsync(int categoriaId)
        {
            try
            {
                var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = await _userManager.FindByIdAsync(userId);

                var category = await _categoryPersist.GetByIdAsync(categoriaId);
                if (category == null) throw new Exception("Categoria não encontrado.");

                _commonPersist.Delete<Category>(category);
                return await _commonPersist.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Message: {ex.Message} InnerException: {ex.InnerException}");
            }
        }

        public async Task<CategoryDto> GetByIdAsync(int categoriaId)
        {
            try
            {
                var category = await _categoryPersist.GetByIdAsync(categoriaId);
                if (category == null) return null;

                var resultado = _mapper.Map<CategoryDto>(category);

                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception($"Message: {ex.Message} InnerException: {ex.InnerException}");
            }
        }

        public async Task<PagedModelDto> GetAsync(int shopId, string name, bool? isActive, int limit, int page, CancellationToken cancellationToken)
        {
            try
            {
                var categories = await _categoryPersist.GetAsync(shopId, name, isActive, limit, page, cancellationToken);
                if (categories == null || categories == null) return null;

                return new CategoryListDto
                {
                    CurrentPage = categories.CurrentPage,
                    TotalPages = categories.TotalPages,
                    TotalItems = categories.TotalItems,
                    Items = categories.Items.ToList()
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Message: {ex.Message} InnerException: {ex.InnerException}");
            }
        }
    }
}
