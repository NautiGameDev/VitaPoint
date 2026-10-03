using VitaPoint.Server.DTOs.Appointment;
using VitaPoint.Server.Interfaces;
using VitaPoint.Server.Models;

namespace VitaPoint.Server.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepo _appointmentRepo;

        public AppointmentService(IAppointmentRepo appointmentRepo)
        {
            _appointmentRepo = appointmentRepo;
        }

        public async Task<Appointment?> CancelAppointment(int id, string userId)
        {
            return await _appointmentRepo.CancelAppointment(id, userId);
        }

        public async Task<bool> DoesAppointmentExist(string doctorId, DateOnly date, TimeOnly time)
        {
            return await _appointmentRepo.DoesAppointmentExist(doctorId, date, time);
        }

        public async Task<List<Appointment>> GetAppointmentsForUser(string userId)
        {
            return await _appointmentRepo.GetAppointmentsForUser(userId);
        }

        public async Task<List<Appointment>> GetCurrentAppointments(string doctorId, DateOnly date)
        {
            return await _appointmentRepo.GetCurrentAppointments(doctorId, date);
        }

        public Task<Appointment?> NewAppointment(NewAppointmentDto dto, Patient patientData, Doctor doctorData)
        {
            Appointment newAppointment = new Appointment()
            {
                PatientId = patientData.Id,
                DoctorId = doctorData.Id,
                Date = dto.Date,
                TimeSlot = dto.TimeSlot,
                Category = Appointment.CategoryStringToEnum(dto.Category ?? string.Empty),
                Notes = dto.Notes ?? string.Empty
            };

            return _appointmentRepo.NewAppointment(newAppointment);
        }
    }
}
