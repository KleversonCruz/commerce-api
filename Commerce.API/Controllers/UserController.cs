using Commerce.Application.Dtos;
using Commerce.Domain.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System;
using System.Threading.Tasks;

namespace Commerce.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(UserRegisterDto model)
        {
            try
            {
                var user = await _userService.Register(model);
                if (user == null) return BadRequest();

                return Ok(new
                {
                    token = _userService.GenerateJWToken(user).Result,
                    user = user
                });
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro: {ex.Message}");
            }
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(UserLoginDto model)
        {
            try
            {
                var user = await _userService.Login(model);
                if (user == null) return BadRequest();

                return Ok(new
                {
                    token = _userService.GenerateJWToken(user).Result,
                    user = user
                });
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro: {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetUser()
        {
            var accessToken = Request.Headers[HeaderNames.Authorization];
            try
            {
                var user = await _userService.DecodeJWToken(accessToken);
                if (user == null) return BadRequest();

                return Ok(user);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro: {ex.Message}");
            }
        }

    }
}
