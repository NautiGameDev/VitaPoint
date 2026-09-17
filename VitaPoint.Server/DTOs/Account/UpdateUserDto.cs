using System.ComponentModel.DataAnnotations;

namespace VitaPoint.Server.DTOs.Account
{
    public class UpdateUserDto
    {
        [Required]
        public string? Email { get; set; }

        [Required]
        public string? CurrentPassword { get; set; }

        [Required]
        public string? NewPassword { get; set; }
    }
}
