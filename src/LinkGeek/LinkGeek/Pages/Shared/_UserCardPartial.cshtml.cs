using LinkGeek.AppIdentity;
using LinkGeek.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LinkGeek.Pages.Shared;

public class UserCardModel : PageModel
{
    public readonly ApplicationUser ApplicationUser;
    public readonly bool IsCurrentUser;
    public readonly ApplicationUser OtherUser;

    public UserCardModel(ApplicationUser applicationUser, bool isCurrentUser, ApplicationUser otherUser)
    {
        IsCurrentUser = isCurrentUser;
        
        ApplicationUser = applicationUser;
        OtherUser = otherUser;
        
        if (applicationUser.Games == null 
            || applicationUser.Games.Count == 0)
        {
            using (var context = new ApplicationDbContext())
            {
                applicationUser.Games = context.Game.Where(g => g.Players.Contains(applicationUser)).ToList();
            }
        }
    }

    public bool AreCurrentlyFriends()
    {
        if (ApplicationUser.Friends.Contains(OtherUser))
        {
            return true;
        }

        return false;
    }
    
    public bool AreCurrentlyPendingFriends()
    {
        if (ApplicationUser.SentFriendRequests.Contains(OtherUser))
        {
            return true;
        }

        return false;
    }
    
    public bool IsAdded()
    {
        if (ApplicationUser.ReceivedFriendRequests.Contains(OtherUser))
        {
            return true;
        }

        return false;
    }

    public string GetImageSrc()
    {
        return "data:" + ApplicationUser.ProfilePictureContentType + ";base64," + ApplicationUser.ProfilePictureData;
    }
        
}