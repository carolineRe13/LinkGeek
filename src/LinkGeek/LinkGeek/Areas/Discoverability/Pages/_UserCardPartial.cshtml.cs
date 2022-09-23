using LinkGeek.AppIdentity;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LinkGeek.Areas.Discoverability.Pages
{
    public class UserCardModel : PageModel
    {
        public readonly int Index;
        public readonly UserCard UserCard;

        public UserCardModel(int index, UserCard userCard)
        {
            this.Index = index;
            this.UserCard = userCard;
        }
    }
}