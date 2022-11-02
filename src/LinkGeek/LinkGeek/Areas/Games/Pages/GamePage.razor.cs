using LinkGeek.AppIdentity;
using LinkGeek.Models;
using LinkGeek.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Graphics.Platform;

namespace LinkGeek.Areas.Games.Pages;

public partial class GamePage
{
    [Parameter]
    public string? GameId { get; set; }
    [Inject]
    public GameService GameService { get; set; }

    private Game Game { get; set; } = new();
    private ICollection<ApplicationUser> Players { get; set; } = new List<ApplicationUser>();

    protected override async Task OnInitializedAsync()
    {
        Game = await GameService.GetGameAsync(GameId);
        Players = await GameService.GetGamePlayersAsync(GameId);
    }

    /// <summary>
    /// Method <c>Base64ImageFromBytes</c> Creates an image from bytes
    /// </summary>
    public string Base64ImageFromBytes(byte[] profilePicture)
    {
        if (profilePicture.Length == 0)
        {
            return string.Empty;
        }
        
        MemoryStream ms = new MemoryStream(profilePicture,0,profilePicture.Length);
        ms.Write(profilePicture, 0, profilePicture.Length);
        
        IImage image;
        image = PlatformImage.FromStream(ms);
        return image.AsBase64();
    }

    private string GetImageSrc(ApplicationUser player)
    {
        return "data:" + player.ProfilePictureContentType + ";base64," + player.ProfilePictureData;
    }
}