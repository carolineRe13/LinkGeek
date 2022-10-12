using LinkGeek.AppIdentity;
using LinkGeek.Areas.ProfileCard.Pages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Build.Framework;

namespace LinkGeek.Areas.Friends.Pages;

public class SearchModel
{
    [Required]
    public string? Search { get; set; }
}

[Authorize]
public partial class Friends
{
    public SearchModel search = new SearchModel();
    public ICollection<ApplicationUser> friendList { get; set; } = new List<ApplicationUser>();
    [Inject]
    AuthenticationStateProvider AuthenticationStateProvider { get; set; }
    
    [Inject]
    UserManager<ApplicationUser> UserManager { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
        var user = authState.User;
        
        if (user.Identity is { IsAuthenticated: true })
        {
            var currentUser = await UserManager.GetUserAsync(user);
            friendList = currentUser.GetFriendList();
        }
    }

    public async Task Search()
    {
        // results = ;

    }
}