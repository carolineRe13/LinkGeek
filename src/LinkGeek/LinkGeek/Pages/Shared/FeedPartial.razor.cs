using LinkGeek.AppIdentity;
using LinkGeek.Models;
using LinkGeek.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace LinkGeek.Pages.Shared;


[Authorize]
public partial class FeedPartial
{
    [Inject] 
    public UserService? UserService { get; set; }
    
    [CascadingParameter] public ApplicationUser? currentUser { get; set; }
    
    private List<Post> posts = new();

    protected override async Task OnParametersSetAsync()
    {
        if (currentUser != null && UserService != null)
        {
            posts = await this.UserService.GetUserFeedAsync(currentUser);
        }
    }
}

