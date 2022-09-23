using LinkGeek.AppIdentity;
using LinkGeek.Data;
using LinkGeek.Models;
using Microsoft.AspNetCore.Mvc;

namespace LinkGeek.Controllers;

public class DiscoverabilityController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
    
    [HttpGet("discoverability/games/{id}")]
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
}