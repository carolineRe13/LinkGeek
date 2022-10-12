using System.Text.Json.Serialization;
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
    public string? Gender { get; set; }
    
    [PersonalData]
    public string? SteamAccount { get; set; }

    public string? ProfilePictureContentType { get; set; }
    public string? ProfilePictureData { get; set; }

    public ICollection<Game>? Games { get; set; }
    
    public virtual ICollection<FriendLinkFriend>? Friends { get; set; }
    public virtual ICollection<FriendLinkIncoming>? PendingIncomingFriendsRequests { get; set; }
    public virtual ICollection<FriendLinkOutgoing>? PendingOutgoingFriendsRequests { get; set; }
    
    [JsonIgnore]
    public virtual ICollection<ChatMessage> ChatMessagesFromUsers { get; set; }
    
    [JsonIgnore]
    public virtual ICollection<ChatMessage> ChatMessagesToUsers { get; set; }
    
    public ApplicationUser()
    {
        ChatMessagesFromUsers = new HashSet<ChatMessage>();
        ChatMessagesToUsers = new HashSet<ChatMessage>();
    }

    public string? Status { get; set; }
    
    public string? TimeZone { get; set; }
    
    public string? Location { get; set; }
    
    public ICollection<ApplicationUser> GetPendingIncomingFriendList()
    {
        return (PendingIncomingFriendsRequests ?? new List<FriendLinkIncoming>()).Select(f => f.From).ToList();
    }
    
    public ICollection<ApplicationUser> GetPendingOutgoingFriendList()
    {
        return (PendingOutgoingFriendsRequests ?? new List<FriendLinkOutgoing>()).Select(f => f.To).ToList();
    }
    
    public ICollection<ApplicationUser> GetFriendList()
    {
        return (Friends ?? new List<FriendLinkFriend>()).Select(f => f.To).ToList();
    }
}