using LinkGeek.AppIdentity;
using LinkGeek.Models;
using LinkGeek.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;

namespace LinkGeek.Pages.Shared;


[Authorize]
public partial class PublishedPostPartial
{
    [Parameter] public Post? post { get; set; }

    private string GetImageSrc(ApplicationUser player)
    {
        return "data:" + player.ProfilePictureContentType + ";base64," + player.ProfilePictureData;
    }

    public void RefreshPost(Post updatedPost)
    {
        this.post = updatedPost;
        this.StateHasChanged();
    }
}