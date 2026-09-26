using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace VitaPoint.Server.Models
{
    [Table("Lab")]
    public class Lab
    {
        public int Id { get; set; }
        public string? LabName { get; set; }
    }
}
