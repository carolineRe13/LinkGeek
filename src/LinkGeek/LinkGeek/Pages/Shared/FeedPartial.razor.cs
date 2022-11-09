using LinkGeek.AppIdentity;
using LinkGeek.Models;
using LinkGeek.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace LinkGeek.Pages.Shared;


[Authorize]
public partial class FeedPartial
{
    [Parameter]
    public List<Post>? posts { get; set; }
}

