using System.ComponentModel.DataAnnotations.Schema;
using VitaPoint.Server.DTOs.Patient;

namespace VitaPoint.Server.Models
{
    [Table("Patients")]
    public class Patient
    {
        public int Id { get; set; }
        public string? UserId { get; set; }

        //Legal Information
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public string? SSNLastFour { get; set; }
        public DateOnly? DOB { get; set; }
        public SexChoices? Sex { get; set; }
        public RaceChoices? Race { get; set; }
        public EthnicityChoices? Ethnicity { get; set; }
        public MaritalStatuses? MaritalStatus { get; set; }


        //Preferences
        public string? PreferredName { get; set; }
        public string? PreferredLanguage { get; set; }
        public PronounChoices? Pronouns { get; set; }


        //Contact Information
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public PreferredContactMethods? PreferredContactMethod { get; set; }

        //Address
        public string? Address { get; set; }
        public string? Address2 { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Zip { get; set; }

        //Profile Information
        public string? ActivationCode { get; set; }
        public bool IsActive { get; set; } = false;
        public DateTime? LastUpdated { get; set; }

        public void UpdatePatientData(UpdatePatientDto dto)
        {
            UpdateMaritalStatus(dto.MaritalStatus);
            UpdatePronouns(dto.Pronouns);
            UpdateContactMethod(dto.PreferredContactMethod);

            PreferredName = dto.PreferredName;
            PreferredLanguage = dto.PreferredLanguage;
            PhoneNumber = dto.PhoneNumber;
            Address = dto.Address;
            Address2 = dto.Address2;
            City = dto.City;
            State = dto.State;
            Zip = dto.Zip;
            LastUpdated = DateTime.UtcNow;
        }

        private void UpdateMaritalStatus(string maritalStatus)
        {
            switch (maritalStatus)
            {
                case "Single":
                    MaritalStatus = MaritalStatuses.Single;
                    break;
                case "Married":
                    MaritalStatus = MaritalStatuses.Married;
                    break;
                case "Separated":
                    MaritalStatus = MaritalStatuses.Separated;
                    break;
                case "Divorced":
                    MaritalStatus = MaritalStatuses.Divorced;
                    break;
                case "Widowed":
                    MaritalStatus = MaritalStatuses.Widowed;
                    break;
                case "Prefer Not To Say":
                    MaritalStatus = MaritalStatuses.PreferNotToSay;
                    break;
            }
        }
        private void UpdatePronouns(string pronoun)
        {
            switch (pronoun)
            {
                case "He Him":
                    Pronouns = PronounChoices.HeHim;
                    break;
                case "She Her":
                    Pronouns = PronounChoices.SheHer;
                    break;
                case "They Them":
                    Pronouns = PronounChoices.TheyThem;
                    break;
                case "Other":
                    Pronouns = PronounChoices.Other;
                    break;
                case "Prefer Not To Say":
                    Pronouns = PronounChoices.PreferNotToSay;
                    break;
            }
        }

        private void UpdateContactMethod(string method)
        {
            switch (method)
            {
                case "Call":
                    PreferredContactMethod = PreferredContactMethods.Call;
                    break;
                case "Text":
                    PreferredContactMethod = PreferredContactMethods.Text;
                    break;
                case "Email":
                    PreferredContactMethod = PreferredContactMethods.Email;
                    break;
            }
        }

        public string GetPatientName()
        {
            return $"{FirstName} {LastName}";
        }
    }

    public enum SexChoices { Male, Female, Other, Unknown, PreferNotToSay }
    public enum PronounChoices { HeHim, SheHer, TheyThem, Other, PreferNotToSay }
    public enum PreferredContactMethods { Call, Text, Email }
    public enum MaritalStatuses { Single, Married, Separated, Divorced, Widowed, PreferNotToSay }
    public enum EthnicityChoices { HispanicOrLatino, NotHispanicOrLatino, PreferNotToSay, Unknown }
    public enum RaceChoices { AmericanIndianOrAlaskaNative, Asian, BlackOrAfricanAmerican, NativeHawaiianOrPacificIslander, White, Other, PreferNotToSay, Unknown }
          
}
