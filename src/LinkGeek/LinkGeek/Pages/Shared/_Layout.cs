using LinkGeek.AppIdentity;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;

namespace LinkGeek.Pages.Shared;

public class _Layout : PageModel
{
    private readonly SnackbarService _snackBar;
    private HubConnection hubConnection;

    public _Layout(SnackbarService snackBar)
    {
        _snackBar = snackBar;
    }

    public bool IsConnected => hubConnection.State == HubConnectionState.Connected;
    
    protected async Task OnInitializedAsync()
    {
        
        hubConnection = new HubConnectionBuilder().WithUrl("/signalRHub").Build();
        await hubConnection.StartAsync();
        hubConnection.On<string, string, string>("ReceiveChatNotification", (message, receiverUserId, senderUserId) =>
        {
            if (User.Identity.Name == receiverUserId)
            {
                _snackBar.Add(message, Severity.Info, config =>
                {
                    config.VisibleStateDuration = 10000;
                    config.HideTransitionDuration = 500;
                    config.ShowTransitionDuration = 500;
                    config.Action = "Chat?";
                    config.ActionColor = Color.Info;
                    config.Onclick = snackbar =>
                    {
                        return Task.CompletedTask;
                    };
                });
            }
        });
    }
}