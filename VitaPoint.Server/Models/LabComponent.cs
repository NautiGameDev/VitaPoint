using System.ComponentModel.DataAnnotations.Schema;

/*
    Labcomponents are line items tested for in each lab result.
    The encompass a variety of targeted markers within a single lab test
    In the database, this model is a 1 to many relationship attached to a single lab result
 */

namespace VitaPoint.Server.Models
{
    [Table("LabComponents")]
    public class LabComponent
    {
        public int Id { get; set; }
        public int LabResultId { get; set; }
        public string? MarkerName { get; set; }
        public string? Value { get; set; }
        public string? Unit { get; set; }
        public MarkerFlags Flag { get; set; }
    }

    public enum MarkerFlags { Normal, High, Low }
}
