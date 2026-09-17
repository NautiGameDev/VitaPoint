using VitaPoint.Server.Models;

namespace VitaPoint.Server.Interfaces
{
    public interface ITokenService
    {
        public Task<string> CreateToken(Account account);
        public string CreateRefreshToken();
    }
}
