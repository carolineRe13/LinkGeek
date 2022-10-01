using LinkGeek.AppIdentity;
using LinkGeek.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LinkGeek.Controllers;

[Authorize]
[Route("user/")]
public class UserController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly UserService _userService;

    public UserController(UserManager<ApplicationUser> userManager, UserService userService)
    {
        _userManager = userManager;
        _userService = userService;
    }

    [HttpPost("game")]
    public async Task<IActionResult> AddGameToUser(string gameId)
    {
        var sessionUser = await _userManager.GetUserAsync(User);
        var userId = sessionUser.Id;

        var result = await _userService.AddGameToUser(userId, gameId);
        if (result == null)
        {
            return BadRequest();
        }

        return Ok(result);
    }

    [HttpPost("location")]
    public async Task<IActionResult> UpdateLocation(string location)
    {
        var sessionUser = await _userManager.GetUserAsync(User);
        var userId = sessionUser.Id;

        var result = await _userService.UpdateLocation(userId, location);
        if (result == null)
        {
            return BadRequest();
        }

        return Ok(result);
    }

    [HttpPost]
    [Route("status")]
    public async Task<IActionResult> UpdateStatus(string status)
    {
        var sessionUser = await _userManager.GetUserAsync(User);
        var userId = sessionUser.Id;

        var result = await _userService.UpdateStatus(userId, status);
        if (result == null)
        {
            return BadRequest();
        }

        return Ok(result);
    }
}