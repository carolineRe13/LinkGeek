using LinkGeek.AppIdentity;
using LinkGeek.Data;
using LinkGeek.Models;
using Microsoft.EntityFrameworkCore;

namespace LinkGeek.Services;

public enum FriendsResponses
{
    AlreadyFriends,
    YouAreNowFriends,
    PendingFriends,
    CanceledFriendRequest,
    FriendRemoved,
    Failure
}

public class FriendService
{
    private readonly IContextProvider contextProvider;

    public FriendService(IContextProvider contextProvider)
    {
        this.contextProvider = contextProvider;
    }

    public async Task<FriendsResponses> AddFriend(string userId, string userToAddId)
    {
        await using var context = contextProvider.GetContext();
        var currentUser = await GetApplicationUserWithFriendLists(context, userId);
        var userToAdd = await GetApplicationUserWithFriendLists(context, userToAddId);

        if (currentUser.Friends.Contains(userToAdd)) return FriendsResponses.AlreadyFriends;
        
        if (currentUser.ReceivedFriendRequests.Contains(userToAdd) &&
            userToAdd.SentFriendRequests.Contains(currentUser))
        {
            // Equivalent to accepting friend request
            currentUser.Friends.Add(userToAdd);
            userToAdd.Friends.Add(currentUser);

            userToAdd.SentFriendRequests.Remove(currentUser);
            currentUser.ReceivedFriendRequests.Remove(userToAdd);

            await context.SaveChangesAsync();
            return FriendsResponses.YouAreNowFriends;
        }
        
        // To avoid duplicates
        if (!currentUser.SentFriendRequests.Contains(userToAdd))
        {
            currentUser.SentFriendRequests.Add(userToAdd);
        }
        
        if (!userToAdd.ReceivedFriendRequests.Contains(currentUser))
        {
            userToAdd.ReceivedFriendRequests.Add(currentUser);
        }

        await context.SaveChangesAsync();
        return FriendsResponses.PendingFriends;
    }

    public async Task<FriendsResponses> CancelFriendRequest(string userId, string userToAddId)
    {
        await using var context = contextProvider.GetContext();
        var currentUser = await this.GetApplicationUserWithFriendLists(context, userId);
        var userToAdd = await this.GetApplicationUserWithFriendLists(context, userToAddId);
        
        currentUser.SentFriendRequests.Remove(userToAdd);
        userToAdd.ReceivedFriendRequests.Remove(currentUser);

        await context.SaveChangesAsync();
        return FriendsResponses.CanceledFriendRequest;
    }

    public async Task<FriendsResponses> DeclineFriendRequest(string userId, string userThatRequestedId)
    {
        await using var context = contextProvider.GetContext();
        var currentUser = await this.GetApplicationUserWithFriendLists(context, userId);
        var userThatRequested = await this.GetApplicationUserWithFriendLists(context, userThatRequestedId);
        
        userThatRequested.SentFriendRequests.Remove(currentUser);
        currentUser.ReceivedFriendRequests.Remove(userThatRequested);

        await context.SaveChangesAsync();
        return FriendsResponses.CanceledFriendRequest;
    }

    public async Task<FriendsResponses> RemoveFriend(string userId, string userToRemoveId)
    {
        await using var context = contextProvider.GetContext();
        var currentUser = await this.GetApplicationUserWithFriendLists(context, userId);
        var userToRemove = await this.GetApplicationUserWithFriendLists(context, userToRemoveId);
        
        currentUser.Friends.Remove(userToRemove);
        userToRemove.Friends.Remove(currentUser);

        await context.SaveChangesAsync();
        return FriendsResponses.FriendRemoved;
    }
    
    private Task<ApplicationUser> GetApplicationUserWithFriendLists(ApplicationDbContext context, string id)
    {
        return context.Users
            .Include(u => u.SentFriendRequests)
            .Include(u => u.ReceivedFriendRequests)
            .Include(u => u.Friends)
            .Include(u => u.MyFriends)
            .FirstAsync(u => u.Id == id);
    }
}