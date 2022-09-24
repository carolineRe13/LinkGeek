using LinkGeek.AppIdentity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LinkGeek.Areas.ProfileCard.Pages;

[Authorize]
public class ProfilePageModel : PageModel
{
    private readonly UserManager<ApplicationUser> _userManager;
    public ApplicationUser? ApplicationUser;
    
    public ProfilePageModel(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public void OnGet()
    {
        ApplicationUser = _userManager.GetUserAsync(User).Result;
    }
}