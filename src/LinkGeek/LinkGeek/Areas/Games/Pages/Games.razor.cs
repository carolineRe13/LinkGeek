using System.ComponentModel.DataAnnotations;
using LinkGeek.AppIdentity;
using LinkGeek.Models;
using LinkGeek.Services;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;

namespace LinkGeek.Areas.Games.Pages;

public class SearchModel
{
    [Required]
    public string? Search { get; set; }
}

[Authorize]
public partial class Games
{
    [Inject]
    AuthenticationStateProvider AuthenticationStateProvider { get; set; }
    
    [Inject]
    UserManager<ApplicationUser> UserManager { get; set; }
    
    [Inject] 
    private GameDbService GameDbService { get; set; }
    
    [Inject] 
    private UserService UserService { get; set; }
    
    public SearchModel search = new SearchModel();
    public ICollection<Game> results { get; set; } = new List<Game>();
    
    public async Task Search()
    {
        results = await GameDbService.SearchGames(search.Search);
    }
    
    public async Task AddGame(string id)
    {
        var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
        var user = authState.User;
        if (user.Identity is { IsAuthenticated: true })
        {
            var userId = (await UserManager.GetUserAsync(user)).Id;
            await UserService.AddGameToUser(userId, id);
        }
        
    }
}