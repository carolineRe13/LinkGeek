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

public enum UpdatePostResponseValue
{
    Success,
    Failure,
    SuccessfullyRemoved
}

public record UpdatePostResponse(UpdatePostResponseValue UpdatePostResponseValue, Post? UpdatedPost);

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

    public async Task<ApplicationUser?> GetUserFromUserNameAsync(string userName)
    {
        await using var context = contextProvider.GetContext();
        return await GetUserFromUserNameAsync(context, userName);
    }

    public async Task<ApplicationUser?> GetUserFromUserNameAsync(ApplicationDbContext context, string userName)
    {
        return await context.Users
            .Include(u => u.Friends)
            .Include(u => u.SentFriendRequests)
            .Include(u => u.ReceivedFriendRequests)
            .Include(u => u.Games)
            .FirstOrDefaultAsync(u => u.UserName == userName);
    }

    public async Task<CreatePostResponse?> CreatePostAsync(ApplicationUser user, string content, Game? game, PlayerRoles lookingFor, DateTimeOffset? playingAt)
    {
        await using (var context = contextProvider.GetContext())
        {
            var contextUser = await GetUserFromUserNameAsync(context, user.UserName);
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
        }

        return CreatePostResponse.Success;
    }

    public async Task<Post?> PostComment(ApplicationUser user, Post post, string content)
    {
        await using var context = contextProvider.GetContext();
        var contextUser = await GetUserFromUserNameAsync(context, user.UserName);
        if (contextUser == null) return default;
            
        var existingPost = await context.Posts
            .Include(p => p.Likes)
            .Include(p => p.Comments)
            .Where(p => p.Id == post.Id)
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
    
    public async Task<UpdatePostResponse> UpdatePost(Post post, ApplicationUser currentUser)
    {
        await using (var context = contextProvider.GetContext())
        {
            var contextUser = await this.GetUserFromUserNameAsync(context, currentUser.UserName);
            if (contextUser == null) return new UpdatePostResponse(UpdatePostResponseValue.Failure, null);

            var contextPost = await context.Posts
                .Include(p => p.Likes)
                .Where(p => p.Id == post.Id)
                .FirstOrDefaultAsync();
            if (contextPost == null) return new UpdatePostResponse(UpdatePostResponseValue.Failure, null);
            
            if (contextPost.Likes.Any(u => u.Id == contextUser.Id))
            {
                contextPost.Likes.Remove(contextUser);
                await context.SaveChangesAsync();
                return new UpdatePostResponse(UpdatePostResponseValue.SuccessfullyRemoved, contextPost);
            }
            contextPost.Likes.Add(contextUser);
            await context.SaveChangesAsync();
            return new UpdatePostResponse(UpdatePostResponseValue.Success, contextPost);
        }
    }
    
    public async Task<bool> IsLiked(Post post, ApplicationUser currentUser)
    {
        await using (var context = contextProvider.GetContext())
        {
            var contextUser = await this.GetUserFromUserNameAsync(context, currentUser.UserName);
            if (contextUser == null) return false;

            var contextPost = await context.Posts.Include(p => p.Likes).Where(p => p.Id == post.Id).FirstOrDefaultAsync();
            if (contextPost == null) return false;
            if (contextPost.Likes.Any(u => u.Id == contextUser.Id))
            {
                return true;
            }

            return false;
        }
    }
    
    public async Task<List<Post>> GetUserFeedAsync(ApplicationUser user)
    {
        await using var context = contextProvider.GetContext();
        
        var feed = await context.Posts
            .Include(p => p.ApplicationUser)
            .Include(p => p.Game)
            .Include(p => p.Likes)
            .Include(p => p.Comments.OrderBy(c => c.CreatedAt))
            .Where(post => post.ApplicationUser.Id == user.Id
                           || user.Friends.Select(f => f.Id).Contains(post.ApplicationUser.Id)
                           || (user.Games != null && post.Game != null && user.Games.Select(f => f.Id).Contains(post.Game.Id)))
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();

        return feed;
    }
    
    public async Task<Post?> GetPost(string postId)
    {
        await using var context = contextProvider.GetContext();
        
        return await context.Posts
            .Include(p => p.ApplicationUser)
            .Include(p => p.Game)
            .Include(p => p.Comments.OrderBy(c => c.CreatedAt))
            .Where(post => post.Id.ToString() == postId)
            .FirstOrDefaultAsync();
    }

    private Task<ApplicationUser> GetUserWithGames(ApplicationDbContext context, string userId)
    {
        return context.Users.Include(u => u.Games)
            .FirstAsync(u => u.Id == userId);
    }
}