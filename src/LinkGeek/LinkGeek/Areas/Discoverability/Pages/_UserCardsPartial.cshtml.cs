using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LinkGeek.Areas.Discoverability.Pages
{
    public class UserCardsModel : PageModel
    {
        public readonly List<UserCard> UserCards;

        public UserCardsModel(List<UserCard> userCards)
        {
            this.UserCards = userCards;
        }
    }
}