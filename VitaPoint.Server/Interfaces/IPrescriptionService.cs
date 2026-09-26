using VitaPoint.Server.Models;

namespace VitaPoint.Server.Interfaces
{
    public interface IPrescriptionService
    {
        public Task<List<Prescription>> GetPrescriptionsByUser(string userId);
        public Task<(bool , Prescription?)> RequestRefill(int id, string userId);
    }
}
