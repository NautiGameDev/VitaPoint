using VitaPoint.Server.Models;

namespace VitaPoint.Server.Interfaces
{
    public interface IPatientService
    {
        //Used to fetch patient data for user's profile
        public Task<Patient?> GetPatientByUser(string userId);

        //Used during account registration to very patient exists
        public Task<Patient?> VerifyPatient(string email, string activationCode, DateOnly DOB, string zip);

        //Used to activate new patient account
        public Task<Patient> ActivatePatient(Patient patient, string userId);
    }
}
