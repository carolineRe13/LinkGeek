using LinkGeek.AppIdentity;
using LinkGeek.Models;
using LinkGeek.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace LinkGeek.Pages.Shared;


[Authorize]
public partial class PublishedPostPartial
{
    [Inject] public UserService UserService { get; set; }
    [Inject] private ISnackbar _snackBar { get; set; }
    [CascadingParameter] public ApplicationUser? currentUser { get; set; }
    [Parameter] public Post? post { get; set; }
    
    private bool isLiked = false;

    protected override async Task OnParametersSetAsync()
    {
        if (post != null && currentUser != null)
        {
            isLiked = await UserService.IsLiked(post, currentUser);
        }
    }

    private string GetImageSrc(ApplicationUser player)
    {
        return "data:" + player.ProfilePictureContentType + ";base64," + player.ProfilePictureData;
    }

    public void RefreshPost(Post updatedPost)
    {
        this.post = updatedPost;
        this.StateHasChanged();
    }
    
    private async Task Like(Post post)
    {
        if (currentUser != null)
        {
            var result = await UserService.UpdatePost(post, currentUser);

            if (result.UpdatePostResponseValue == UpdatePostResponseValue.Success)
            {
                isLiked = true;
                _snackBar.Add("Liked post", Severity.Info);
                this.post.Likes = result.UpdatedPost.Likes;
            }
            else if(result.UpdatePostResponseValue == UpdatePostResponseValue.SuccessfullyRemoved)
            {
                isLiked = false;
                _snackBar.Add("Removed like", Severity.Warning);
                this.post.Likes = result.UpdatedPost.Likes;
            }
            else
            {
                _snackBar.Add("An error occured while liking post", Severity.Error);
            }

        }
        else
        {
            _snackBar.Add("Error");
        }
        
        StateHasChanged();
    }
}