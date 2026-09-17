using System.ComponentModel.DataAnnotations.Schema;

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
    }

    public enum SexChoices { Male, Female, Other, Unknown, PreferNotToSay }
    public enum PronounChoices { HeHim, SheHer, TheyThem, Other, PreferNotToSay }
    public enum PreferredContactMethods { Call, Text, Email }
    public enum MaritalStatuses { Single, Married, Separated, Divorced, Widowed, PreferNotToSay }
    public enum EthnicityChoices { HispanicOrLatino, NotHispanicOrLatino, PreferNotToSay, Unknown }
    public enum RaceChoices { AmericanIndianOrAlaskaNative, Asian, BlackOrAfricanAmerican, NativeHawaiianOrPacificIslander, White, Other, PreferNotToSay, Unknown }
          
}
