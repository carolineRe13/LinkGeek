using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LinkGeek.Areas.ProfileCard.Pages;

[Authorize]
public class mainModel : PageModel
{
    public void OnGet()
    {
        
    }
}