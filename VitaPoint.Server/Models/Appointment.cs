using System.ComponentModel.DataAnnotations.Schema;

namespace VitaPoint.Server.Models
{
    [Table("Appointments")]
    public class Appointment
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public virtual Patient? Patient { get; set; }
        public int DoctorId { get; set; }
        public virtual Doctor? Doctor { get; set; }
        public DateOnly Date { get; set; }
        public TimeOnly TimeSlot { get; set; }
        public AppointmentCategory Category { get; set; }
        public string Notes { get; set; } = string.Empty;

        public static AppointmentCategory CategoryStringToEnum(string category)
        {
            if (string.IsNullOrWhiteSpace(category))
                return AppointmentCategory.Other;

            category = category.Replace(" ", "").ToLower();

            return category.ToLower() switch
            {
                "generalcheckup" => AppointmentCategory.GeneralCheckup,
                "sickvisit" => AppointmentCategory.SickVisit,
                "followup" => AppointmentCategory.FollowUp,
                "labwork" => AppointmentCategory.LabWork,
                "vaccination" => AppointmentCategory.Vaccination,
                "telehealth" => AppointmentCategory.Telehealth,
                "newpatient" => AppointmentCategory.NewPatient,
                "other" => AppointmentCategory.Other
            };
        }
    }

    public enum AppointmentCategory
    {
        GeneralCheckup,
        SickVisit,
        FollowUp,
        LabWork,
        Vaccination,
        Telehealth,
        Other
    }
}
