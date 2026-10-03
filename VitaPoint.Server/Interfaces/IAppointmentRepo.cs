using VitaPoint.Server.DTOs.Appointment;
using VitaPoint.Server.Migrations;
using VitaPoint.Server.Models;

namespace VitaPoint.Server.Interfaces
{
    public interface IAppointmentRepo
    {
        public Task<List<Appointment>> GetAppointmentsForUser(string userId);
        public Task<List<Appointment>> GetCurrentAppointments(string doctorId, DateOnly date);
        public Task<bool> DoesAppointmentExist(string doctorId, DateOnly date, TimeOnly time);
        public Task<Appointment?> NewAppointment(Appointment appointment);
        public Task<Appointment?> CancelAppointment(int id, string userId);
    }
}
