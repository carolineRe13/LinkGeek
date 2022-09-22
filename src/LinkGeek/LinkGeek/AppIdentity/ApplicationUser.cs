using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace LinkGeek;

public class ApplicationUser : IdentityUser
{
    public async Task<IdentityResult> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
    {
        var userIdentity = await manager.CreateAsync(this);
        // Add custom user claims here
        return userIdentity;
    }
    
    [PersonalData]   
    public string FirstName { get; set; }
 
    [PersonalData]  
    public string LastName { get; set; }

    [PersonalData]  
    public string Gender { get; set; }
    
    [PersonalData]
    public string? SteamAccount { get; set; }

    public byte[]? ProfilePicture { get; set; }
}