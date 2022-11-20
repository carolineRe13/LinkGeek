using LinkGeek.AppIdentity;
using LinkGeek.Models;
using LinkGeek.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using MudBlazor;

namespace LinkGeek.Areas.Games.Pages;

/// <summary>
/// Class <c>GameCard</c> The functionality of the game cards including e.g. add, remove
/// </summary>
public partial class GameCard
{
    [Inject] 
    private UserService UserService { get; set; }
    
    [Inject]
    private ISnackbar Snackbar { get; set; }
    
    [Inject]
    private AuthenticationStateProvider AuthenticationStateProvider { get; set; }

    [Parameter]
    public Game Game { get; set; }

    [Parameter] 
    public bool DisplayGamePageButton { get; set; } = true;
    
    [CascadingParameter] ApplicationUser? currentUser { get; set; }
    
    private bool _isGameInLibrary = false;

    protected override void OnParametersSet()
    {
        IsGameInLibrary();
    }
    
    
    /// <summary>
    /// Method <c>AddGame</c> Game will be added to current user's library
    /// </summary>
    private async Task AddGame(string id)
    {
        if (currentUser == null) return;
        currentUser?.Games?.Add(this.Game);
        
        var response = await UserService.AddGameToUserAsync(currentUser.Id, id);
        switch (response)
        {
            case AddGameToUserResponse.Success:
                Snackbar.Add("Game added to library", Severity.Success);
                IsGameInLibrary();
                break;
            case AddGameToUserResponse.GameAlreadyAdded:
                Snackbar.Add("Game already in library", Severity.Warning);
                IsGameInLibrary();
                break;
            default:
                Snackbar.Add("Error adding game to library", Severity.Error);
                currentUser?.Games?.Remove(this.Game);
                break;
        }
    }

    /// <summary>
    /// Method <c>RemoveGame</c> Game will be removed from current user's library
    /// </summary>
    private async Task RemoveGame(string id)
    {
        if (currentUser == null) return;
        currentUser?.Games?.Remove(this.Game);

        var response = await UserService.RemoveGameFromUserAsync(currentUser.Id, id);
        switch (response)
        {
            case RemoveGameFromUserResponse.Success:
                Snackbar.Add("Game removed from library", Severity.Success);
                IsGameInLibrary();
                break;
            case RemoveGameFromUserResponse.GameAlreadyRemoved:
                Snackbar.Add("Game already removed from library", Severity.Warning);
                IsGameInLibrary();
                break;
            default:
                Snackbar.Add("Error adding game to library", Severity.Error);
                currentUser?.Games?.Add(this.Game);
                break;
        }
    }

    /// <summary>
    /// Method <c>IsGameInLibrary</c> returns true if the game is in the current user's library
    /// </summary>
    private void IsGameInLibrary()
    {
        if (currentUser == null) return;
        _isGameInLibrary = currentUser.Games != null && currentUser.Games.Any(game => game.Id == Game.Id);
        StateHasChanged();
    }
}