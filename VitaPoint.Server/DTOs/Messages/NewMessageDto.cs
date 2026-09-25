namespace VitaPoint.Server.DTOs.Messages
{
    public class NewMessageDto
    {
        public string[] UserIds { get; set; }
        public string? Subject { get; set; }
        public string? Content { get; set; }
        public DateTime TimeSent { get; set; } = DateTime.Now;
        public bool IsReply { get; set; } = false;
        public int? RootId { get; set; }
    }
}
