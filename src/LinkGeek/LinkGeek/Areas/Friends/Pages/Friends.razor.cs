using LinkGeek.AppIdentity;
using LinkGeek.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
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
    UserService UserService { get; set; }
    
    [CascadingParameter]
    public ApplicationUser? CurrentUser { get; set; }

    protected override async Task OnParametersSetAsync()
    {
        if (CurrentUser != null)
        {
            // best case we update the user, worst case it is still the same one. We use this to update the new friends
            CurrentUser = await UserService.GetUserFromUserNameAsync(CurrentUser.UserName, includeLikedPosts: false, includeGames: false) ?? CurrentUser;
            friendList = CurrentUser?.Friends ?? new List<ApplicationUser>();
        }
    }

    public async Task Search()
    {
        // results = ;

    }
}