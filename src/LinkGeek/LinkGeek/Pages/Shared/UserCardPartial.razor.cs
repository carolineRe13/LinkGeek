using LinkGeek.AppIdentity;
using LinkGeek.Models;
using LinkGeek.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;

namespace LinkGeek.Pages.Shared;

[Authorize]
public partial class UserCardPartial
{
    [Inject] private UserService userService { get; init; }

    [Parameter]
    public ApplicationUser? User { get; init; }

    [Parameter]
    public bool IsCurrentUser { get; init; }

    [Parameter]
    public ApplicationUser? OtherUser { get; init; }

    public bool AreCurrentlyFriends()
    {
        if (User != null && User.GetFriendList().Contains(OtherUser))
        {
            return true;
        }

        return false;
    }

    public bool AreCurrentlyPendingFriends()
    {
        if (User != null && User.GetPendingOutgoingFriendList().Contains(OtherUser))
        {
            return true;
        }

        return false;
    }

    public bool IsAdded()
    {
        if (User != null && User.GetPendingIncomingFriendList().Contains(OtherUser))
        {
            return true;
        }

        return false;
    }

    public string GetImageSrc()
    {
        return "data:" + User.ProfilePictureContentType + ";base64," + User.ProfilePictureData;
    }

    public async Task<ICollection<Game>> GetGames()
    {
        return User == null ? new List<Game>() : await userService.GetUsersGamesAsync(User.Id);
    }
}