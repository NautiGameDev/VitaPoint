using VitaPoint.Server.Models;

namespace VitaPoint.Server.DTOs.Appointment
{
    public class NewAppointmentDto
    {
        public string? DoctorId { get; set; }
        public DateOnly Date { get; set; }
        public TimeOnly TimeSlot { get; set; }
        public string? Category { get; set; }
        public string? Notes { get; set; }
    }
}
