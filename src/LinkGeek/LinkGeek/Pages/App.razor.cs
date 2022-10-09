using LinkGeek.AppIdentity;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;
using MudBlazor;

namespace LinkGeek.Pages;

[Authorize]
public partial class App
{
    private HubConnection hubConnection;
    public bool IsConnected => hubConnection.State == HubConnectionState.Connected;
    [Inject]
    UserManager<ApplicationUser> UserManager { get; set; }

    [Inject] 
    private NavigationManager _navigationManager { get; set; }

    [Inject] 
    private ISnackbar _snackBar { get; set; }

    [Inject] 
    private IJSRuntime _jsRuntime { get; set; }

    [Inject] 
    private AuthenticationStateProvider AuthenticationStateProvider { get; set; }
    
    protected override async Task OnInitializedAsync()
    {
        var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
        var user = authState.User;
        var CurrentUserId = "-1";
        
        if (user.Identity.IsAuthenticated)
        {
            var currentUser = await UserManager.GetUserAsync(user);
            CurrentUserId = currentUser.Id;
        }
        
        hubConnection = new HubConnectionBuilder().WithUrl(_navigationManager.ToAbsoluteUri("/signalRHub")).Build();
        await hubConnection.StartAsync();
        hubConnection.On<string, string, string>("ReceiveChatNotification", (message, receiverUserId, senderUserId) =>
        {
            if (CurrentUserId == receiverUserId)
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
    }
}