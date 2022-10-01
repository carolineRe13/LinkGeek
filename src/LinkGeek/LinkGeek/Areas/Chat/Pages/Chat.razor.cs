using System.Security.Claims;
using LinkGeek.AppIdentity;
using LinkGeek.Services;
using LinkGeek.Shared;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR.Client;

namespace LinkGeek.Areas.Chat.Pages;

[Authorize]
public partial class Chat
{
    [Inject]
    UserManager<ApplicationUser> UserManager { get; set; }
    [CascadingParameter] public HubConnection HubConnection { get; set; }
    [Parameter] public string CurrentMessage { get; set; }
    [Parameter] public string CurrentUserId { get; set; }
    [Parameter] public string CurrentUserEmail { get; set; }    
    [Parameter] public string ContactEmail { get; set; }
    [Parameter] public string ContactId { get; set; }
    public List<ApplicationUser> ChatUsers;
    private List<ChatMessage> _messages;

    string currentUserId;

    private async Task SubmitAsync()
    {
        if (!string.IsNullOrEmpty(CurrentMessage) && !string.IsNullOrEmpty(ContactId))
        {
            var chatHistory = new ChatMessage()
            {
                Message = CurrentMessage,
                ToUserId = ContactId,
                CreatedDate = DateTime.Now
            };
            await _chatManager.SaveMessageAsync(chatHistory, currentUserId);
            chatHistory.FromUserId = CurrentUserId;
            await HubConnection.SendAsync("SendMessageAsync", chatHistory, CurrentUserEmail);
            CurrentMessage = string.Empty;
        }
    }
    protected override async Task OnInitializedAsync()
    {
        var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
        var user = authState.User;
        // var user = (await authenticationStateTask).User;
        
        if (user.Identity.IsAuthenticated)
        {
            var currentUser = await UserManager.GetUserAsync(user);
            currentUserId = currentUser.Id;
            var currentUserEmail = currentUser.Email;
            var currentUserPhone = currentUser.PhoneNumber;
            var currentUserEmailConfirmed = currentUser.EmailConfirmed;
        }
        
        if (HubConnection == null)
        {
            HubConnection = new HubConnectionBuilder().WithUrl(NavigationManager.ToAbsoluteUri("/signalRHub")).Build();
        }
        if (HubConnection.State == HubConnectionState.Disconnected)
        {
            await HubConnection.StartAsync();
        }
        HubConnection.On<ChatMessage, string>("ReceiveMessage", async (message, userName) =>
        {
            if ((ContactId == message.ToUserId && CurrentUserId == message.FromUserId) || (ContactId == message.FromUserId && CurrentUserId == message.ToUserId))
            {
                   
                if ((ContactId == message.ToUserId && CurrentUserId == message.FromUserId))
                {
                    _messages.Add(new ChatMessage { Message = message.Message, CreatedDate = message.CreatedDate, FromUser = new ApplicationUser() { Email = CurrentUserEmail } } );
                    await HubConnection.SendAsync("ChatNotificationAsync", $"New Message From {userName}", ContactId, CurrentUserId);
                }
                else if ((ContactId == message.FromUserId && CurrentUserId == message.ToUserId))
                {
                    _messages.Add(new ChatMessage { Message = message.Message, CreatedDate = message.CreatedDate, FromUser = new ApplicationUser() { Email = ContactEmail } });
                }
            }
        });
        await GetUsersAsync();
        if (!string.IsNullOrEmpty(ContactId))
        {
            await LoadUserChat(ContactId);
        }
    }
    public async Task LoadUserChat(string contactId)
    {
        var contact = await _chatManager.GetUserDetailsAsync(contactId);
        ContactId = contact.Id;
        ContactEmail = contact.Email;
        _messages = new List<ChatMessage>();
        _messages = await _chatManager.GetConversationAsync(currentUserId, contactId);
    }
    private async Task GetUsersAsync()
    {
        ChatUsers = await _chatManager.GetUsersAsync(currentUserId);
    }
}

