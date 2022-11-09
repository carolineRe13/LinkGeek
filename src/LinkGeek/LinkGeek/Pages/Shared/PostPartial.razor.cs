using LinkGeek.AppIdentity;
using LinkGeek.Models;
using LinkGeek.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;

namespace LinkGeek.Pages.Shared;

public class PostFormModel
{
    public string Content { get; set; } = string.Empty;
    public Game? Game { get; set; }
    public DateTime? PlayingAtDate { get; set; }
    public TimeSpan? PlayingAtTime { get; set; }
    public TimeZoneInfo? TimeZone { get; set; }
    public PlayerRoles LookingFor { get; set; }
}

[Authorize]
public partial class PostPartial
{
    [Inject] public UserService UserService { get; set; }
    [Inject] public GameService GameService { get; set; }
    [Inject] public NavigationManager NavigationManager { get; set; }
    [CascadingParameter] public ApplicationUser currentUser { get; set; }

    private PostFormModel postFormModel = new();

    protected override void OnParametersSet()
    {
        this.StateHasChanged();
    }
    
    private async Task CreatePostAsync()
    {
        DateTimeOffset? playingAt = null;
        if (this.postFormModel.PlayingAtDate != null && this.postFormModel.PlayingAtTime != null &&
            this.postFormModel.TimeZone != null)
        {
            playingAt = new DateTimeOffset(this.postFormModel.PlayingAtDate.Value, this.postFormModel.TimeZone.BaseUtcOffset);
            playingAt = playingAt.Value.Add(this.postFormModel.PlayingAtTime.Value);
        }
        var response = await this.UserService.CreatePostAsync(currentUser, postFormModel.Content, postFormModel.Game, postFormModel.LookingFor, playingAt);

        if (response == CreatePostResponse.Success)
        {
            postFormModel.Content = string.Empty;
            postFormModel.Game = null;
            postFormModel.PlayingAtDate = null;
            postFormModel.PlayingAtTime = null;
            postFormModel.TimeZone = null;
            this.NavigationManager.NavigateTo(this.NavigationManager.Uri);
        }
    }
}

