using LinkGeek.AppIdentity;
using LinkGeek.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;

namespace LinkGeek.Areas.Discoverability.Pages;

[Authorize]
public partial class DiscoverabilityPage
{
    [CascadingParameter] public ApplicationUser? CurrentUser { get; set; }

    [Inject]
    private DiscoverUserService DiscoverUserService { get; set; } = null!;
    
    [Inject]
    private IJSRuntime JS { get; set; } = null!;

    private IList<ApplicationUser>? users;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender && users != null)
        {
            var a = await JS.InvokeVoidAsyncWithErrorHandling("initCarousel");
        }
    }

    public async Task OpenPack()
    {
        if (CurrentUser != null)
        {
            this.users = await this.DiscoverUserService.GetUsers(CurrentUser);
            this.StateHasChanged();
        }
    }
}