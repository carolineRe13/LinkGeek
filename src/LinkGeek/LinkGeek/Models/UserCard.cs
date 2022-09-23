using LinkGeek.Models;

namespace LinkGeek.AppIdentity;

public class UserCard
{
    public string Login { get; init; }
    public string? ProfilePicture { get; init; }
    public List<Game> Games { get; init; } = new ();
}