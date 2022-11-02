using LinkGeek.AppIdentity;
using LinkGeek.Models;
using LinkGeek.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LinkGeek.Areas.Discoverability.Pages;

[Authorize]
public class DiscoverabilityModel : PageModel
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<DiscoverabilityModel> _logger;
    public List<ApplicationUser> UserCards = new();
    private readonly DiscoverUserService _service;

    public DiscoverabilityModel(ILogger<DiscoverabilityModel> logger, UserManager<ApplicationUser> userManager, DiscoverUserService service)
    {
        _userManager = userManager;
        _logger = logger;
        _service = service;
    }

    public async Task<IActionResult> OnPostAddToCollectionAsync(UserCard userCard)
    {
        return Content("");
    }

    /// <summary>
    /// Method <c>OnGetUserCardsAsync</c> Returns the partial user card
    /// </summary>
    public async Task<IActionResult> OnGetUserCardsAsync()
    {
        this.UserCards = GetUserCards();
        return Partial("_UserCardsPartial", new UserCardsModel(this.UserCards));
    }

    private List<ApplicationUser> GetUserCards()
    {
        var currentUser = _userManager.GetUserAsync(User).Result;
        return _service.GetUsers(currentUser).Result;
    }
}