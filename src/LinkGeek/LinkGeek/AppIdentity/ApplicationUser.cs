using LinkGeek.Models;
using Microsoft.AspNetCore.Identity;

namespace LinkGeek.AppIdentity;

public class ApplicationUser : IdentityUser
{
    [PersonalData]   
    public string FirstName { get; set; }
 
    [PersonalData]  
    public string LastName { get; set; }

    [PersonalData]  
    public string Gender { get; set; }
    
    [PersonalData]
    public string? SteamAccount { get; set; }

    public byte[]? ProfilePicture { get; set; }

    public ICollection<Game>? Games { get; set; }
}