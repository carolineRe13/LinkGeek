﻿using LinkGeek.AppIdentity;
using LinkGeek.Models;
using LinkGeek.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace LinkGeek.Pages.Shared;


[Authorize]
public partial class FeedPartial
{
    [Inject] 
    public UserService UserService { get; set; }
    
    [CascadingParameter] public ApplicationUser? currentUser { get; set; }
    
    private List<Post> posts = new();

    protected override void OnParametersSet()
    {
        if (currentUser != null && UserService != null)
        {
            posts = this.UserService.GetUserFeed(currentUser);
        }
    }
}
