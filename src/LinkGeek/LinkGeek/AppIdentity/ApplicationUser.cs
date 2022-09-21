using Microsoft.AspNetCore.Identity;

namespace LinkGeek;

public class ApplicationUser : IdentityUser
{
    [PersonalData]   
    public string FirstName { get; set; }
 
    [PersonalData]  
    public string LastName { get; set; }
    
    [PersonalData]
    public string? SteamAccount { get; set; }
}