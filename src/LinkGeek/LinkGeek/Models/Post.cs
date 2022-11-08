using System.Text.Json.Serialization;
using LinkGeek.AppIdentity;
using Newtonsoft.Json.Converters;

namespace LinkGeek.Models;

public class Post
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public ApplicationUser ApplicationUser { get; init; }
    public Game? Game { get; init; }
    public string Content { get; init; }
    public DateTimeOffset CreatedAt { get; init; } = DateTimeOffset.Now;
    
    public DateTimeOffset? PlayingAt { get; init; } = DateTimeOffset.Now;
    
    public PlayerRoles? LookingFor { get; init; }
}