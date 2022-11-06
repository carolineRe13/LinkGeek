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

    protected bool Equals(ApplicationUser other)
    {
        return FirstName == other.FirstName 
               && LastName == other.LastName 
               && Gender == other.Gender 
               && SteamAccount == other.SteamAccount 
               && ProfilePictureContentType == other.ProfilePictureContentType 
               && ProfilePictureData == other.ProfilePictureData 
               && (Games ?? new List<Game>()).SequenceEqual(other.Games ?? new List<Game>())
               && Friends.SequenceEqual(other.Friends) 
               && SentFriendRequests.SequenceEqual(other.SentFriendRequests) 
               && ReceivedFriendRequests.SequenceEqual(other.ReceivedFriendRequests) 
               && Posts.SequenceEqual(other.Posts) 
               && Status == other.Status 
               && TimeZone == other.TimeZone 
               && Location == other.Location;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((ApplicationUser)obj);
    }

    public override int GetHashCode()
    {
        var hashCode = new HashCode();
        hashCode.Add(FirstName);
        hashCode.Add(LastName);
        hashCode.Add(Gender);
        hashCode.Add(SteamAccount);
        hashCode.Add(ProfilePictureContentType);
        hashCode.Add(ProfilePictureData);
        hashCode.Add(Games);
        hashCode.Add(Friends);
        hashCode.Add(SentFriendRequests);
        hashCode.Add(ReceivedFriendRequests);
        hashCode.Add(Posts);
        hashCode.Add(Status);
        hashCode.Add(TimeZone);
        hashCode.Add(Location);
        return hashCode.ToHashCode();
    }
}