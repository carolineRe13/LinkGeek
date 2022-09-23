namespace LinkGeek.Models;

public class Game
{
    public Game(string id, string name, Uri logo)
    {
        this.Id = id;
        this.Name = name;
        this.Logo = logo;
    }

    public string Id { get; init; }
    public string Name { get; init; }
    public Uri Logo { get; init; } 
}