using Commerce.Domain;
using System.Threading.Tasks;
using Commerce.Application.Dtos;

namespace Commerce.Domain.Contracts
{
    public interface IUserService
    {
        Task<UserDto> Login(UserLoginDto model);
        Task<UserDto> Register(UserRegisterDto model);
        Task<string> GenerateJWToken(UserDto model);
        Task<UserDto> DecodeJWToken(string tokenString);
    }
}
