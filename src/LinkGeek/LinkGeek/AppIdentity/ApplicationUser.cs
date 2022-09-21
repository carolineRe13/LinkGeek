using Microsoft.AspNetCore.Identity;

namespace LinkGeek;

public class ApplicationUser : IdentityUser
{
    public string? SteamAccount { get; set; }
}