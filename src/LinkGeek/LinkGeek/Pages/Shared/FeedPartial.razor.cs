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
    public UserService UserService { get; set; }
    [Inject] 
    private ISnackbar _snackBar { get; set; }
    [CascadingParameter] public ApplicationUser? currentUser { get; set; }
    
    private List<Post> posts = new List<Post>();

    protected override void OnParametersSet()
    {
        if (currentUser != null && UserService != null)
        {
            posts = this.UserService.GetUserFeed(currentUser);
        }
    }
    
    private string GetImageSrc(ApplicationUser player)
    {
        return "data:" + player.ProfilePictureContentType + ";base64," + player.ProfilePictureData;
    }
    
    private async Task Like(Post post)
    {
        if (currentUser != null)
        {
            var result = await UserService.UpdatePost(post, currentUser);

            if (result == UpdatePostResponse.Success)
            {
                _snackBar.Add("Liked post");
            }
            else if(result == UpdatePostResponse.SuccessfullyRemoved)
            {
                _snackBar.Add("Removed like");
            }
        }
        else
        {
            _snackBar.Add("Error");
        }
        
        StateHasChanged();
    }
    
    private async Task<bool> Liked(Post post)
    {
        var result = false;
        if (currentUser != null)
        {
            result =  await UserService.IsLiked(post, currentUser);
        }
        return result;
    }

    public void RefreshPost(Post post)
    {
        var index = posts.FindIndex(p => p.Id == post.Id);
        if (index != -1)
        {
            posts[index] = post;
            this.StateHasChanged();
        }
    }
}

