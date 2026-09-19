using VitaPoint.Server.Models;

namespace VitaPoint.Server.DTOs.Patient
{
    public class UpdatePatientDto
    {        
        public string? CurrentPassword { get; set; }
        public string? NewPassword { get; set; } = "";
        public string? MaritalStatus { get; set; }


        //Preferences
        public string? PreferredName { get; set; }
        public string? PreferredLanguage { get; set; }
        public string? Pronouns { get; set; }


        //Contact Information
        public string? PhoneNumber { get; set; }
        public string? PreferredContactMethod { get; set; }
        public string? Address { get; set; }
        public string? Address2 { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Zip { get; set; }
    }
}
