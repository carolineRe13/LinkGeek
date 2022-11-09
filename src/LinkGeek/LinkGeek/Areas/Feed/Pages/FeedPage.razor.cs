using LinkGeek.AppIdentity;
using LinkGeek.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace LinkGeek.Areas.Feed.Pages;

[Authorize]
public partial class FeedPage
{
    [Inject]
    AuthenticationStateProvider AuthenticationStateProvider { get; set; }
    
    [Inject]
    UserService UserService { get; set; }
    
    [CascadingParameter]
    public ApplicationUser? CurrentUser { get; set; }

    private List<Models.Post> userFeed;

    protected override async Task OnParametersSetAsync()
    {
        if (CurrentUser != null)
        {
            this.userFeed = await this.UserService.GetUserFeedAsync(CurrentUser);
        }
    }
}