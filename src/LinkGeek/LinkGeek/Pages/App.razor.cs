using System.Net;
using LinkGeek.AppIdentity;
using LinkGeek.Services;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;
using MudBlazor;
using Cookie = System.Net.Cookie;

namespace LinkGeek.Pages;

[Authorize]
public partial class App
{
    private HubConnection? hubConnection;
    private ApplicationUser? currentUser;
    [Inject]
    UserManager<ApplicationUser> UserManager { get; set; }
    
    [Inject]
    UserService UserService { get; set; }

    [Inject] 
    private NavigationManager _navigationManager { get; set; }

    [Inject] 
    private ISnackbar _snackBar { get; set; }

    [Inject] 
    private IJSRuntime _jsRuntime { get; set; }

    [Inject] 
    private AuthenticationStateProvider AuthenticationStateProvider { get; set; }
    
    [Inject]
    private IHttpContextAccessor HttpContextAccessor { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
        var user = authState.User;
        var currentUserId = "-1";
        
        if (user.Identity is { IsAuthenticated: true })
        {
            var incompleteUser = await UserManager.GetUserAsync(user);
            currentUser = UserService.GetUserFromUserName(incompleteUser.UserName) ?? incompleteUser;
            currentUserId = currentUser.Id;
            
            hubConnection = new HubConnectionBuilder()
                .WithUrl(_navigationManager.ToAbsoluteUri("/signalRHub"), options =>
                {
                    if (HttpContextAccessor.HttpContext != null) {
                        foreach (var cookie in HttpContextAccessor.HttpContext.Request.Cookies) {
                            options.Cookies.Add(new Cookie(cookie.Key, cookie.Value, null, HttpContextAccessor.HttpContext.Request.Host.Host));
                        }
                    }
                })
                .WithAutomaticReconnect()
                .Build();
        
            hubConnection.On<int>("Connected", i => {
                StateHasChanged();
            });
            
            hubConnection.On<string, string, string>("ReceiveChatNotification", (message, receiverUserId, senderUserId) =>
            {
                if (currentUserId == receiverUserId)
                {
                    _jsRuntime.InvokeAsync<string>("PlayAudio", "notification");
                    _snackBar.Add(message, Severity.Info, config =>
                    {
                        config.VisibleStateDuration = 10000;
                        config.HideTransitionDuration = 500;
                        config.ShowTransitionDuration = 500;
                        config.Action = "Chat?";
                        config.ActionColor = Color.Info;
                        config.Onclick = snackbar =>
                        {
                            _navigationManager.NavigateTo($"chat/{senderUserId}");
                            return Task.CompletedTask;
                        };
                    });
                }
            });
            
            await hubConnection.StartAsync();
        }
    }

    // important to get feed and friends updates
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (currentUser != null)
        {
            var latestUserData = UserService.GetUserFromUserName(currentUser.UserName);

            if (!currentUser.Equals(latestUserData))
            {
                currentUser = latestUserData;
                StateHasChanged();
            }
        }
    }

    public async ValueTask DisposeAsync() {
        if (hubConnection is not null) {
            await hubConnection.DisposeAsync();
        }
    }
}