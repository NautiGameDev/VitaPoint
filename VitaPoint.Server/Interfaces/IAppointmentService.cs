using VitaPoint.Server.DTOs.Appointment;
using VitaPoint.Server.Models;

namespace VitaPoint.Server.Interfaces
{
    public interface IAppointmentService
    {
        public Task<List<Appointment>> GetAppointmentsForUser(string userId);
        public Task<List<Appointment>> GetCurrentAppointments(string doctorId, DateOnly date);
        public Task<bool> DoesAppointmentExist(string doctorId, DateOnly date, TimeOnly time);
        public Task<Appointment?> NewAppointment(NewAppointmentDto dto, Patient patientData, Doctor doctorData);
        public Task<Appointment?> CancelAppointment(int id, string userId);
    }
}
