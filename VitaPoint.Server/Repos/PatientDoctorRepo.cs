using Microsoft.EntityFrameworkCore;
using VitaPoint.Server.Data;
using VitaPoint.Server.Interfaces;
using VitaPoint.Server.Migrations;
using VitaPoint.Server.Models;

namespace VitaPoint.Server.Repos
{
    public class PatientDoctorRepo : IPatientDoctorRepo
    {
        private readonly ApplicationDbContext _context;

        public PatientDoctorRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        //May not need this. Consider deleting
        //public async Task<PatientDoctor?> GetPatientDoctorData(string firstId, string secondId)
        //{
        //    return await _context.PatientDoctor
        //        .Include(pd => pd.Patient)
        //        .Include(pd => pd.Doctor)
        //        .FirstOrDefaultAsync(pd =>
        //        (pd.PatientUserId == firstId && pd.DoctorUserId == secondId) ||
        //        (pd.PatientUserId == secondId && pd.DoctorUserId == firstId));
        //}

        public async Task<List<PatientDoctor>> GetPDByUserId(string userId)
        {
            return await _context.PatientDoctor
                .Where(pd => pd.PatientUserId == userId || pd.DoctorUserId == userId)
                .Include(pd => pd.Doctor)
                .Include(pd => pd.Patient)
                .ToListAsync();
        }
    }
}
