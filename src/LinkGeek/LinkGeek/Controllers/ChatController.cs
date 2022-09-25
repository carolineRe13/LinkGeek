using System.Security.Claims;
using LinkGeek.AppIdentity;
using LinkGeek.Data;
using LinkGeek.Services;
using LinkGeek.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LinkGeek.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ChatController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ChatService _chatService;
    private readonly ApplicationDbContext _context;
    public ChatController(UserManager<ApplicationUser> userManager, ApplicationDbContext context, ChatService chatService)
    {
        _userManager = userManager;
        _context = context;
        _chatService = chatService;
    }
    
    [HttpGet("users/{userId}")]
    public async Task<IActionResult> GetUserDetailsAsync(string userId)
    {
        var user = _chatService.GetUserDetailsAsync(userId);
        return Ok(user);
    }
    
    [HttpGet("users")]
    public async Task<IActionResult> GetUsersAsync()
    {
        var userId = User.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier).Select(a => a.Value).FirstOrDefault();
        var allUsers = _chatService.GetUsersAsync(userId);
        return Ok(allUsers);
    }
    
    [HttpPost]
    public async Task<IActionResult> SaveMessageAsync(ChatMessage message)
    {
        var userId = User.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier).Select(a => a.Value).FirstOrDefault();

        return Ok(_chatService.SaveMessageAsync(message, userId));
    }
    
    [HttpGet("{contactId}")]
    public async Task<IActionResult> GetConversationAsync(string contactId)
    {
        var userId = User.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier).Select(a => a.Value).FirstOrDefault();

        var messages = _chatService.GetConversationAsync(userId, contactId);
        
        return Ok(messages);
    }
}



