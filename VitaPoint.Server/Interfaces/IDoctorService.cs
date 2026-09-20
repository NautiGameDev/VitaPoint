using VitaPoint.Server.Models;

namespace VitaPoint.Server.Interfaces
{
    public interface IDoctorService
    {
        public Task<Doctor?> GetDoctorByUserId(string id);
    }
}
