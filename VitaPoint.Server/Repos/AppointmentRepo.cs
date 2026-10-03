using Microsoft.EntityFrameworkCore;
using VitaPoint.Server.Data;
using VitaPoint.Server.Interfaces;
using VitaPoint.Server.Models;

namespace VitaPoint.Server.Repos
{
    public class AppointmentRepo : IAppointmentRepo
    {
        private readonly ApplicationDbContext _context;

        public AppointmentRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Appointment?> CancelAppointment(int id, string userId)
        {
            var appointment = await _context.Appointments
                .Include(a => a.Patient)
                .FirstOrDefaultAsync(a => a.Id == id && a.Patient.UserId == userId);


            if (appointment == null) return null;

            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();

            return appointment;
        }

        public async Task<bool> DoesAppointmentExist(string doctorId, DateOnly date, TimeOnly time)
        {
            return await _context.Appointments.AnyAsync(a => a.Doctor.AccountId == doctorId && a.Date == date && a.TimeSlot == time);
        }

        public async Task<List<Appointment>> GetAppointmentsForUser(string userId)
        {
            return await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Where(a => a.Patient.UserId == userId)
                .ToListAsync();
        }

        public async Task<List<Appointment>> GetCurrentAppointments(string doctorId, DateOnly date)
        {
            return await _context.Appointments
                .Include(a => a.Doctor)
                .Where(a => a.Doctor.AccountId == doctorId && a.Date == date)
                .ToListAsync();
        }

        public async Task<Appointment?> NewAppointment(Appointment appointment)
        {
            try
            {
                await _context.Appointments.AddAsync(appointment);
                await _context.SaveChangesAsync();
                return appointment;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
            
        }
    }
}
