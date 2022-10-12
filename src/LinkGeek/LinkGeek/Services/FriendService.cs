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
            var currentUser = await context.Users.FindAsync(userId);
            var userToAdd = await context.Users.FindAsync(userToAddId);

            if (userToAdd.PendingOutgoingFriendsRequests.Contains(new FriendLinkOutgoing(userToAdd, currentUser)))
            {
                currentUser.Friends?.Add(new FriendLinkFriend(currentUser, userToAdd));
                userToAdd.Friends?.Add(new FriendLinkFriend(userToAdd, currentUser));

                userToAdd.PendingOutgoingFriendsRequests?.Remove(new FriendLinkOutgoing(currentUser, userToAdd));
                currentUser.PendingIncomingFriendsRequests?.Remove(new FriendLinkIncoming(currentUser, userToAdd));

                await context.SaveChangesAsync();
                return FriendsResponses.YouAreNowFriends;
            }
            else
            {
                currentUser.PendingOutgoingFriendsRequests?.Add(new FriendLinkOutgoing(currentUser, userToAdd));
                userToAdd.PendingIncomingFriendsRequests?.Add(new FriendLinkIncoming(userToAdd, currentUser));


                await context.SaveChangesAsync();
                return FriendsResponses.PendingFriends;
            }
        }

        return FriendsResponses.Failure;
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