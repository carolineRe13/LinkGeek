using LinkGeek.Data;
using LinkGeek.Models;
using LinkGeek.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LinkGeek.Controllers;

[Authorize]
public class DiscoverabilityController : Controller
{
    private readonly GameDbService _gameDbService;

    public DiscoverabilityController(GameDbService gameDbService)
    {
        _gameDbService = gameDbService;
    }

    [HttpPost("discoverability/game/{search}")]
    public async Task<IActionResult> SearchGamesAsync(string search, int page = 0)
    {
        var games = await _gameDbService.SearchGames(search, page);
        return Ok(games);
    }
}