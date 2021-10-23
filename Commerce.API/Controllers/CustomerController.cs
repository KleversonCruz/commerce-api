using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Commerce.Domain.Contracts;
using Commerce.Application.Dtos;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Commerce.Application;
using Commerce.Domain.Links;
using System.Threading;

namespace Commerce.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CustomerController(ICustomerService customerService, IWebHostEnvironment hostEnvironment)
        {
            _customerService = customerService;
            _webHostEnvironment = hostEnvironment;
        }

        public record UrlQueryParameters
        {
            public int ShopId { get; init; }
            public string Name { get; init; }
            public int Limit { get; init; } = 20;
            public int Page { get; init; } = 1;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomerById(int id)
        {
            try
            {
                var cliente = await _customerService.GetByIdAsync(id);
                if (cliente == null) return NoContent();

                return Ok(cliente);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar recuperar clientes. Erro: {ex.Message}");
            }
        }

        [HttpGet(Name = nameof(GetCustomer))]
        [Authorize(Roles = "admin,loja")]
        public async Task<IActionResult> GetCustomer([FromQuery] UrlQueryParameters urlQueryParameters, CancellationToken cancellationToken)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                var customers = await _customerService.GetAsync(
                    urlQueryParameters.ShopId,
                    urlQueryParameters.Name,
                    urlQueryParameters.Limit,
                    urlQueryParameters.Page,
                    cancellationToken
                );

                return Ok(GeneratePageLinks(urlQueryParameters, customers));
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
                var prevRoute = Url.RouteUrl(nameof(GetCustomer), new { limit = queryParameters.Limit, page = queryParameters.Page - 1 });

                response.AddResourceLink(LinkedResourceType.Prev, prevRoute);

            }

            if (response.CurrentPage < response.TotalPages)
            {
                var nextRoute = Url.RouteUrl(nameof(GetCustomer), new { limit = queryParameters.Limit, page = queryParameters.Page + 1 });

                response.AddResourceLink(LinkedResourceType.Next, nextRoute);
            }

            return response;
        }

        

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> AddCustomer(CustomerDto model)
        {
            try
            {
                var cliente = await _customerService.AddAsync(model);
                if (cliente == null) return NoContent();

                return Ok(cliente);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar adicionar cliente. Erro: {ex.Message}");
            }
        }

        [Authorize(Roles = "cliente")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomer(int id, CustomerDto model)
        {
            try
            {
                var cliente = await _customerService.UpdateAsync(id, model);
                if (cliente == null) return NoContent();

                return Ok(cliente);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar atualizar cliente. Erro: {ex.Message}");
            }
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            try
            {
                if (await _customerService.DeleteAsync(id))
                {
                    return Ok();
                }
                else
                {
                    throw new Exception("Ocorreu um problem não específico ao tentar deletar Cliente.");
                }
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar deletar clientes. Erro: {ex.Message}");
            }
        }
    }
}
