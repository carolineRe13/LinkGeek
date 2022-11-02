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

/// <summary>
/// Class <c>Games</c> Overall games tab containing all game pages and a search functionality
/// </summary>
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
    
    protected override async Task OnInitializedAsync()
    {
        results = await GameDbService.GetPopularGames();
    }
    
    public async Task Search()
    {
        results = await GameDbService.SearchGames(search.Search);
        
    }
}