using LinkGeek.AppIdentity;
using LinkGeek.Data;
using LinkGeek.Models;
using Microsoft.EntityFrameworkCore;

namespace LinkGeek.Services;

public class GameService
{
    private readonly IContextProvider contextProvider;

    public GameService(IContextProvider contextProvider)
    {
        this.contextProvider = contextProvider;
    }

    public async Task<Game?> GetGameAsync(string gameId)
    {
        await using var context = contextProvider.GetContext();
        return await context.Game.FindAsync(gameId);
    }

    public async Task<ICollection<ApplicationUser>> GetGamePlayersAsync(string gameId, int playersToDisplay = 10)
    {
        await using var context = contextProvider.GetContext();
        var game = await context.Game.Include(g => g.Players).FirstAsync(g => g.Id == gameId);
        return game.Players.Take(playersToDisplay).ToList();
    }

    public async Task<List<Post>> GetGameFeedAsync(string gameId)
    {
        await using var context = contextProvider.GetContext();
        var result = await context.Posts
            .Include(p => p.ApplicationUser)
            .Include(p => p.Comments)
            .Include(p => p.Likes)
            .Include(p => p.Game)
            .Where(p => (p.Game != null ? p.Game.Id : null) == gameId)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
        return result;
    }
}