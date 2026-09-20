using System.ComponentModel.DataAnnotations.Schema;

/*
 
    This model is a join table connecting doctors to their respective patients in a many-to-many relationship 
 */

namespace VitaPoint.Server.Models
{
    [Table("PatientDoctor")]
    public class PatientDoctor
    {
        public int Id { get; set; }
        public string? DoctorUserId { get; set; }
        public int DoctorId { get; set; }
        public virtual Doctor? Doctor { get; set; }
        public string? PatientUserId { get; set; }
        public int PatientId { get; set; }
        public virtual Patient? Patient { get; set; }
    }
}
