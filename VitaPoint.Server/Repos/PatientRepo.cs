using Microsoft.EntityFrameworkCore;
using VitaPoint.Server.Data;
using VitaPoint.Server.Interfaces;
using VitaPoint.Server.Models;

namespace VitaPoint.Server.Repos
{
    public class PatientRepo : IPatientRepo
    {
        private readonly ApplicationDbContext _context;

        public PatientRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Patient?> GetPatientByUser(string userId)
        {
            return await _context.Patients.FirstOrDefaultAsync(x => x.UserId == userId);
        }
                
        public async Task<Patient?> VerifyPatient(string email, string activationCode, DateOnly DOB, string zip)
        {
            return await _context.Patients.FirstOrDefaultAsync(x => x.Email == email && x.ActivationCode == activationCode && x.Zip == zip && x.DOB == DOB && x.IsActive == false);
        }

        public async Task<Patient> UpdatePatient(Patient patient)
        {
            await _context.SaveChangesAsync();
            return patient;
        }
    }
}
