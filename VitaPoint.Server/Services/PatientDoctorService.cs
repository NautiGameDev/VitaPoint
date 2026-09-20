using VitaPoint.Server.Interfaces;
using VitaPoint.Server.Models;

namespace VitaPoint.Server.Services
{
    public class PatientDoctorService : IPatientDoctorService
    {
        private readonly IPatientDoctorRepo _patientDoctorRepo;

        public PatientDoctorService(IPatientDoctorRepo patientDoctorRepo)
        {
            _patientDoctorRepo = patientDoctorRepo;
        }        

        //May not need this. Consider deleting
        //public async Task<PatientDoctor?> GetPatientDoctorData(string firstId, string secondId)
        //{
        //    return await _patientDoctorRepo.GetPatientDoctorData(firstId, secondId);
        //}

        public async Task<List<PatientDoctor>> GetPDByUserId(string userId)
        {
            return await _patientDoctorRepo.GetPDByUserId(userId);
        }
    }
}
