using LinkGeek.AppIdentity;
using LinkGeek.Models;
using LinkGeek.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;

namespace LinkGeek.Pages.Shared;


[Authorize]
public partial class FeedPartial
{
    [Inject] public UserService UserService { get; set; }
    [CascadingParameter] public ApplicationUser? currentUser { get; set; }
    
    private List<Post> posts = new List<Post>();

    protected override void OnParametersSet()
    {
        if (currentUser != null)
        {
            posts = this.UserService.GetUserFeed(currentUser);
        }
    }
    
    private string GetImageSrc(ApplicationUser player)
    {
        return "data:" + player.ProfilePictureContentType + ";base64," + player.ProfilePictureData;
    }
}

