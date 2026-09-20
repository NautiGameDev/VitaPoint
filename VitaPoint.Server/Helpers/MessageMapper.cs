using VitaPoint.Server.DTOs.Messages;
using VitaPoint.Server.Models;

namespace VitaPoint.Server.Helpers
{
    public static class MessageMapper
    {
        public static MessageDto ToMessageDto(this Message message, string sender, string receiver)
        {
            return new MessageDto()
            {
                SenderId = message.SenderId,
                SenderName = sender,
                ReceiverId = message.ReceiverId,
                ReceiverName = receiver,
                Subject = message.Subject,
                Content = message.Content,
                TimeSent = message.TimeSent,
                IsReply = message.IsReply
            };
        }
    }
}
