using LinkGeek.AppIdentity;
using LinkGeek.Data;
using LinkGeek.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LinkGeek.Areas.Discoverability.Pages;

[Authorize]
public class DiscoverabilityModel : PageModel
{
    private readonly ILogger<DiscoverabilityModel> _logger;
    public List<ApplicationUser> UserCards = new();

    public DiscoverabilityModel(ILogger<DiscoverabilityModel> logger)
    {
        _logger = logger;
    }

    public async Task<IActionResult> OnPostAddToCollectionAsync(UserCard userCard)
    {
        return Content("");
    }

    public async Task<IActionResult> OnGetUserCardsAsync()
    {
        this.UserCards = GetUserCards();
        return Partial("_UserCardsPartial", new UserCardsModel(this.UserCards));
    }

    private List<ApplicationUser> GetUserCards()
    {
        using(var context = new ApplicationDbContext())
        {
            var users = context.Users.ToList();

            if (users.Count < 5)
            {
                var a = new List<ApplicationUser>();

                for (var i = 0;i < 5; i++)
                {
                    a.Add(users[i % users.Count]);
                }

                return a;
            }
            
            return users;
        }
    }
}