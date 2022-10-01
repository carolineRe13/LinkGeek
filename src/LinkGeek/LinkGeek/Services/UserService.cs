using LinkGeek.Data;
using LinkGeek.Models;
using Microsoft.EntityFrameworkCore;

namespace LinkGeek.Services;

public enum AddGameToUserResponse
{
    Success,
    GameNotFound,
    UserNotFound,
    GameAlreadyAdded
}

public class UserService
{
    public async Task<AddGameToUserResponse?> AddGameToUser(string userId, string gameId)
    {
        await using (var context = new ApplicationDbContext())
        {
            var game = await context.Game.FindAsync(gameId);
            if (game == null)
            {
                return AddGameToUserResponse.GameNotFound;
            }

            var user = await context.Users.Include(u => u.Games)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                return AddGameToUserResponse.UserNotFound;
            }

            user.Games ??= new List<Game>();
            if (user.Games.Contains(game))
            {
                return AddGameToUserResponse.GameAlreadyAdded;
            }
            user.Games.Add(game);

            await context.SaveChangesAsync();
            return AddGameToUserResponse.Success;
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