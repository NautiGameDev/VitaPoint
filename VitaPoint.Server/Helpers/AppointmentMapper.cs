using VitaPoint.Server.DTOs.Appointment;
using VitaPoint.Server.Models;

namespace VitaPoint.Server.Helpers
{
    public static class AppointmentMapper
    {
        public static AppointmentDto ToAppointmentDto(this Appointment appointment)
        {
            return new AppointmentDto()
            {
                Id = appointment.Id,
                DoctorName = appointment.Doctor.GetDoctorName(),
                Date = appointment.Date,
                TimeSlot = appointment.TimeSlot,
                CategoryName = appointment.Category.ToFormattedString(),
                Notes = appointment.Notes
            };
        }

        public static UnavailableAppointmentsDto ToUnavailableAppointmentDto(this Appointment appointment)
        {
            return new UnavailableAppointmentsDto()
            {
                unavailableTime = appointment.TimeSlot
            };
        }
    }
}
