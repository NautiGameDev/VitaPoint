using VitaPoint.Server.DTOs.LabResults;
using VitaPoint.Server.Models;

namespace VitaPoint.Server.Helpers
{
    public static class LabResultMapper
    {
        public static LabResultMetaDto ToLabResultMetaDto(this LabResult labResult)
        {
            return new LabResultMetaDto()
            {
                TestName = labResult.TestName,
                OrderingDoctor = labResult.OrderingDoctor.GetDoctorName(),
                LabName = labResult.Lab.LabName,
                CollectedAt = labResult.CollectedAt,
                ResultAt = labResult.ResultAt,
                Status = labResult.Status.ToString()
            };
        }

        public static LabResultDto ToLabResultDto(this LabResult labResult)
        {
            return new LabResultDto()
            {
                TestName = labResult.TestName,
                OrderingDoctor = labResult.OrderingDoctor.GetDoctorName(),
                LabName = labResult.Lab.LabName,
                CollectedAt = labResult.CollectedAt,
                ResultAt = labResult.ResultAt,
                Status = labResult.Status.ToString(),
                Notes = labResult.Notes,
                Components = labResult.Components
            };
        }
    }
}
