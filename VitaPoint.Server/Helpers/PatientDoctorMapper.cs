using VitaPoint.Server.DTOs.Messages;
using VitaPoint.Server.Models;

namespace VitaPoint.Server.Helpers
{
    public static class PatientDoctorMapper
    {
        public static RecipientDto ToDoctorReceipientDto(this PatientDoctor pd)
        {
            return new RecipientDto()
            {
                SysId = pd.DoctorUserId,
                FirstName = pd.Doctor.FirstName,
                LastName = pd.Doctor.LastName,
                Title = pd.Doctor.Title
            };
        }
    }
}
