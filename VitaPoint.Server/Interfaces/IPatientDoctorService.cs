using VitaPoint.Server.Models;

namespace VitaPoint.Server.Interfaces
{
    public interface IPatientDoctorService
    {
        public Task<List<PatientDoctor>> GetPDByUserId(string userId);
        //public Task<PatientDoctor?> GetPatientDoctorData(string firstId, string secondId);
    }
}
