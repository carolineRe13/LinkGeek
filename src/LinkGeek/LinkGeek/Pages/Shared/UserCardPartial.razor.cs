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
    [Inject]
    private UserService userService { get; init; }

    [Inject] 
    private FriendService FriendService { get; init; }

    [Parameter]
    public ApplicationUser? DisplayedUser { get; init; }

    [Parameter]
    public bool IsCurrentUser { get; init; }

    [CascadingParameter]
    public ApplicationUser? CurrentUser { get; init; }

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
            Snackbar.Add(result.ToString());
            StateHasChanged();
        }
        else
        {
            Snackbar.Add("Error");
        }
    }
}