using VitaPoint.Server.DTOs.Messages;
using VitaPoint.Server.Models;

namespace VitaPoint.Server.Interfaces
{
    public interface IMessageRepo
    {
        public Task<Message> PostMessage(Message message);
        public Task<List<Message>> GetRootMessagesForUser(string userId);
        public Task<List<Message>> GetRepliesByRootId(string userId, int id);
    }
}
