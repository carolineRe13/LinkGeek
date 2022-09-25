using LinkGeek.Data;
using LinkGeek.Models;
using Microsoft.EntityFrameworkCore;

namespace LinkGeek.Services;

public class UserService
{
    public async Task<Game?> AddGameToUser(string userId, string gameId)
    {
        await using (var context = new ApplicationDbContext())
        {
            var game = await context.Game.FindAsync(gameId);
            if (game == null)
            {
                return null;
            }

            var user = await context.Users.Include(u => u.Games)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                return null;
            }

            user.Games ??= new List<Game>();
            user.Games.Add(game);

            await context.SaveChangesAsync();
            return game;
        }
    }

    public async Task<string?> UpdateLocation(string userId, string location)
    {
        await using (var context = new ApplicationDbContext())
        {
            var user = await context.Users.FindAsync(userId);
            if (user == null)
            {
                return null;
            }

            user.Location = location;
            await context.SaveChangesAsync();
            return user.Location;
        }
    }

    public async Task<string?> UpdateStatus(string userId, string status)
    {
        // TODO: Add profanity filter
        await using (var context = new ApplicationDbContext())
        {
            var user = await context.Users.FindAsync(userId);
            if (user == null)
            {
                return null;
            }

            user.Status = status;
            await context.SaveChangesAsync();
            return user.Status;
        }
    }
}