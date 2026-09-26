using Microsoft.EntityFrameworkCore;
using VitaPoint.Server.Data;
using VitaPoint.Server.Interfaces;
using VitaPoint.Server.Models;

namespace VitaPoint.Server.Repos
{
    public class LabResultRepo : ILabResultRepo
    {
        private readonly ApplicationDbContext _context;

        public LabResultRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<LabResult?> GetLabResultById(int id, string userId)
        {
            return await _context.LabResults
                .AsNoTracking()
                .Include(lr => lr.Patient)
                .Include(lr => lr.OrderingDoctor)
                .Include(lr => lr.Lab)
                .Include(lr => lr.Components)
                .FirstOrDefaultAsync(lr => lr.Id == id && lr.Patient.UserId == userId);
        }

        public async Task<List<LabResult>> GetLabResultsByUser(string userId)
        {
            return await _context.LabResults
                .AsNoTracking()
                .Include(lr => lr.Patient)
                .Include(lr => lr.OrderingDoctor)
                .Include(lr => lr.Lab)
                .Where(lr => lr.Patient.UserId == userId)
                .ToListAsync();
        }
    }
}
