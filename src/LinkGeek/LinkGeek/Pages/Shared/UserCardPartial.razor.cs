using LinkGeek.AppIdentity;
using LinkGeek.Models;
using LinkGeek.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace LinkGeek.Pages.Shared;

[Authorize]
public partial class UserCardPartial
{
    private record SnackBarContent(string Text, Severity Severity);

    private Dictionary<FriendsResponses, SnackBarContent> _friendsResponses = new()
    {
        { FriendsResponses.AlreadyFriends, new SnackBarContent("You're already friends", Severity.Warning) },
        { FriendsResponses.FriendRemoved, new SnackBarContent("Friend removed", Severity.Info) },
        { FriendsResponses.PendingFriends, new SnackBarContent("Friend request sent", Severity.Success) },
        { FriendsResponses.Failure, new SnackBarContent("Failure", Severity.Error) },
        { FriendsResponses.YouAreNowFriends, new SnackBarContent("You are now friends", Severity.Success) },
        { FriendsResponses.CanceledFriendRequest, new SnackBarContent("Canceled friend request", Severity.Warning) },
        { FriendsResponses.DeclinedFriendRequest, new SnackBarContent("Declined friend request", Severity.Warning) }
    };

    [Inject]
    private UserService userService { get; init; }

    [Inject] 
    private FriendService FriendService { get; init; }

    [Parameter]
    public ApplicationUser? DisplayedUser { get; set; }

    [Parameter]
    public bool IsCurrentUser { get; init; }

    [CascadingParameter]
    public ApplicationUser? CurrentUser { get; set; }

    [Inject]
    private ISnackbar Snackbar { get; set; }

    // Also returns true if the current user only added the other user
    public bool AreCurrentlyFriends()
    {
        return CurrentUser != null
               && DisplayedUser != null
               && CurrentUser.Friends.Select(f => f.Id).Contains(DisplayedUser.Id);
    }

    private bool FriendRequestSent()
    {
        return CurrentUser != null
               && DisplayedUser != null
               && CurrentUser.SentFriendRequests.Select(f => f.Id).Contains(DisplayedUser.Id);
    }

    public bool FriendRequestReceived()
    {
        return CurrentUser != null
               && DisplayedUser != null 
               && CurrentUser.ReceivedFriendRequests.Select(f => f.Id).Contains(DisplayedUser.Id);
    }

    private string GetImageSrc()
    {
        return "data:" + DisplayedUser?.ProfilePictureContentType + ";base64," + DisplayedUser?.ProfilePictureData;
    }

    private async Task<ICollection<Game>> GetGames()
    {
        return DisplayedUser == null ? new List<Game>() : await userService.GetUsersGamesAsync(DisplayedUser.Id);
    }

    private async Task AddFriend()
    {
        await this.CallUserServiceMethod(FriendService.AddFriend);
    }

    private async Task CancelFriendRequest()
    {
        await this.CallUserServiceMethod(FriendService.CancelFriendRequest);
    }

    private async Task DeclineFriendRequest()
    {
        await this.CallUserServiceMethod(FriendService.DeclineFriendRequest);
    }

    private async Task RemoveFriend()
    {
        await this.CallUserServiceMethod(FriendService.RemoveFriend);
    }

    private async Task CallUserServiceMethod(Func<string, string, Task<FriendsResponses>> method)
    {
        if (CurrentUser != null && DisplayedUser != null)
        {
            var result = await method(CurrentUser.Id, DisplayedUser.Id);
            Snackbar.Add(this._friendsResponses[result].Text, this._friendsResponses[result].Severity);
            await this.ReloadUsers();
            StateHasChanged();
        }
        else
        {
            Snackbar.Add("Error");
        }
    }

    private async Task ReloadUsers()
    {
        this.CurrentUser = this.userService.GetUserFromUserName(CurrentUser.UserName) ?? this.CurrentUser;
        this.DisplayedUser = this.userService.GetUserFromUserName(DisplayedUser.UserName) ?? this.DisplayedUser;

        this.DisplayedUser.Games = await GetGames();
    }
}