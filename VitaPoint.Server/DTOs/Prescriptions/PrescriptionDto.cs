namespace VitaPoint.Server.DTOs.Prescriptions
{
    public class PrescriptionDto
    {
        public string? DoctorName { get; set; }
        public string? MedicineName { get; set; }
        public string? Dosage { get; set; }
        public string? Instructions { get; set; }
        public int RefillsRemaining { get; set; }
        public DateTime PrescribedAt { get; set; }
        public DateTime? LastRefilledAt { get; set; }
        public string? RefillStatus { get; set; }
        public DateTime? LastRefillRequest { get; set; }
    }
}
