namespace VitaPoint.Server.DTOs.Appointment
{
    public class UnavailableRequestDto
    {
        public DateOnly Date { get; set; }
        public string? DoctorId { get; set; }
    }
}
