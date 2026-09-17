using VitaPoint.Server.Models;

namespace VitaPoint.Server.Interfaces
{
    public interface IPatientRepo
    {
        //Used to fetch patient data for user's profile
        public Task<Patient?> GetPatientByUser(string userId);

        //Used during account registration to very patient exists
        public Task<Patient?> VerifyPatient(string email, string activationCode, DateOnly DOB, string zip);

        
        public Task<Patient> UpdatePatient(Patient patient);
    }
}
