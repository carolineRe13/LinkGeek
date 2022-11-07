using LinkGeek.AppIdentity;
using LinkGeek.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace LinkGeek.Areas.Friends.Pages;

[Authorize]
public partial class PendingFriends
{
    [Inject]
    private AuthenticationStateProvider AuthenticationStateProvider { get; set; }

    [Inject]
    UserService UserService { get; set; }
    
    [CascadingParameter]
    public ApplicationUser? CurrentUser { get; set; }
    
    public ICollection<ApplicationUser> pendingFriends { get; set; } = new List<ApplicationUser>();
    
    protected override async Task OnParametersSetAsync()
    {
        if (CurrentUser != null)
        {
            // best case we update the user, worst case it is still the same one. We use this to update the new friends
            CurrentUser = UserService.GetUserFromUserName(CurrentUser.UserName) ?? CurrentUser;
            pendingFriends = CurrentUser?.ReceivedFriendRequests ?? new List<ApplicationUser>();
            StateHasChanged();
        }
    }
}