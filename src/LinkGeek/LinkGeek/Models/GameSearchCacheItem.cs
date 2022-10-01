namespace LinkGeek.Models;

public class GameSearchCacheItem
{
    public string Query { get; init; } = string.Empty;
    public Game Game {get; init;} = new Game();
    public int Rank {get; init;} = 0;
    public DateTimeOffset LastUpdated { get; init; } = DateTimeOffset.MinValue;
}