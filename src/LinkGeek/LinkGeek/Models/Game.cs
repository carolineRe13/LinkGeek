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
    
    public string Id { get; set; }
    public string Name { get; set; }
    public Uri? Logo { get; set; } 
}