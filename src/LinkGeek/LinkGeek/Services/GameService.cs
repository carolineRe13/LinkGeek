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
        using (var context = contextProvider.GetContext())
        {
            return await context.Game.FindAsync(gameId);
        }
    }

    public async Task<ICollection<ApplicationUser>> GetGamePlayersAsync(string gameId, int playersToDisplay = 10)
    {
        using (var context = contextProvider.GetContext())
        {
            var game = await context.Game.Include(g => g.Players).FirstAsync(g => g.Id == gameId);
            return game.Players.Take(playersToDisplay).ToList();
        }
    }
}