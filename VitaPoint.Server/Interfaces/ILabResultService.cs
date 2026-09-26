using VitaPoint.Server.Models;

namespace VitaPoint.Server.Interfaces
{
    public interface ILabResultService
    {
        public Task<List<LabResult>> GetLabResultsByUser(string userId);
        public Task<LabResult?> GetLabResultById(int id, string userId);
    }
}
