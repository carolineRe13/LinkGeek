using LinkGeek.Models;
using LinkGeek.Shared;
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
    public ICollection<ApplicationUser>? Friends { get; set; }
    public ICollection<ApplicationUser>? PendingIncomingFriendsRequests { get; set; }
    public ICollection<ApplicationUser>? PendingOutgoingFriendsRequests { get; set; }
    
    public virtual ICollection<ChatMessage> ChatMessagesFromUsers { get; set; }
    
    public virtual ICollection<ChatMessage> ChatMessagesToUsers { get; set; }
    
    public ApplicationUser()
    {
        ChatMessagesFromUsers = new HashSet<ChatMessage>();
        ChatMessagesToUsers = new HashSet<ChatMessage>();
    }

    public string? Status { get; set; }
    
    public string? TimeZone { get; set; }
    
    public string? Location { get; set; }
}