namespace VitaPoint.Server.DTOs.LabResults
{
    /*
        This DTO is used to populate the lab results page.
        Each of the user's lab result is converted to meta data to populate the list page on the client side
     */
    public class LabResultMetaDto
    {
        public string? TestName { get; set; }
        public string? OrderingDoctor { get; set; }
        public string? LabName { get; set; }
        public DateTime CollectedAt { get; set; }
        public DateTime? ResultAt { get; set; }
        public string? Status { get; set; }
    }
}
