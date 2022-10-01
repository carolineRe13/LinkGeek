using LinkGeek.AppIdentity;
using LinkGeek.Models;
using LinkGeek.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;

namespace LinkGeek.Areas.Games.Pages;

public partial class GameCard
{
    [Inject]
    AuthenticationStateProvider AuthenticationStateProvider { get; set; }
    
    [Inject]
    UserManager<ApplicationUser> UserManager { get; set; }

    [Inject] 
    private UserService UserService { get; set; }

    [Parameter]
    public Game Game { get; set; }

    [Parameter] 
    public bool DisplayGamePageButton { get; set; } = true;
    
    private async Task AddGame(string id)
    {
        var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
        var user = authState.User;
        if (user.Identity is { IsAuthenticated: true })
        {
            var userId = (await UserManager.GetUserAsync(user)).Id;
            var response = await UserService.AddGameToUser(userId, id);
            
            switch (response)
            {
                case AddGameToUserResponse.Success:
                    // await NotificationService.Success("Game added to library");
                    break;
                case AddGameToUserResponse.GameAlreadyAdded:
                    // await NotificationService.Warning("Game already in library");
                    break;
                default:
                    // await NotificationService.Error("Error adding game to library");
                    break;
            }
        }
        
    }
}