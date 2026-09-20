using Microsoft.EntityFrameworkCore;
using VitaPoint.Server.Data;
using VitaPoint.Server.Interfaces;
using VitaPoint.Server.Models;

namespace VitaPoint.Server.Repos
{
    public class DoctorRepo : IDoctorRepo
    {
        private readonly ApplicationDbContext _context;

        public DoctorRepo(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<Doctor?> GetDoctorByUserId(string accountId)
        {
            return await _context.Doctors.FirstOrDefaultAsync(d => d.AccountId == accountId);
        }
    }
}
