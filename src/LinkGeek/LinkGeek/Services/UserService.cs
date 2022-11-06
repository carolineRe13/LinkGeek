using LinkGeek.AppIdentity;
using LinkGeek.Data;
using LinkGeek.Models;
using LinkGeek.Pages;
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

public enum CreatePostResponse
{
    Success,
}

public class UserService
{
    private readonly IContextProvider contextProvider;

    public UserService(IContextProvider contextProvider)
    {
        this.contextProvider = contextProvider;
    }

    public async Task<AddGameToUserResponse?> AddGameToUser(string userId, string gameId)
    {
        await using (var context = contextProvider.GetContext())
        {
            var game = await context.Game.FindAsync(gameId);
            if (game == null)
            {
                return AddGameToUserResponse.GameNotFound;
            }

            var user = await GetUserWithGames(context, userId);

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

    public async Task<CreatePostResponse?> CreatePost(ApplicationUser user, string content, Game? game)
    {
        
        await using (var context = contextProvider.GetContext())
        {
            var contextUser = GetUserFromUserName(context, user.UserName);
            var contextGame = game != null ? await context.Game.FindAsync(game.Id) : null;
            var post = new Post
            {
                ApplicationUser = contextUser!,
                Content = content,
                Game = contextGame
            };
            context.Posts.Add(post);
            await context.SaveChangesAsync();
        }

        return CreatePostResponse.Success;
    }
    
    public async Task<RemoveGameFromUserResponse?> RemoveGameFromUser(string userId, string gameId)
    {
        await using (var context = contextProvider.GetContext())
        {
            var game = await context.Game.FindAsync(gameId);
            if (game == null)
            {
                return RemoveGameFromUserResponse.GameNotFound;
            }

            var user = await GetUserWithGames(context, userId);

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
        await using (var context = contextProvider.GetContext())
        {
            var game = context.Game.FindAsync(gameId).Result;
            if (game == null)
            {
                return false;
            }

            var user = await GetUserWithGames(context, userId);

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

    public async Task<string?> UpdateLocation(ApplicationUser user, string location) =>
        await UpdateLocation(user.Id, location);


    public async Task<string?> UpdateLocation(string userId, string location)
    {
        await using (var context = contextProvider.GetContext())
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

    public async Task<string?> UpdateStatus(ApplicationUser user, string status) =>
        await UpdateStatus(user.Id, status);

    public async Task<string?> UpdateStatus(string userId, string status)
    {
        // TODO: Add profanity filter
        await using (var context = contextProvider.GetContext())
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

    public async Task<ICollection<Game>> GetUsersGamesAsync(string userId)
    {
        await using (var context = contextProvider.GetContext())
        {
            var user = await GetUserWithGames(context, userId);
            return user.Games ?? new List<Game>();
        }
    }

    public ApplicationUser? GetUserFromUserName(string userName)
    {
        using var context = contextProvider.GetContext();
        return GetUserFromUserName(context, userName);
    }

    public ApplicationUser? GetUserFromUserName(ApplicationDbContext context, string userName)
    {
        return context.Users
            .Include(u => u.Friends)
            .Include(u => u.SentFriendRequests)
            .Include(u => u.ReceivedFriendRequests)
            .Include(u => u.Games)
            .FirstOrDefault(u => u.UserName == userName);
    }

    public List<Post> GetUserFeed(ApplicationUser user)
    {
        using var context = contextProvider.GetContext();
        
        var feed = context.Posts
            .Include(p => p.ApplicationUser)
            .Include(p => p.Game)
            .Where(post => post.ApplicationUser.Id == user.Id
                       || user.Friends.Select(f => f.Id).Contains(post.ApplicationUser.Id)
                       || (user.Games != null && post.Game != null && user.Games.Select(f => f.Id).Contains(post.Game.Id)))
            .OrderByDescending(p => p.CreatedAt)
            .ToList();

        return feed;
    }

    private Task<ApplicationUser> GetUserWithGames(ApplicationDbContext context, string userId)
    {
        return context.Users.Include(u => u.Games)
            .FirstAsync(u => u.Id == userId);
    }
}