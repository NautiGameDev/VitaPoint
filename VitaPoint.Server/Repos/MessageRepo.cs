using Microsoft.EntityFrameworkCore;
using VitaPoint.Server.Data;
using VitaPoint.Server.Interfaces;
using VitaPoint.Server.Models;

namespace VitaPoint.Server.Repos
{
    public class MessageRepo : IMessageRepo
    {
        private readonly ApplicationDbContext _context;

        public MessageRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Message>> GetRepliesByRootId(string userId, int id)
        {
            return await _context.Messages
                .Where(m => (m.RootId == id || m.Id == id)
                && (m.SenderId == userId || m.ReceiverId == userId))
                .OrderBy(m => m.TimeSent)
                .ToListAsync();
        }

        public async Task<List<Message>> GetRootMessagesForUser(string userId)
        {
            return await _context.Messages
                .Where(m => !m.IsReply && (m.ReceiverId == userId || m.SenderId == userId))
                .OrderBy(m => m.TimeSent)
                .ToListAsync();
        }

        public async Task<Message> PostMessage(Message message)
        {
            await _context.Messages.AddAsync(message);
            await _context.SaveChangesAsync();

            return message;
        }
    }
}
