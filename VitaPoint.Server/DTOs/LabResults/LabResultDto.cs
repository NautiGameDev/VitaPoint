using VitaPoint.Server.Models;

namespace VitaPoint.Server.DTOs.LabResults
{
    public class LabResultDto
    {
        public string? TestName { get; set; }
        public string? OrderingDoctor { get; set; }
        public string? LabName { get; set; }
        public DateTime CollectedAt { get; set; }
        public DateTime? ResultAt { get; set; }
        public string? Status { get; set; }
        public string? Notes { get; set; }
        public List<LabComponent> Components { get; set; } = new List<LabComponent>();
    }
}
