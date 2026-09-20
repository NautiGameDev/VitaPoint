namespace VitaPoint.Server.DTOs.Messages
{
    public class NewMessageDto
    {
        public string? SenderId { get; set; }
        public string? ReceiverId { get; set; }
        public string? Subject { get; set; }
        public string? Content { get; set; }
        public DateTime TimeSent { get; set; } = DateTime.Now;
        public bool IsReply { get; set; } = false;
        public int? RootId { get; set; }
    }
}
