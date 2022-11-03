using LinkGeek.AppIdentity;
using LinkGeek.Models;
using LinkGeek.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;

namespace LinkGeek.Areas.Feed.Pages;

public class PostFormModel
{
    public string Content { get; set; }
    public Game? Game { get; set; }
}

[Authorize]
public partial class FeedPage
{
    [Inject] public UserService UserService { get; set; }
    [Inject] public GameService GameService { get; set; }
    [CascadingParameter] public ApplicationUser currentUser { get; set; }

    private PostFormModel postFormModel = new();

    private async Task CreatePostAsync()
    {
        var response = await this.UserService.CreatePost(currentUser, postFormModel.Content, postFormModel.Game);

        if (response == CreatePostResponse.Success)
        {
            postFormModel.Content = string.Empty;
            postFormModel.Game = null;
        }
    }
}

