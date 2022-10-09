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

public enum RemoveGameFromUserResponse
{
    Success,
    GameNotFound,
    UserNotFound,
    GameAlreadyRemoved
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
    
    public async Task<RemoveGameFromUserResponse?> RemoveGameFromUser(string userId, string gameId)
    {
        await using (var context = new ApplicationDbContext())
        {
            var game = await context.Game.FindAsync(gameId);
            if (game == null)
            {
                return RemoveGameFromUserResponse.GameNotFound;
            }

            var user = await context.Users.Include(u => u.Games)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                return RemoveGameFromUserResponse.UserNotFound;
            }

            user.Games ??= new List<Game>();
            if (!user.Games.Contains(game))
            {
                return RemoveGameFromUserResponse.GameAlreadyRemoved;
            }

            user.Games.Remove(game);

            await context.SaveChangesAsync();
            return RemoveGameFromUserResponse.Success;
        }
    }

    public async Task<bool> HasGameInLibrary(string userId, string gameId)
    {
        await using (var context = new ApplicationDbContext())
        {
            var game = context.Game.FindAsync(gameId).Result;
            if (game == null)
            {
                return false;
            }

            var user = context.Users.Include(u => u.Games)
                .FirstOrDefaultAsync(u => u.Id == userId).Result;

            if (user == null)
            {
                return false;
            }

            if (user.Games.Contains(game))
            {
                return true;
            }

            return false;
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