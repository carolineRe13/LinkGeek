using System.ComponentModel;
using LinkGeek.AppIdentity;
using LinkGeek.Models;
using LinkGeek.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;

namespace LinkGeek.Pages.Shared;


[Authorize]
public partial class CommentPartial
{
    [Inject] public UserService UserService { get; set; }
    [Parameter] public Action<Post> ParentRefreshFunction { get; set; }
    [Parameter] public Post Post { get; set; }
    [CascadingParameter] public ApplicationUser? currentUser { get; set; }
    
    private string pendingComment = String.Empty;
    
    private async Task PostComment()
    {
        if (currentUser == null) return;
        
        var updatedPost = await this.UserService.PostCommentAsync(currentUser, Post, pendingComment);
        if (updatedPost == null) return;
        
        this.Post = updatedPost;
        
        pendingComment = String.Empty; 
        ParentRefreshFunction.Invoke(Post);
    }
}