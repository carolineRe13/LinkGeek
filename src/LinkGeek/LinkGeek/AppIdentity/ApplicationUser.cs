using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace LinkGeek.AppIdentity;

public class ApplicationUser : IdentityUser
{
    public ApplicationUser(string firstName, string lastName, string gender, string? steamAccount, byte[]? profilePicture)
    {
        FirstName = firstName;
        LastName = lastName;
        Gender = gender;
        SteamAccount = steamAccount;
        ProfilePicture = profilePicture;
    }

    public ApplicationUser(string userName, string firstName, string lastName, string gender, string? steamAccount, byte[]? profilePicture) : base(userName)
    {
        FirstName = firstName;
        LastName = lastName;
        Gender = gender;
        SteamAccount = steamAccount;
        ProfilePicture = profilePicture;
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