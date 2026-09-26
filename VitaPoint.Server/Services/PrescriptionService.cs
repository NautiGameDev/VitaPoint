using VitaPoint.Server.Interfaces;
using VitaPoint.Server.Models;

namespace VitaPoint.Server.Services
{
    public class PrescriptionService : IPrescriptionService
    {
        private readonly IPrescriptionRepo _prescriptionRepo;

        public PrescriptionService(IPrescriptionRepo prescriptionRepo)
        {
            _prescriptionRepo = prescriptionRepo;
        }

        public async Task<List<Prescription>> GetPrescriptionsByUser(string userId)
        {
            return await _prescriptionRepo.GetPrescriptionsByUser(userId);
        }

        public async Task<(bool, Prescription?)> RequestRefill(int id, string userId)
        {
            Prescription? prescription = await _prescriptionRepo.GetPrescriptionById(id, userId);

            if (prescription == null) return (false, null);

            if (prescription.RefillsRemaining == 0 || (prescription.RefillStatus != RefillStatuses.None && prescription.RefillStatus != RefillStatuses.Filled))
            {                
                return (false, prescription);
            }

            prescription.RefillStatus = RefillStatuses.Requested;
            prescription.LastRefillRequest = DateTime.UtcNow;

            return (true, await _prescriptionRepo.UpdatePrescription(prescription));
        }
    }
}
