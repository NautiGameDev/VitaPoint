using System.ComponentModel.DataAnnotations.Schema;

namespace VitaPoint.Server.Models
{
    [Table("LabResults")]
    public class LabResult
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public virtual Patient Patient { get; set; }
        public string? TestName { get; set; }
        public int OrderingDoctorId { get; set; }
        public virtual Doctor OrderingDoctor { get; set; }
        public int LabId { get; set; }
        public virtual Lab Lab { get; set; }
        public DateTime CollectedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ResultAt { get; set; } = null;
        public LabStatuses Status { get; set; } = LabStatuses.Pending;
        public string? Notes { get; set; } = "";

        public virtual List<LabComponent> Components { get; set; } = new List<LabComponent>();
    }

    public enum LabStatuses { Pending, Final, Corrected }
}
