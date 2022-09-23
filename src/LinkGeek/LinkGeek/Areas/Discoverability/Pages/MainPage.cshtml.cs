using LinkGeek.AppIdentity;
using LinkGeek.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LinkGeek.Areas.Discoverability.Pages;

public class DiscoverabilityModel : PageModel
{
    private readonly ILogger<DiscoverabilityModel> _logger;
    public List<UserCard> UserCards = new();

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

    private List<UserCard> GetUserCards()
    {
        return new()
        {
            new ()
            {
                Login = "User1",
                ProfilePicture = Constants.CorgyPP,
                Games = new List<Game>
                {
                    new("1", "Diablo II: Resurrected", new Uri(Constants.Diablo2RLogo)),
                    new("1", "Diablo II: Resurrected", new Uri(Constants.Diablo2RLogo)),
                    new("1", "Diablo II: Resurrected", new Uri(Constants.Diablo2RLogo)),
                    new("1", "Diablo II: Resurrected", new Uri(Constants.Diablo2RLogo)),
                    new("1", "Diablo II: Resurrected", new Uri(Constants.Diablo2RLogo)),
                    new("1", "Diablo II: Resurrected", new Uri(Constants.Diablo2RLogo)),
                    new("2", "Overwatch", new Uri(Constants.OverwatchLogo))
                }
            }, 
            new ()
            {
                Login = "User2",
                ProfilePicture = Constants.ShibaPP,
                Games = new List<Game> {
                    new("1", "Diablo II: Resurrected", new Uri(Constants.Diablo2RLogo)),
                    new("2", "Overwatch 2", new Uri(Constants.Overwatch2Logo))
                }

            }, 
            new ()
            {
                Login = "User3",
                ProfilePicture = Constants.ShibaPP,
                Games = new List<Game> {
                    new("1", "Diablo II: Resurrected", new Uri(Constants.Diablo2RLogo)),
                    new("2", "Overwatch 2", new Uri(Constants.Overwatch2Logo))
                }

            }, 
            new ()
            {
                Login = "User4",
                ProfilePicture = Constants.ShibaPP,
                Games = new List<Game> {
                    new("1", "Diablo II: Resurrected", new Uri(Constants.Diablo2RLogo)),
                    new("2", "Overwatch", new Uri(Constants.OverwatchLogo))
                }

            }, 
            new ()
            {
                Login = "User5",
                ProfilePicture = Constants.ShibaPP,
                Games = new List<Game> {
                    new("1", "Diablo II: Resurrected", new Uri(Constants.Diablo2RLogo)),
                    new("2", "Overwatch 2", new Uri(Constants.Overwatch2Logo))
                }

            }
        };
    }
}