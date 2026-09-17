using VitaPoint.Server.DTOs.Account;
using VitaPoint.Server.Entities;
using VitaPoint.Server.Models;

namespace VitaPoint.Server.Interfaces
{
    public interface IAccountService
    {
        public Task<AuthResult> Login(LoginDto dto);

        public Task<(AuthResult result, string? userId)> Register(RegisterDto dto);

        public Task<AuthResult> UpdateCredentials(string userId, UpdateUserDto dto);

        public Task<AuthResult> RefreshToken(string refreshToken);
    }
}
