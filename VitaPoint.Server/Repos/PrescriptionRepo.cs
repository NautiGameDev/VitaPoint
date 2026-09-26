using Microsoft.EntityFrameworkCore;
using VitaPoint.Server.Data;
using VitaPoint.Server.Interfaces;
using VitaPoint.Server.Models;

namespace VitaPoint.Server.Repos
{
    public class PrescriptionRepo : IPrescriptionRepo
    {
        private readonly ApplicationDbContext _context;

        public PrescriptionRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Prescription?> GetPrescriptionById(int id, string userId)
        {
            return await _context.Prescriptions
                .AsNoTracking()
                .Include(p => p.Patient)
                .Include(p => p.OrderingDoctor)
                .FirstOrDefaultAsync(p => p.Id == id && p.Patient.UserId == userId);
        }

        public async Task<List<Prescription>> GetPrescriptionsByUser(string userId)
        {
            return await _context.Prescriptions
                .AsNoTracking()
                .Include(p => p.Patient)
                .Include(p => p.OrderingDoctor)
                .Where(p => p.Patient.UserId == userId)
                .ToListAsync();
        }

        public async Task<Prescription> UpdatePrescription(Prescription prescription)
        {
            await _context.SaveChangesAsync();

            return prescription;
        }
    }
}
