using LinkGeek.AppIdentity;

namespace LinkGeek.Models;

public class FriendLinkOutgoing : FriendLink
{
    public FriendLinkOutgoing(string id, ApplicationUser from, ApplicationUser to) : base(id, from, to)
    {
    }

    public FriendLinkOutgoing(ApplicationUser from, ApplicationUser to) : base(from, to)
    {
    }

    public FriendLinkOutgoing() : base()
    {
    }
}