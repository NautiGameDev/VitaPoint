using VitaPoint.Server.Models;

namespace VitaPoint.Server.DTOs.Appointment
{
    public class AppointmentDto
    {
        public string? DoctorName { get; set; }
        public DateOnly Date { get; set; }
        public TimeOnly TimeSlot { get; set; }
        public string? CategoryName { get; set; }
        public string? Notes { get; set; }
    }
}
