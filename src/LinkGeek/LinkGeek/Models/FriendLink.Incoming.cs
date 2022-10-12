using LinkGeek.AppIdentity;

namespace LinkGeek.Models;

public class FriendLinkIncoming : FriendLink
{
    public FriendLinkIncoming(string id, ApplicationUser from, ApplicationUser to) : base(id, from, to)
    {
    }

    public FriendLinkIncoming(ApplicationUser from, ApplicationUser to) : base(from, to)
    {
    }

    public FriendLinkIncoming() : base()
    {
    }
}