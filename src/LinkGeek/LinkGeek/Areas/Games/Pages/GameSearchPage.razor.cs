using System.ComponentModel.DataAnnotations;
using LinkGeek.Models;
using LinkGeek.Services;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

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
public partial class GameSearchPage
{
    [Inject] 
    private GameDbService GameDbService { get; set; }
    
    [Inject]
    private AuthenticationStateProvider AuthenticationStateProvider { get; set; }
    
    public SearchModel search = new();
    public ICollection<Game>? results { get; set; }
    
    protected override async Task OnInitializedAsync()
    {
        results = await GameDbService.GetPopularGames();
    }
    
    public async Task Search()
    {
        results = await GameDbService.SearchGames(search.Search);
    }
}