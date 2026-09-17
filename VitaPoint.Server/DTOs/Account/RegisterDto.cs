using System.ComponentModel.DataAnnotations;

namespace VitaPoint.Server.DTOs.Account
{
    public class RegisterDto
    {
        [Required]
        public string? Email { get; set; }

        [Required]
        public string? Password { get; set; }
        
        [Required]
        public string? ActivationCode { get; set; }
        
        [Required]
        public string? Zip { get; set; }
        
        [Required]
        public DateOnly DOB { get; set; }
    }
}
