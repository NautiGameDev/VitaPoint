using VitaPoint.Server.Models;

namespace VitaPoint.Server.Interfaces
{
    public interface IPrescriptionRepo
    {
        public Task<List<Prescription>> GetPrescriptionsByUser(string userId);
        public Task<Prescription?> GetPrescriptionById(int id, string userId);
        public Task<Prescription> UpdatePrescription(Prescription prescription);
    }
}
