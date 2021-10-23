using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Commerce.Domain.Contracts;
using Commerce.Application.Dtos;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Commerce.Domain.Links;
using Commerce.Application;
using System.Threading;
using System.IO;

namespace Commerce.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CategoryController(ICategoryService categoryService, IWebHostEnvironment hostEnvironment)
        {
            _categoryService = categoryService;
            _webHostEnvironment = hostEnvironment;
        }
        public record UrlQueryParameters
        {
            public int ShopId { get; init; }
            public string Name { get; init; }
            public bool? IsActive { get; init; } = null;
            public int Limit { get; init; } = 20;
            public int Page { get; init; } = 1;
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            try
            {
                var category = await _categoryService.GetByIdAsync(id);
                if (category == null) return NoContent();

                return Ok(category);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar recuperar categorias. Erro: {ex.Message}");
            }
        }

        [HttpGet(Name = nameof(GetCategory))]
        [AllowAnonymous]
        public async Task<IActionResult> GetCategory([FromQuery] UrlQueryParameters urlQueryParameters, CancellationToken cancellationToken)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                var categories = await _categoryService.GetAsync(
                    urlQueryParameters.ShopId,
                    urlQueryParameters.Name,
                    urlQueryParameters.IsActive,
                    urlQueryParameters.Limit,
                    urlQueryParameters.Page,
                    cancellationToken
                );

                return Ok(GeneratePageLinks(urlQueryParameters, categories));
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
                var prevRoute = Url.RouteUrl(nameof(GetCategory), new { limit = queryParameters.Limit, page = queryParameters.Page - 1 });

                response.AddResourceLink(LinkedResourceType.Prev, prevRoute);

            }

            if (response.CurrentPage < response.TotalPages)
            {
                var nextRoute = Url.RouteUrl(nameof(GetCategory), new { limit = queryParameters.Limit, page = queryParameters.Page + 1 });

                response.AddResourceLink(LinkedResourceType.Next, nextRoute);
            }

            return response;
        }

        [Authorize(Roles = "loja")]
        [HttpPost]
        public async Task<IActionResult> AddCategory([FromForm] CategoryDto model)
        {
            try
            {
                model.ImageUrl = GenerateFileName(model.ImageFile);
                var category = await _categoryService.AddAsync(model);
                if (category == null) return NoContent();

                UploadedFile(model.ImageFile, model.ImageUrl);

                return Ok(category);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar adicionar categoria. Erro: {ex.Message}");
            }
        }

        [Authorize(Roles = "loja,admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromForm] CategoryDto model)
        {
            try
            {
                if (!string.IsNullOrEmpty(GenerateFileName(model.ImageFile)))
                {
                    model.ImageUrl = GenerateFileName(model.ImageFile);
                }
                
                var categoria = await _categoryService.UpdateAsync(id, model);

                if (categoria == null) return NoContent();
                UploadedFile(model.ImageFile, model.ImageUrl);

                return Ok(categoria);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar atualizar categoria. Erro: {ex.Message}");
            }
        }
       
        [Authorize(Roles = "loja,admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                if (await _categoryService.DeleteAsync(id))
                {
                    return Ok();
                }
                else
                {
                    throw new Exception("Ocorreu um problem não específico ao tentar deletar Categoria.");
                }
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar deletar categorias. Erro: {ex.Message}");
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
