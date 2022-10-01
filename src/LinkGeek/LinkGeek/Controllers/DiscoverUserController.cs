using LinkGeek.AppIdentity;
using LinkGeek.Services;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LinkGeek.Controllers;

[Authorize]
[Route("userDiscovery/")]
public class DiscoverUserController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly DiscoverUserService _userService;
    
    public DiscoverUserController(UserManager<ApplicationUser> userManager, DiscoverUserService userService)
    {
        _userManager = userManager;
        _userService = userService;
    }
    
    [HttpPost("users")]
    public async Task<List<ApplicationUser>> GetUsers()
    {
        var sessionUser = await _userManager.GetUserAsync(User);

        List<ApplicationUser> userList = await _userService.GetUsers(sessionUser);
        return userList;
    }
    
}