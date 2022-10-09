using System.ComponentModel.DataAnnotations;
using LinkGeek.AppIdentity;

namespace LinkGeek.Models;
public abstract class FriendLink
{
    [Key] public string Id { get; set; } = new Guid().ToString();
    public ApplicationUser User1 { get; set; }
    public ApplicationUser User2 { get; set; }
}