using VitaPoint.Server.Models;

namespace VitaPoint.Server.Interfaces
{
    public interface IDoctorRepo
    {
        public Task<Doctor?> GetDoctorByUserId(string AccountId);
    }
}
