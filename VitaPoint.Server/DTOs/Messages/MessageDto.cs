/*
    This DTO is used when retrieving a list of messages for the user.
    The each message object retrieved in the list will be converted to this DTO.
 */

namespace VitaPoint.Server.DTOs.Messages
{
    public class MessageDto
    {
        public int Id { get; set; }
        public string? SenderId { get; set; }
        public string? SenderName { get; set; }
        public string? ReceiverId { get; set; }
        public string? ReceiverName { get; set; }
        public string? Subject { get; set; }
        public string? Content { get; set; }
        public DateTime TimeSent { get; set; }
        public bool IsReply { get; set; }
    }
}
