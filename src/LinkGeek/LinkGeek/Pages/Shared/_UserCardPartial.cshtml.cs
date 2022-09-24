using LinkGeek.AppIdentity;
using LinkGeek.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LinkGeek.Pages.Shared;

public class UserCardModel : PageModel
{
    public readonly ApplicationUser ApplicationUser;
    public readonly bool IsCurrentUser;

    public UserCardModel(ApplicationUser applicationUser, bool isCurrentUser)
    {
        IsCurrentUser = isCurrentUser;

        this.ApplicationUser = applicationUser;
        if (applicationUser.Games == null 
            || applicationUser.Games.Count == 0)
        {
            using (var context = new ApplicationDbContext())
            {
                applicationUser.Games = context.Game.Where(g => g.Players.Contains(applicationUser)).ToList();
            }
        }
    }
}