using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Commerce.Domain.Contracts;
using Commerce.Application.Dtos;
using Commerce.Domain;
using Commerce.Domain.Identity;
using Commerce.Persistence.Contracts;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Commerce.Application;

namespace Commerce.Application
{
    public class ShopService : IShopService
    {
        private readonly ICommonPersist _commonPersist;
        private readonly IShopPersist _shopPersist;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public ShopService(ICommonPersist commonPersist, IShopPersist shopPersist, UserManager<User> userManager, IMapper mapper)
        {
            _commonPersist = commonPersist;
            _shopPersist = shopPersist;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<ShopDto> AddAsync(ShopDto model)
        {
            try
            {
                var loja = _mapper.Map<Domain.Shop>(model);
                
                _commonPersist.Add(loja);
                if (await _commonPersist.SaveChangesAsync())
                {
                    var lojaRetorno = await _shopPersist.GetByIdAsync(loja.Id);
                    return _mapper.Map<ShopDto>(lojaRetorno);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Message: {ex.Message} InnerException: {ex.InnerException}");
            }
        }

        public async Task<ShopDto> UpdateAsync(int id, ShopDto model)
        {
            try
            {
                var loja = await _shopPersist.GetByIdAsync(id);
                if (loja == null) return null;

                model.Id = loja.Id;

                _mapper.Map(model, loja);

                _commonPersist.Update(loja);

                if (await _commonPersist.SaveChangesAsync())
                {
                    var lojaRetorno = await _shopPersist.GetByIdAsync(loja.Id);
                    return _mapper.Map<ShopDto>(lojaRetorno);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Message: {ex.Message} InnerException: {ex.InnerException}");
            }
        }

        public async Task<bool> DeleteAsync(int lojaId)
        {
            try
            {
                var loja = await _shopPersist.GetByIdAsync(lojaId);
                if (loja == null) throw new Exception("Loja não encontrada.");

                _commonPersist.Delete(loja);
                return await _commonPersist.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ShopDto> GetByIdAsync(int lojaId)
        {
            try
            {
                var loja = await _shopPersist.GetByIdAsync(lojaId);
                if (loja == null) return null;

                var resultado = _mapper.Map<ShopDto>(loja);
                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception($"Message: {ex.Message} InnerException: {ex.InnerException}");
            }
        }

        public async Task<PagedModelDto> GetAsync(string name, int limit, int page, CancellationToken cancellationToken)
        {
            try
            {
                var shops = await _shopPersist.GetAsync(name, limit, page, cancellationToken);
                if (shops == null) return null;

                return new ShopListDto
                {
                    CurrentPage = shops.CurrentPage,
                    TotalPages = shops.TotalPages,
                    TotalItems = shops.TotalItems,
                    Items = shops.Items.ToList()
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Message: {ex.Message} InnerException: {ex.InnerException}");
            }
        }
    }
}
