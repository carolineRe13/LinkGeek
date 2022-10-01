using LinkGeek.AppIdentity;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LinkGeek.Areas.Discoverability.Pages
{
    public class UserCardsModel : PageModel
    {
        public readonly List<ApplicationUser> Users;

        public UserCardsModel(List<ApplicationUser> users)
        {
            this.Users = users;
        }
    }
}