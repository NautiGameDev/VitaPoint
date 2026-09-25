using VitaPoint.Server.DTOs.Messages;
using VitaPoint.Server.Models;

namespace VitaPoint.Server.Interfaces
{
    public interface IMessageService
    {
        public Task<Message> NewMessage(NewMessageDto dto, string senderId, string receiverId);
        public Task<List<Message>> GetRootMessagesForUser(string userId);
        public Task<List<Message>> GetRepliesByRootId(string userId, int id);
    }
}
