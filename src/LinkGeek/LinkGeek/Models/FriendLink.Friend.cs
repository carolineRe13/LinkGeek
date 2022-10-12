using Humanizer;
using LinkGeek.AppIdentity;

namespace LinkGeek.Models;

public class FriendLinkFriend : FriendLink
{
    public FriendLinkFriend(string id, ApplicationUser from, ApplicationUser to) : base(id, from, to)
    {
    }

    public FriendLinkFriend(ApplicationUser from, ApplicationUser to) : base(from, to)
    {
    }

    public FriendLinkFriend() : base()
    {
    }
}