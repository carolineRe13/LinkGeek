using LinkGeek.AppIdentity;
using LinkGeek.Data;
using LinkGeek.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LinkGeek.Controllers;

[Authorize]
[Route("user/")]
public class UserController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<UserController> _logger;

    public UserController(UserManager<ApplicationUser> userManager, ILogger<UserController> logger)
    {
        _userManager = userManager;
        _logger = logger;
    }

    [HttpPost("game")]
    public async Task<IActionResult> AddGameToUser(string id)
    {
        var sessionUser = await _userManager.GetUserAsync(User);
        var userId = sessionUser.Id;
        
        using (var context = new ApplicationDbContext())
        {
            var existingGame = await context.Game.FindAsync(id);

            if (existingGame == null)
            {
                return BadRequest();
            }

            var applicationUser = context.Users.Include(u => u.Games).FirstOrDefaultAsync(u => u.Id == userId).Result;
            if (applicationUser != null)
            {
                applicationUser.Games
                    .Add(existingGame);
            }

            var changes = await context.SaveChangesAsync();
            // _logger.LogInformation("{Changes}", changes);
            return Ok(existingGame);
        }
    }
}