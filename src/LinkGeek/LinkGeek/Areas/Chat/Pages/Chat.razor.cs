using LinkGeek.AppIdentity;
using LinkGeek.Shared;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNetCore.Components;
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
    [Parameter] public string CurrentUserUsername { get; set; } 
    [Parameter] public string ContactEmail { get; set; }
    [Parameter] public string ContactId { get; set; }
    public List<ApplicationUser> ChatUsers = new List<ApplicationUser>();
    private List<ChatMessage> _messages = new List<ChatMessage>();
    private string selectedUserId = "-1";
    private string selectedUserName = "";

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
            await _chatManager.SaveMessageAsync(chatHistory, CurrentUserId);
            chatHistory.FromUserId = CurrentUserId;
            await HubConnection.SendAsync("SendMessageAsync", chatHistory, CurrentUserUsername);
            CurrentMessage = string.Empty;
            await InvokeAsync(() => StateHasChanged())
                .ConfigureAwait(true);
        }
    }
    protected override async Task OnInitializedAsync()
    {
        var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
        var user = authState.User;
        
        if (user.Identity.IsAuthenticated)
        {
            var currentUser = await UserManager.GetUserAsync(user);
            CurrentUserId = currentUser.Id;
            var currentUserEmail = currentUser.Email;
            var currentUserPhone = currentUser.PhoneNumber;
            var currentUserEmailConfirmed = currentUser.EmailConfirmed;
            CurrentUserUsername = currentUser.UserName;
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
                    _messages.Add(new ChatMessage { Message = message.Message, CreatedDate = message.CreatedDate, FromUser = new ApplicationUser() { UserName = CurrentUserUsername} } );
                    await HubConnection.SendAsync("ChatNotificationAsync", $"New Message From {userName}", ContactId, CurrentUserId);
                }
                else if ((ContactId == message.FromUserId && CurrentUserId == message.ToUserId))
                {
                    _messages.Add(new ChatMessage { Message = message.Message, CreatedDate = message.CreatedDate, FromUser = new ApplicationUser() { UserName = selectedUserName} });
                }
                await InvokeAsync(() => StateHasChanged())
                    .ConfigureAwait(true);
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
        selectedUserId = contactId;
        var contact = await _chatManager.GetUserDetailsAsync(contactId);
        ContactId = contact.Id;
        ContactEmail = contact.Email;
        selectedUserName = contact.UserName;
        _messages = await _chatManager.GetConversationAsync(CurrentUserId, contactId);
        await InvokeAsync(() => StateHasChanged())
            .ConfigureAwait(true);
    }
    private async Task GetUsersAsync()
    {
        ChatUsers = await _chatManager.GetUsersAsync(CurrentUserId);
    }
}

