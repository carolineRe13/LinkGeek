using System.ComponentModel.DataAnnotations;
using LinkGeek.AppIdentity;

namespace LinkGeek.Models;
public abstract class FriendLink
{
    [Key] public string Id { get; set; } = new Guid().ToString();
    public ApplicationUser From { get; set; }
    public ApplicationUser To { get; set; }

    public FriendLink(string id, ApplicationUser from, ApplicationUser to)
    {
        Id = id;
        From = from;
        To = to;
    }

    public FriendLink(ApplicationUser from, ApplicationUser to)
    {
        From = from;
        To = to;
    }

    public FriendLink()
    {
    }
}