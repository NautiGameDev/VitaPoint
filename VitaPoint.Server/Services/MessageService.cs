using VitaPoint.Server.DTOs.Messages;
using VitaPoint.Server.Interfaces;
using VitaPoint.Server.Models;

namespace VitaPoint.Server.Services
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepo _messageRepo;

        public MessageService(IMessageRepo messageRepo)
        {
            _messageRepo = messageRepo;
        }

        public async Task<List<Message>> GetRepliesByRootId(string userId, int id)
        {
            return await _messageRepo.GetRepliesByRootId(userId, id);
        }

        public async Task<List<Message>> GetRootMessagesForUser(string userId)
        {
            return await _messageRepo.GetRootMessagesForUser(userId);
        }

        public async Task<Message> NewMessage(NewMessageDto dto)
        {
            Message message = new Message()
            {
                SenderId = dto.SenderId,
                ReceiverId = dto.ReceiverId,
                Subject = dto.Subject,
                Content = dto.Content,
                IsReply = dto.IsReply,
                RootId = dto.RootId
            };

            return await _messageRepo.PostMessage(message);
        }
    }
}
