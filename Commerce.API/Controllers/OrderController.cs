using Commerce.Application.Dtos;
using Commerce.Domain.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Commerce.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet("{shopId}")]
        [Authorize(Roles = "cliente")]
        public async Task<IActionResult> GetOrderById(int shopId)
        {
            try
            {
                var order = await _orderService.GetByCustomerAndShopAsync(shopId);
                if (order == null) return NoContent();

                return Ok(order);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar recuperar pedido. Erro: {ex.Message}");
            }
        }


        [Authorize(Roles = "cliente")]
        [HttpPost]
        public async Task<IActionResult> AddOrder(OrderDto model)
        {
            try
            {
                var order = await _orderService.AddAsync(model);
                if (order == null) return NoContent();

                return Ok(order);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar adicionar pedido. Erro: {ex.Message}");
            }
        }

        [Authorize(Roles = "cliente")]
        [HttpPut("{shopId}")]
        public async Task<IActionResult> UpdateOrder(int shopId, OrderDto model)
        {
            try
            {
                var order = await _orderService.UpdateAsync(shopId, model);
                if (order == null) return NoContent();

                return Ok(order);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar atualizar pedido. Erro: {ex.Message}");
            }
        }



        [Authorize(Roles = "cliente")]
        [HttpDelete("{shopId}")]
        public async Task<IActionResult> DeleteOrder(int shopId)
        {
            try
            {
                if (await _orderService.DeleteAsync(shopId))
                {
                    return Ok();
                }
                else
                {
                    throw new Exception("Ocorreu um problem não específico ao tentar deletar pedido.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar deletar pedido. Erro: {ex.Message}");
            }
        }
    }
}
