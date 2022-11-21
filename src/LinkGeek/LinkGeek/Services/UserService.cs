using LinkGeek.AppIdentity;
using LinkGeek.Data;
using LinkGeek.Models;
using LinkGeek.Pages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

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

public enum UpdatePostResponseValue
{
    Success,
    Failure,
    SuccessfullyRemoved
}

public record UpdatePostResponse(UpdatePostResponseValue UpdatePostResponseValue, Post? UpdatedPost);

public class UserService
{
    private readonly IContextProvider _contextProvider;

    public UserService(IContextProvider contextProvider)
    {
        this._contextProvider = contextProvider;
    }

    public async Task<AddGameToUserResponse?> AddGameToUserAsync(string userId, string gameId)
    {
        await using var context = _contextProvider.GetContext();
        var game = await context.Game.FindAsync(gameId);
        if (game == null)
        {
            return AddGameToUserResponse.GameNotFound;
        }

        var user = await GetUserWithGamesAsync(context, userId);
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
    
    public async Task<RemoveGameFromUserResponse?> RemoveGameFromUserAsync(string userId, string gameId)
    {
        await using var context = _contextProvider.GetContext();
        var game = await context.Game.FindAsync(gameId);
        if (game == null)
        {
            return RemoveGameFromUserResponse.GameNotFound;
        }

        var user = await GetUserWithGamesAsync(context, userId);
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

    public async Task<bool> HasGameInLibraryAsync(string userId, string gameId)
    {
        await using var context = _contextProvider.GetContext();
        var game = context.Game.FindAsync(gameId).Result;
        if (game == null)
        {
            return false;
        }

        var user = await GetUserWithGamesAsync(context, userId);

        if (user == null)
        {
            return false;
        }

        return user.Games != null && user.Games.Contains(game);
    }

    public async Task<string?> UpdateLocationAsync(ApplicationUser user, string location) =>
        await UpdateLocationAsync(user.Id, location);

    public async Task<string?> UpdateLocationAsync(string userId, string location)
    {
        await using var context = _contextProvider.GetContext();
        var user = await context.Users.FindAsync(userId);
        if (user == null)
        {
            return null;
        }

        user.Location = location;
        await context.SaveChangesAsync();
        return user.Location;
    }

    public async Task<string?> UpdateStatusAsync(ApplicationUser user, string status) =>
        await UpdateStatusAsync(user.Id, status);

    public async Task<string?> UpdateStatusAsync(string userId, string status)
    {
        // TODO: Add profanity filter
        await using var context = _contextProvider.GetContext();
        var user = await context.Users.FindAsync(userId);
        if (user == null)
        {
            return null;
        }

        user.Status = status;
        await context.SaveChangesAsync();
        return user.Status;
    }

    public async Task<ICollection<Game>> GetUsersGamesAsync(string userId)
    {
        await using var context = _contextProvider.GetContext();
        var user = await GetUserWithGamesAsync(context, userId);
        return user?.Games ?? new List<Game>();
    }

    public async Task<ApplicationUser?> GetUserFromUserNameAsync(
        string userName, 
        bool includeFriends = true,
        bool includeGames = true,
        bool includeLikedPosts = true)
    {
        await using var context = _contextProvider.GetContext();
        return await GetUserFromUserNameAsync(context, userName, includeFriends, includeGames, includeLikedPosts);
    }

    public async Task<ApplicationUser?> GetUserFromUserNameAsync(
        ApplicationDbContext context, 
        string userName, 
        bool includeFriends = true,
        bool includeGames = true,
        bool includeLikedPosts = true)
    {
        var a = context.Users.AsQueryable();
        if (includeFriends)
        {
            a = a.Include(u => u.Friends)
                .Include(u => u.SentFriendRequests)
                .Include(u => u.ReceivedFriendRequests);
        }

        if (includeGames)
        {
            a = a.Include(u => u.Games);
        }

        if (includeLikedPosts)
        {
            a = a.Include(u => u.LikedPosts);
        }
        return await a.AsSplitQuery().FirstOrDefaultAsync(u => u.UserName == userName);
    }

    public async Task<CreatePostResponse?> CreatePostAsync(ApplicationUser user, string content, Game? game, PlayerRoles lookingFor, DateTimeOffset? playingAt)
    {
        await using var context = _contextProvider.GetContext();
        var contextUser = await this.GetUserFromUserNameAsync(context, user.UserName);
        var contextGame = game != null ? await context.Game.FindAsync(game.Id) : null;
        var post = new Post
        {
            ApplicationUser = contextUser!,
            Content = content,
            Game = contextGame,
            PlayingAt = playingAt,
            LookingFor = lookingFor == PlayerRoles.None ? null : lookingFor,
        };
        context.Posts.Add(post);
        await context.SaveChangesAsync();

        return CreatePostResponse.Success;
    }

    public async Task<Post?> PostCommentAsync(ApplicationUser user, Post post, string content)
    {
        await using var context = _contextProvider.GetContext();
        var contextUser = await GetUserFromUserNameAsync(context, user.UserName);
        if (contextUser == null) return default;
            
        var existingPost = await context.Posts
            .Include(p => p.Likes)
            .Include(p => p.Comments)
            .Where(p => p.Id == post.Id)
            .AsSplitQuery()
            .FirstOrDefaultAsync();
        if (existingPost == null) return default;

        var comment = new Comment
        {
            ApplicationUser = contextUser,
            Text = content
        };
        context.Comments.Add(comment);
        existingPost?.Comments.Add(comment);
        await context.SaveChangesAsync();
        
        return existingPost;
    }
    
    public async Task<UpdatePostResponse> UpdatePostAsync(Post post, ApplicationUser currentUser)
    {
        await using var context = _contextProvider.GetContext();
        var contextPost = await context.Posts
            .Include(p => p.Likes)
            .Where(p => p.Id == post.Id)
            .FirstOrDefaultAsync();
        if (contextPost == null) return new UpdatePostResponse(UpdatePostResponseValue.Failure, null);
            
        if (contextPost.Likes.Any(u => u.Id == currentUser.Id))
        {
            contextPost.Likes.Remove(contextPost.Likes.Single(u => u.Id == currentUser.Id));
            await context.SaveChangesAsync();
            return new UpdatePostResponse(UpdatePostResponseValue.SuccessfullyRemoved, contextPost);
        }
        
        var contextUser = await this.GetUserFromUserNameAsync(context, currentUser.UserName, includeFriends: false, includeGames: false, includeLikedPosts: false);
        if (contextUser == null) return new UpdatePostResponse(UpdatePostResponseValue.Failure, null);
        contextPost.Likes.Add(contextUser);
        await context.SaveChangesAsync();
        return new UpdatePostResponse(UpdatePostResponseValue.Success, contextPost);
    }
    
    public async Task<bool> IsLikedAsync(Post post, ApplicationUser currentUser)
    {
        await using var context = _contextProvider.GetContext();
        var contextUser = await this.GetUserFromUserNameAsync(context, currentUser.UserName);
        if (contextUser == null) return false;

        var contextPost = await context.Posts.Include(p => p.Likes).Where(p => p.Id == post.Id).FirstOrDefaultAsync();
        return contextPost != null && contextPost.Likes.Any(u => u.Id == contextUser.Id);
    }
    
    public async Task<List<Post>> GetUserFeedAsync(ApplicationUser user)
    {
        await using var context = _contextProvider.GetContext();
        
        return await context.Posts
            .Include(p => p.ApplicationUser)
            .Include(p => p.Game)
            .Include(p => p.Likes)
            .Include(p => p.Comments)
                .ThenInclude(c => c.ApplicationUser)
            .Where(post => post.ApplicationUser.Id == user.Id
                           || user.Friends.Select(f => f.Id).Contains(post.ApplicationUser.Id)
                           || (user.Games != null && post.Game != null && user.Games.Select(f => f.Id).Contains(post.Game.Id)))
            .OrderByDescending(p => p.CreatedAt)
            .AsSplitQuery()
            .ToListAsync();
    }
    
    public async Task<Post?> GetPostAsync(string postId)
    {
        await using var context = _contextProvider.GetContext();
        
        return await context.Posts
            .Include(p => p.ApplicationUser)
            .Include(p => p.Game)
            .Include(p => p.Comments.OrderBy(c => c.CreatedAt))
            .Include(p => p.Likes)
            .Where(post => post.Id.ToString() == postId)
            .AsSplitQuery()
            .FirstOrDefaultAsync();
    }

    private async Task<ApplicationUser?> GetUserWithGamesAsync(ApplicationDbContext context, string userId)
    {
        return await context.Users.Include(u => u.Games)
            .FirstOrDefaultAsync(u => u.Id == userId);
    }
}