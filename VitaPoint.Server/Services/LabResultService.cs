using VitaPoint.Server.Interfaces;
using VitaPoint.Server.Models;

namespace VitaPoint.Server.Services
{
    public class LabResultService : ILabResultService
    {
        private readonly ILabResultRepo _labResultRepo;

        public LabResultService(ILabResultRepo labResultRepo)
        {
            _labResultRepo = labResultRepo;
        }

        public async Task<LabResult?> GetLabResultById(int id, string userId)
        {
            return await _labResultRepo.GetLabResultById(id, userId);
        }

        public async Task<List<LabResult>> GetLabResultsByUser(string userId)
        {
            return await _labResultRepo.GetLabResultsByUser(userId);
        }
    }
}
