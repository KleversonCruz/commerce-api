using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Commerce.Domain.Contracts;
using Commerce.Domain;
using Commerce.Domain.Identity;
using Commerce.Persistence.Contracts;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Commerce.Application.Dtos;

namespace Commerce.Application
{
    public class UserService : IUserService
    {
        private readonly ICommonPersist _commonPersist;
        private readonly IConfiguration _config;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public UserService(ICommonPersist commonPersist, IConfiguration config, UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<Role> roleManager, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _commonPersist = commonPersist;
            _config = config;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        public async Task<UserDto> Register(UserRegisterDto model)
        {
            try
            {
                var user = _mapper.Map<User>(model);
                var result = await _userManager.CreateAsync(user, model.Password);
                if (!result.Succeeded) throw new Exception(result.ToString());

                var role = await _roleManager.FindByIdAsync("3");
                await _userManager.AddToRoleAsync(user, role.Name);

                var appUser = _mapper.Map<UserDto>(user);
                return _mapper.Map<UserDto>(appUser);
            }
            catch (Exception ex)
            {
                throw new Exception($"Message: {ex.Message} InnerException: {ex.InnerException}");
            }
        }

        public async Task<UserDto> Login(UserLoginDto userLogin)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(userLogin.UserName);
                if (user == null) return null;
                var result = await _signInManager.CheckPasswordSignInAsync(user, userLogin.Password, false);
                if (!result.Succeeded) throw new Exception(result.ToString());

                var appUser = await _userManager.Users
                    .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                    .FirstOrDefaultAsync(u => u.NormalizedUserName == userLogin.UserName.ToUpper());

                return _mapper.Map<UserDto>(appUser);
            }
            catch (Exception ex)
            {
                throw new Exception($"Message: {ex.Message} InnerException: {ex.InnerException}");
            }
        }

        public async Task<string> GenerateJWToken(UserDto userDto)
        {
            var user = await _userManager.FindByNameAsync(userDto.UserName);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName)
            };

            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.ASCII
                .GetBytes(_config.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            var teste = tokenHandler.WriteToken(token);
            return teste;

        }

        public async Task<UserDto> DecodeJWToken(string tokenString)
        {
            try
            {
                var token = new JwtSecurityToken(jwtEncodedString: tokenString[7..]);
                if (token.ValidTo >= DateTime.Now)
                {
                    var userIdToken = token.Claims.First(i => i.Type == "nameid").Value;

                    var appUser = await _userManager.Users
                        .Include(u => u.UserRoles)
                        .ThenInclude(ur => ur.Role)
                        .FirstOrDefaultAsync(u => u.Id == Convert.ToInt32(userIdToken));
                    return _mapper.Map<UserDto>(appUser);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Message: {ex.Message} InnerException: {ex.InnerException}");
            }
        }
    }
}
