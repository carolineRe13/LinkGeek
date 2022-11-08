using LinkGeek.AppIdentity;
using LinkGeek.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;

namespace LinkGeek.Areas.Post.Pages;


/// <summary>
/// Class <c>ProfilePage</c> a user's profile page. This can be the current user or someone else's profile
/// </summary>
[Authorize]
public partial class PostPage
{
    [Inject] private UserService _userService { get; set; }
    [Inject] private NavigationManager NavigationManager { get; set; }

    [CascadingParameter] public ApplicationUser? CurrentUser { get; set; }

    [Parameter] public string? PostId { get; set; }

    private Models.Post? post;

    protected override async Task OnParametersSetAsync()
    {
        if (CurrentUser == null) return;
        
        if (PostId == null)
        {
            NavigationManager.NavigateTo("/404");
        }
        else
        {
            post = await _userService.GetPost(PostId);
            if (post == null)
            {
                NavigationManager.NavigateTo("/404");
            }
            this.StateHasChanged();
        }
    }
}

