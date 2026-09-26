using VitaPoint.Server.DTOs.Prescriptions;
using VitaPoint.Server.Models;

namespace VitaPoint.Server.Helpers
{
    public static class PrescriptionMapper
    {
        public static PrescriptionDto ToPrescriptionDto(this Prescription prescription)
        {
            return new PrescriptionDto()
            {
                DoctorName = prescription.OrderingDoctor.GetDoctorName(),
                MedicineName = prescription.MedicineName,
                Dosage = prescription.Dosage,
                Instructions = prescription.Instructions,
                RefillsRemaining = prescription.RefillsRemaining,
                PrescribedAt = prescription.PrescribedAt,
                LastRefilledAt = prescription.LastRefilledAt,
                RefillStatus = prescription.RefillStatus.ToString(),
                LastRefillRequest = prescription.LastRefillRequest
            };
        }
    }
}
