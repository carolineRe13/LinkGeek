using LinkGeek.AppIdentity;
using LinkGeek.Data;
using LinkGeek.Shared;
using Microsoft.EntityFrameworkCore;

namespace LinkGeek.Services;

public class ChatService
{
    private readonly IContextProvider contextProvider;

    public ChatService(IContextProvider contextProvider)
    {
        this.contextProvider = contextProvider;
    }

    public async Task<ApplicationUser> GetUserDetailsAsync(string userId)
    {
        await using var context = contextProvider.GetContext();
        return await context.Users.Where(user => user.Id == userId).FirstAsync();
    }

    public async Task<List<ChatMessage>> GetConversationAsync(string userId, string contactId, int count = int.MaxValue)
    {
        await using var context = contextProvider.GetContext();
        var messages = await context.ChatMessages
            .Where(h => (h.FromUserId == contactId && h.ToUserId == userId) || (h.FromUserId == userId && h.ToUserId == contactId))
            .OrderByDescending(a => a.CreatedDate) // needed to select the count most recent messages
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
            })
            .Take(count)
            .OrderBy(a => a.CreatedDate) // needed to re-order the messages in the correct order
            .ToListAsync();

        return messages;
    }

    public async Task<int> SaveMessageAsync(ChatMessage message, string userId)
    {
        await using (var context = contextProvider.GetContext())
        {
            message.FromUserId = userId;
            message.CreatedDate = DateTime.Now;
            await context.ChatMessages.AddAsync(message);
            return await context.SaveChangesAsync();
        }
    }
    
}