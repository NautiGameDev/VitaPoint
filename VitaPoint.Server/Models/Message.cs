using System.ComponentModel.DataAnnotations.Schema;
using VitaPoint.Server.DTOs.Messages;

namespace VitaPoint.Server.Models
{
    [Table("Messages")]
    public class Message
    {
        public int Id { get; set; }
        public string? SenderId { get; set; }
        public string? ReceiverId { get; set; }
        public string? Subject { get; set; }
        public string? Content { get; set; }
        public DateTime TimeSent { get; set; } = DateTime.UtcNow;
        public bool IsReply { get; set; } = false;
        public int? RootId { get; set; }

        
    }
}
