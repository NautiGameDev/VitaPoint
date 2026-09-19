using VitaPoint.Server.Helpers;

namespace VitaPoint.Server.DTOs.Patient
{
    public class PatientDto
    {
        //Legal Information
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public string? DOB { get; set; }
        public string? Sex { get; set; }
        public string? Race { get; set; }
        public string? Ethnicity { get; set; }
        public string? MaritalStatus { get; set; }


        //Preferences
        public string? PreferredName { get; set; }
        public string? PreferredLanguage { get; set; }
        public string? Pronouns { get; set; }


        //Contact Information
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? PreferredContactMethod { get; set; }

        //Address
        public string? Address { get; set; }
        public string? Address2 { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Zip { get; set; }

        //Update Info
        public string? LastUpdated { get; set; }


        public void TransferData(Models.Patient patient)
        {
            FirstName = patient.FirstName;
            MiddleName = patient.MiddleName;
            LastName = patient.LastName;
            DOB = patient.DOB.ToString();
            Sex = patient.Sex.ToFormattedString();
            Race = patient.Race.ToFormattedString();
            Ethnicity = patient.Ethnicity.ToFormattedString();
            MaritalStatus = patient.MaritalStatus.ToFormattedString();
            PreferredName = patient.PreferredName;
            PreferredLanguage = patient.PreferredLanguage;
            Pronouns = patient.Pronouns.ToFormattedString();
            Email = patient.Email;
            PhoneNumber = patient.PhoneNumber;
            PreferredContactMethod = patient.PreferredContactMethod.ToFormattedString();
            Address = patient.Address;
            Address2 = patient.Address2;
            City = patient.City;
            State = patient.State;
            Zip = patient.Zip;
            LastUpdated = patient.LastUpdated.ToString();
        }
    }
}
