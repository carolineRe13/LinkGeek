using LinkGeek.AppIdentity;
using LinkGeek.Models;
using LinkGeek.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;

namespace LinkGeek.Pages.Shared;


[Authorize]
public partial class PublishedPostPartial
{
    [Inject] public UserService UserService { get; set; }
    [Inject] private ISnackbar _snackBar { get; set; }
    [Inject] private IJSRuntime _jsRuntime { get; set; }
    [Inject] private NavigationManager _navigationManager { get; set; }
    [CascadingParameter] public ApplicationUser? currentUser { get; set; }
    [Parameter] public Post? post { get; set; }
    private List<Comment>? orderedComments;

    private bool _isLiked = false;

    protected override async Task OnParametersSetAsync()
    {
        if (post != null && currentUser != null)
        {
            if (currentUser.LikedPosts.Count > 1)
            {
                _isLiked = currentUser.LikedPosts.Any(p => p.Id == post.Id);
            }
            else
            {
                _isLiked = await UserService.IsLikedAsync(post, currentUser);
            }
            this.orderedComments = post.Comments.OrderBy(p => p.CreatedAt).ToList();
        }
    }

    private string GetImageSrc(ApplicationUser player)
    {
        return "data:" + player.ProfilePictureContentType + ";base64," + player.ProfilePictureData;
    }

    public void RefreshPost(Post updatedPost)
    {
        this.post = updatedPost;
        this.orderedComments = post.Comments.OrderBy(p => p.CreatedAt).ToList();
        this.StateHasChanged();
    }
    
    private async Task Like(Post post)
    {
        if (currentUser != null)
        {
            var result = await UserService.UpdatePostAsync(post, currentUser);

            if (result.UpdatePostResponseValue == UpdatePostResponseValue.Success)
            {
                _isLiked = true;
                _snackBar.Add("Liked post", Severity.Info);
                this.post.Likes = result.UpdatedPost.Likes;
            }
            else if(result.UpdatePostResponseValue == UpdatePostResponseValue.SuccessfullyRemoved)
            {
                _isLiked = false;
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

    private async Task CopyShareLinkToClipboard()
    {
        var success = await _jsRuntime.InvokeAsync<bool>("clipboardCopy.copyText", _navigationManager.ToAbsoluteUri("/post/" + post?.Id));
        if (success)
        {
            this._snackBar.Add("Copied to clipboard", Severity.Success);
        }
        else
        {
            this._snackBar.Add("Could not copy to clipboard", Severity.Error);
        }
    }
}