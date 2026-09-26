using System.ComponentModel.DataAnnotations.Schema;

namespace VitaPoint.Server.Models
{
    [Table("Prescriptions")]
    public class Prescription
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public virtual Patient Patient { get; set; }
        public int OrderingDoctorId { get; set; }
        public virtual Doctor OrderingDoctor { get; set; }

        public string? MedicineName { get; set; }
        public string? Dosage { get; set; }
        public string? Instructions { get; set; }
        public int RefillsRemaining { get; set; } = 0;

        public DateTime PrescribedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastRefilledAt { get; set; }
        public RefillStatuses RefillStatus { get; set; } = RefillStatuses.None;
        public DateTime? LastRefillRequest { get; set; }            
    }

    public enum RefillStatuses { None, Requested, Approved, Denied, Filled }
}
