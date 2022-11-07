using System.Text.Json.Serialization;
using LinkGeek.AppIdentity;

namespace LinkGeek.Models;

public class Game
{
    public Game()
    {
        this.Id = new Guid().ToString();
    }
    
    public Game(string id, string name, Uri? logo)
    {
        this.Id = id;
        this.Name = name;
        this.Logo = logo;
    }

    [JsonIgnore]
    public ICollection<ApplicationUser> Players { get; set; } = new List<ApplicationUser>();

    [JsonIgnore]
    public ICollection<Post> Posts { get; set; } = new List<Post>();
    
    public string Id { get; set; }
    public string Name { get; set; }
    public Uri? Logo { get; set; }

    protected bool Equals(Game other)
    {
        return Id == other.Id && Name == other.Name && Equals(Logo, other.Logo);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Game)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id, Name, Logo);
    }
}