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
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IProductService productService, IWebHostEnvironment hostEnvironment)
        {
            _productService = productService;
            _webHostEnvironment = hostEnvironment;
        }

        public record UrlQueryParameters
        {
            public int ShopId { get; init; }
            public string Name { get; init; }
            public int CategoryId { get; init; }
            public bool? IsActive { get; init; } = null;
            public int Limit { get; init; } = 20;
            public int Page { get; init; } = 1;
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetProductById(int id)
        {
            try
            {
                var product = await _productService.GetByIdAsync(id);
                if (product == null) return NoContent();

                return Ok(product);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar recuperar produtos. Erro: {ex.Message}");
            }
        }

        [HttpGet(Name = nameof(GetProduct))]
        [AllowAnonymous]
        public async Task<IActionResult> GetProduct([FromQuery] UrlQueryParameters urlQueryParameters, CancellationToken cancellationToken)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                var products = await _productService.GetAsync(
                    urlQueryParameters.ShopId,
                    urlQueryParameters.Name,
                    urlQueryParameters.CategoryId,
                    urlQueryParameters.IsActive,
                    urlQueryParameters.Limit,
                    urlQueryParameters.Page,
                    cancellationToken
                );

                return Ok(GeneratePageLinks(urlQueryParameters, products));
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
                var prevRoute = Url.RouteUrl(nameof(GetProduct), new { limit = queryParameters.Limit, page = queryParameters.Page - 1 });

                response.AddResourceLink(LinkedResourceType.Prev, prevRoute);

            }

            if (response.CurrentPage < response.TotalPages)
            {
                var nextRoute = Url.RouteUrl(nameof(GetProduct), new { limit = queryParameters.Limit, page = queryParameters.Page + 1 });

                response.AddResourceLink(LinkedResourceType.Next, nextRoute);
            }

            return response;
        }



        [Authorize(Roles = "loja")]
        [HttpPost]
        public async Task<IActionResult> AddProduct([FromForm] ProductDto model)
        {
            try
            {
                model.ImageUrl = GenerateFileName(model.ImageFile);
                var product = await _productService.AddAsync(model);

                if (product == null) return NoContent();
                UploadedFile(model.ImageFile, model.ImageUrl);

                return Ok(product);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar adicionar produto. Erro: {ex.Message}");
            }
        }

        [Authorize(Roles = "loja")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromForm] ProductDto model)
        {
            try
            {
                if (!string.IsNullOrEmpty(GenerateFileName(model.ImageFile)))
                {
                    model.ImageUrl = GenerateFileName(model.ImageFile);
                }

                var product = await _productService.UpdateAsync(id, model);

                if (product == null) return NoContent();
                UploadedFile(model.ImageFile, model.ImageUrl);

                return Ok(product);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar atualizar produto. Erro: {ex.Message}");
            }
        }



        [Authorize(Roles = "loja")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                if (await _productService.DeleteAsync(id))
                {
                    return Ok();
                }
                else
                {
                    throw new Exception("Ocorreu um problem não específico ao tentar deletar produto.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar deletar produto. Erro: {ex.Message}");
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
