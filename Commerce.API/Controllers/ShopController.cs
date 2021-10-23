using Commerce.Application;
using Commerce.Application.Dtos;
using Commerce.Domain.Contracts;
using Commerce.Domain.Links;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Commerce.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ShopController : ControllerBase
    {
        private readonly IShopService _shopService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ShopController(IShopService shopService, IWebHostEnvironment hostEnvironment)
        {
            _shopService = shopService;
            _webHostEnvironment = hostEnvironment;
        }

        public record UrlQueryParameters
        {
            public string Name { get; init; }
            public int Limit { get; init; } = 20;
            public int Page { get; init; } = 1;
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetShopById(int id)
        {
            try
            {
                var loja = await _shopService.GetByIdAsync(id);
                if (loja == null) return NoContent();

                return Ok(loja);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar recuperar lojas. Erro: {ex.Message}");
            }
        }

        [HttpGet(Name = nameof(GetShop))]
        [AllowAnonymous]
        public async Task<IActionResult> GetShop([FromQuery] UrlQueryParameters urlQueryParameters, CancellationToken cancellationToken)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                var shops = await _shopService.GetAsync(
                    urlQueryParameters.Name,
                    urlQueryParameters.Limit,
                    urlQueryParameters.Page,
                    cancellationToken
                );

                return Ok(GeneratePageLinks(urlQueryParameters, shops));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar recuperar produtos. Erro: {ex.Message}");
            }
        }

        private PagedModelDto GeneratePageLinks(UrlQueryParameters queryParameters, PagedModelDto response)
        {

            if (response.CurrentPage > 1)
            {
                var prevRoute = Url.RouteUrl(nameof(GetShop), new { limit = queryParameters.Limit, page = queryParameters.Page - 1 });

                response.AddResourceLink(LinkedResourceType.Prev, prevRoute);

            }

            if (response.CurrentPage < response.TotalPages)
            {
                var nextRoute = Url.RouteUrl(nameof(GetShop), new { limit = queryParameters.Limit, page = queryParameters.Page + 1 });

                response.AddResourceLink(LinkedResourceType.Next, nextRoute);
            }

            return response;
        }



        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> AddShop([FromForm] ShopDto model)
        {
            try
            {
                model.ImageUrl = GenerateFileName(model.ImageFile);
                var loja = await _shopService.AddAsync(model);
                if (loja == null) return NoContent();

                UploadedFile(model.ImageFile, model.ImageUrl);

                return Ok(loja);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar adicionar loja. Erro: {ex.Message}");
            }
        }

        [Authorize(Roles = "loja,admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateShop(int id, [FromForm] ShopDto model)
        {
            try
            {
                if (!string.IsNullOrEmpty(GenerateFileName(model.ImageFile)))
                {
                    model.ImageUrl = GenerateFileName(model.ImageFile);
                }

                var loja = await _shopService.UpdateAsync(id, model);

                if (loja == null) return NoContent();
                UploadedFile(model.ImageFile, model.ImageUrl);

                return Ok(loja);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar atualizar loja. Erro: {ex.Message}");
            }
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteShop(int id)
        {
            try
            {
                var loja = await _shopService.GetByIdAsync(id);
                if (loja == null) return NoContent();

                if (await _shopService.DeleteAsync(id))
                {
                    return Ok();
                }
                else
                {
                    throw new Exception("Ocorreu um problem não específico ao tentar deletar Loja.");
                }
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar deletar lojas. Erro: {ex.Message}");
            }
        }

        private void UploadedFile(IFormFile image, string filename)
        {
            if (image != null && !string.IsNullOrWhiteSpace(filename))
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                string filePath = Path.Combine(uploadsFolder, filename);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    image.CopyTo(fileStream);
                }
            }
        }

        private string GenerateFileName(IFormFile image)
        {
            string uniqueFileName = null;
            if (image != null)
            {
                uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
            }
            return uniqueFileName;
        }
    }
}
