using System.ComponentModel.DataAnnotations.Schema;

namespace VitaPoint.Server.Models
{
    [Table("Doctors")]
    public class Doctor
    {
        public int Id { get; set; }
        public string? AccountId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Title { get; set; }

        public string GetDoctorName()
        {
            return $"{Title} {FirstName} {LastName}";
        }
    }
}
