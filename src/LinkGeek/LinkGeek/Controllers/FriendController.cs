using LinkGeek.AppIdentity;
using LinkGeek.Data;
using LinkGeek.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LinkGeek.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class FriendController: ControllerBase
{
    
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly FriendService _friendService;
    private readonly ApplicationDbContext _context;
    
    public FriendController(UserManager<ApplicationUser> userManager, ApplicationDbContext context, FriendService friendService)
    {
        _userManager = userManager;
        _context = context;
        _friendService = friendService;
    }

    [HttpPost]
    public async Task<IActionResult> AddFriend(string userToAddId)
    {
        var userId = User.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier).Select(a => a.Value).FirstOrDefault();

        return Ok(_friendService.AddFriend(userId, userToAddId));
    }

    [HttpPost]
    public async Task<IActionResult> CancelFriendRequest(string userToAddId)
    {
        var userId = User.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier).Select(a => a.Value).FirstOrDefault();

        return Ok(_friendService.CancelFriendRequest(userId, userToAddId));
    }

    [HttpPost]
    public async Task<IActionResult> RemoveFriend(string userToAddId)
    {
        var userId = User.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier).Select(a => a.Value).FirstOrDefault();

        return Ok(_friendService.RemoveFriend(userId, userToAddId));
    }
}