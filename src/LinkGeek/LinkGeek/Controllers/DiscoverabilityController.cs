using LinkGeek.Data;
using LinkGeek.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LinkGeek.Controllers;

[Authorize]
public class DiscoverabilityController : Controller
{
    [HttpGet("discoverability/game/{id}")]
    public IActionResult GetGame(string id)
    {
        using (var context = new ApplicationDbContext())
        {
            var game = context.Game.Find(id);
            if (game == null)
            {
                return BadRequest();
            }
            return Ok(game);
        }
    }
    
    
    [HttpPost("discoverability/game")]
    public IActionResult PostGame(Game game)
    {
        using (var context = new ApplicationDbContext())
        {
            game.Id = Guid.NewGuid().ToString();
            context.Game.Add(game);
            context.SaveChanges();
            return Ok(game);
        }
    }
}