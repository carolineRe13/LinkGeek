using LinkGeek.AppIdentity;

namespace LinkGeek.Models;

public class Post
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public ApplicationUser ApplicationUser { get; init; }
    public Game? Game { get; init; }
    public string Content { get; init; }
    public DateTimeOffset CreatedAt { get; init; } = DateTimeOffset.Now;

    public ICollection<ApplicationUser> Likes { get; set; } = new List<ApplicationUser>() { };
    
    public DateTimeOffset? PlayingAt { get; init; } = DateTimeOffset.Now;
    
    public PlayerRoles? LookingFor { get; init; }
    
    public ICollection<Comment> Comments { get; init; } = new List<Comment>();
}