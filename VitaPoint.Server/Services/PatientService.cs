using VitaPoint.Server.Interfaces;
using VitaPoint.Server.Models;

namespace VitaPoint.Server.Services
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepo _patientRepo;

        public PatientService(IPatientRepo patientRepo) 
        { 
            _patientRepo = patientRepo;
        }

        
        //General getter method used to fetch patient data for user dashboard
        public async Task<Patient?> GetPatientByUser(string userId)
        {
            return await _patientRepo.GetPatientByUser(userId);
        }


        /*
            Method used during account creation to verify patient exists in the system
            Users without an existing patient profile or unverified credentials will not be able to create an account
         */
        public async Task<Patient?> VerifyPatient(string email, string activationCode, DateOnly DOB, string zip)
        {
            return await _patientRepo.VerifyPatient(email, activationCode, DOB, zip);
        }


        //Updates and activates the patient profile after account creation
        public async Task<Patient> ActivatePatient(Patient patient, string userId)
        {
            patient.UserId = userId;
            patient.IsActive = true;
            patient.LastUpdated = DateTime.UtcNow;
            patient.ActivationCode = null;

            return await _patientRepo.UpdatePatient(patient);
        }
    }
}
