namespace LinkGeek.Models;

public class UserCard
{
    public ICollection<Game> Games { get; set; }
    
    public Guid id { get; set; }
    public string Login { get; init; }
    public string? ProfilePicture { get; init; }
}