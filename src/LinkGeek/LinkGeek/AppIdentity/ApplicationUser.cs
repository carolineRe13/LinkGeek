using System.Text.Json.Serialization;
using LinkGeek.Models;
using LinkGeek.Shared;
using Microsoft.AspNetCore.Identity;

namespace LinkGeek.AppIdentity;

public class ApplicationUser : IdentityUser
{
    [PersonalData]   
    public string FirstName { get; set; } = string.Empty;
 
    [PersonalData]  
    public string LastName { get; set; } = string.Empty;

    [PersonalData]  
    public string? Gender { get; set; }
    
    [PersonalData]
    public string? SteamAccount { get; set; }

    public string? ProfilePictureContentType { get; set; }
    public string? ProfilePictureData { get; set; }

    public ICollection<Game>? Games { get; set; }

    public virtual ICollection<ApplicationUser> Friends { get; set; } = new List<ApplicationUser>();
    public virtual ICollection<ApplicationUser> MyFriends { get; set; } = new List<ApplicationUser>();
    
    public virtual ICollection<ApplicationUser> SentFriendRequests { get; set; } = new List<ApplicationUser>();
    public virtual ICollection<ApplicationUser> ReceivedFriendRequests { get; set; } = new List<ApplicationUser>();

    [JsonIgnore]
    public virtual ICollection<ChatMessage> ChatMessagesFromUsers { get; set; }
    
    [JsonIgnore]
    public virtual ICollection<ChatMessage> ChatMessagesToUsers { get; set; }
    
    public virtual ICollection<Post> Posts { get; set; }

    public ApplicationUser()
    {
        ChatMessagesFromUsers = new HashSet<ChatMessage>();
        ChatMessagesToUsers = new HashSet<ChatMessage>();
    }

    public string? Status { get; set; }
    
    public string? TimeZone { get; set; }
    
    public string? Location { get; set; }
}