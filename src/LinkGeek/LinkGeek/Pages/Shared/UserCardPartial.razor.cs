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

    public bool AreCurrentlyFriends()
    {
        if (CurrentUser != null && DisplayedUser != null && CurrentUser.Friends.Contains(DisplayedUser))
        {
            return true;
        }

        return false;
    }

    public bool AreCurrentlyPendingFriends()
    {
        if (CurrentUser != null && DisplayedUser != null && CurrentUser.SentFriendRequests.Contains(DisplayedUser))
        {
            return true;
        }

        return false;
    }

    public bool IsAdded()
    {
        if (CurrentUser != null && DisplayedUser != null && CurrentUser.ReceivedFriendRequests.Contains(DisplayedUser))
        {
            return true;
        }

        return false;
    }

    public string GetImageSrc()
    {
        return "data:" + CurrentUser?.ProfilePictureContentType + ";base64," + CurrentUser?.ProfilePictureData;
    }

    public async Task<ICollection<Game>> GetGames()
    {
        return DisplayedUser == null ? new List<Game>() : await userService.GetUsersGamesAsync(DisplayedUser.Id);
    }


    public void AddFriend()
    {
        if (CurrentUser != null && DisplayedUser != null)
        {
            var result = FriendService.AddFriend(CurrentUser.Id, DisplayedUser.Id);
            Snackbar.Add(result.ToString());
            StateHasChanged();
        }
        else
        {
            Snackbar.Add("Error");
        }
    }
}