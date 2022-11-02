using LinkGeek.AppIdentity;
using LinkGeek.Data;
using LinkGeek.Models;
using Microsoft.EntityFrameworkCore;

namespace LinkGeek.Services;

public enum FriendsResponses
{
    YouAreNowFriends,
    PendingFriends,
    CanceledFriendRequest,
    FriendRemoved,
    Failure
    
}

public class FriendService
{
    public async Task<FriendsResponses> AddFriend(string userId, string userToAddId) {
        using (var context = new ApplicationDbContext())
        {
            var currentUser = await GetApplicationUserWithFriendLists(context, userId);
            var userToAdd = await GetApplicationUserWithFriendLists(context, userToAddId);

            if (currentUser.GetPendingIncomingFriendList().Select(f => f.Id).Contains(userToAddId) &&
                userToAdd.GetPendingOutgoingFriendList().Select(f => f.Id).Contains(userId))
            {
                // Equivalent to accepting friend request
                currentUser.Friends?.Add(new FriendLinkFriend(currentUser, userToAdd));
                userToAdd.Friends?.Add(new FriendLinkFriend(userToAdd, currentUser));

                userToAdd.PendingOutgoingFriendsRequests?.Remove(new FriendLinkOutgoing(currentUser, userToAdd));
                currentUser.PendingIncomingFriendsRequests?.Remove(new FriendLinkIncoming(currentUser, userToAdd));

                await context.SaveChangesAsync();
                return FriendsResponses.YouAreNowFriends;
            }
            else if (!currentUser.GetPendingOutgoingFriendList().Select(f => f.Id).Contains(userToAddId) || // prevent duplicates
                     !userToAdd.GetPendingIncomingFriendList().Select(f => f.Id).Contains(userId))
            {
                // Send friend request
                currentUser.PendingOutgoingFriendsRequests?.Add(new FriendLinkOutgoing(currentUser, userToAdd));
                userToAdd.PendingIncomingFriendsRequests?.Add(new FriendLinkIncoming(currentUser, userToAdd));

                await context.SaveChangesAsync();
                return FriendsResponses.PendingFriends;
            }
        }

        return FriendsResponses.Failure;
    }

    private Task<ApplicationUser> GetApplicationUserWithFriendLists(ApplicationDbContext context, string id)
    {
        return context.Users
            .Include(u => u.PendingIncomingFriendsRequests)
            .Include(u => u.PendingOutgoingFriendsRequests)
            .Include(u => u.Friends)
            .FirstAsync(u => u.Id == id);
    }

    public async Task<FriendsResponses> CancelFriendRequest(string userId, string userToAddId)
    {
        using (var context = new ApplicationDbContext())
        {
            var currentUser = await context.Users.FindAsync(userId);
            var userToAdd = await context.Users.FindAsync(userToAddId);

            if (currentUser != null && userToAdd != null)
            {
                currentUser?.PendingOutgoingFriendsRequests?.Remove(new FriendLinkOutgoing(currentUser, userToAdd));
                userToAdd?.PendingIncomingFriendsRequests?.Remove(new FriendLinkIncoming(userToAdd, currentUser));

                await context.SaveChangesAsync();
                return FriendsResponses.CanceledFriendRequest;
            }
        }

        return FriendsResponses.Failure;
    }

    public async Task<FriendsResponses> RemoveFriend(string userId, string userToAddId)
    {
        using (var context = new ApplicationDbContext())
        {
            var currentUser = await context.Users.FindAsync(userId);
            var userToAdd = await context.Users.FindAsync(userToAddId);

            if (currentUser != null && userToAdd != null)
            {
                currentUser?.Friends?.Remove(new FriendLinkFriend(currentUser, userToAdd));
                userToAdd?.Friends?.Remove(new FriendLinkFriend(userToAdd, currentUser));

                await context.SaveChangesAsync();
                return FriendsResponses.FriendRemoved;

            }
        }

        return FriendsResponses.Failure;

    }
}