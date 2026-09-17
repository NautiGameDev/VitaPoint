using Microsoft.AspNetCore.Identity;

namespace VitaPoint.Server.Models
{
    public class Account : IdentityUser
    {
        public string? RefreshToken { get; set; } = "";
        public DateTime RefreshExpiration { get; set; }
    }
}
