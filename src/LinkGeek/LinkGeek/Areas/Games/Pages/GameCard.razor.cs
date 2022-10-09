using LinkGeek.AppIdentity;
using LinkGeek.Models;
using LinkGeek.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using MudBlazor;

namespace LinkGeek.Areas.Games.Pages;

public partial class GameCard
{
    [Inject]
    AuthenticationStateProvider AuthenticationStateProvider { get; set; }
    
    [Inject]
    UserManager<ApplicationUser> UserManager { get; set; }

    [Inject] 
    private UserService UserService { get; set; }
    
    [Inject]
    private ISnackbar Snackbar { get; set; }

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
                    Snackbar.Add("Game added to library", Severity.Success);
                    break;
                case AddGameToUserResponse.GameAlreadyAdded:
                    Snackbar.Add("Game already in library", Severity.Warning);
                    break;
                default:
                    Snackbar.Add("Error adding game to library", Severity.Error);
                    break;
            }
        }
    }
    
    private async Task RemoveGame(string id)
    {
        var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
        var user = authState.User;
        if (user.Identity is { IsAuthenticated: true })
        {
            var userId = (await UserManager.GetUserAsync(user)).Id;
            var response = await UserService.RemoveGameFromUser(userId, id);
            
            switch (response)
            {
                case RemoveGameFromUserResponse.Success:
                    Snackbar.Add("Game removed from library", Severity.Success);
                    break;
                case RemoveGameFromUserResponse.GameAlreadyRemoved:
                    Snackbar.Add("Game already removed from library", Severity.Warning);
                    break;
                default:
                    Snackbar.Add("Error adding game to library", Severity.Error);
                    break;
            }
        }
    }
    
    private bool IsGameInLibrary(string id)
    {
        var authState = AuthenticationStateProvider.GetAuthenticationStateAsync().Result;
        var user = authState.User;
        if (user.Identity is { IsAuthenticated: true })
        {
            var userId = (UserManager.GetUserAsync(user)).Result.Id;
            return UserService.HasGameInLibrary(userId, id).Result;
        }

        return false;
    }
}