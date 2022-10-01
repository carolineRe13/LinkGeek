using LinkGeek.AppIdentity;
using LinkGeek.Data;
using LinkGeek.Shared;
using Microsoft.EntityFrameworkCore;

namespace LinkGeek.Services;

public class ChatService
{
    public async Task<ApplicationUser> GetUserDetailsAsync(string userId) {
        using (var context = new ApplicationDbContext())
        {
            return await context.Users.Where(user => user.Id == userId).FirstOrDefaultAsync();
        }
    }
    public async Task<List<ApplicationUser>> GetUsersAsync(string userId) {
        using (var context = new ApplicationDbContext())
        {
            return await context.Users.Where(user => user.Id != userId).ToListAsync();
        }
    }

    public async Task<List<ChatMessage>> GetConversationAsync(string userId, string contactId) {
        using (var context = new ApplicationDbContext())
        {
            var messages = await context.ChatMessages
                .Where(h => (h.FromUserId == contactId && h.ToUserId == userId) || (h.FromUserId == userId && h.ToUserId == contactId))
                .OrderBy(a => a.CreatedDate)
                .Include(a => a.FromUser)
                .Include(a => a.ToUser)
                .Select(x => new ChatMessage
                {
                    FromUserId = x.FromUserId,
                    Message = x.Message,
                    CreatedDate = x.CreatedDate,
                    Id = x.Id,
                    ToUserId = x.ToUserId,
                    ToUser = x.ToUser,
                    FromUser = x.FromUser
                }).ToListAsync();

            return messages;
        }
    }

    public async Task<int> SaveMessageAsync(ChatMessage message, string userId)
    {
        using (var context = new ApplicationDbContext())
        {
            message.FromUserId = userId;
            message.CreatedDate = DateTime.Now;
            message.ToUser = await context.Users.Where(user => user.Id == message.ToUserId).FirstOrDefaultAsync();
            await context.ChatMessages.AddAsync(message);
            return await context.SaveChangesAsync();
        }
    }
    
}