using LinkGeek.AppIdentity;
using LinkGeek.Services;
using LinkGeek.Shared;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;

namespace LinkGeek.Areas.Chat.Pages;

/// <summary>
/// Class <c>Chat</c> logic for the chat function
/// </summary>

[Authorize]
public partial class Chat
{
    [Inject]
    private UserManager<ApplicationUser> UserManager { get; set; }
    [Inject]
    private IJSRuntime _jsRuntime { get; set; }
    [Inject]
    private NavigationManager NavigationManager { get; set; }
    [Inject]
    private ChatService _chatManager { get; set; }
    [Inject]
    private AuthenticationStateProvider AuthenticationStateProvider { get; set; }
    [CascadingParameter] public HubConnection? hubConnection { get; set; }
    [CascadingParameter] public ApplicationUser? currentUser { get; set; }
    [Parameter] public string ContactId { get; set; }

    [Inject]
    UserService UserService { get; set; }

    private string? _currentMessage;
    private List<ApplicationUser>? _chatUsers;
    private ApplicationUser? _selectedContact;
    private List<ChatMessage>? _messages;
    private readonly Dictionary<string, string> _lastMessagePerUser = new();

    /// <summary>
    /// Method <c>SubmitAsync</c> Creates a new ChatMessage and calls the sending function
    /// </summary>
    private async Task SubmitAsync()
    {
        if (!string.IsNullOrEmpty(_currentMessage) && !string.IsNullOrEmpty(ContactId))
        {
            var message = new ChatMessage()
            {
                Message = _currentMessage,
                ToUserId = ContactId,
                CreatedDate = DateTime.Now,
                FromUserId = currentUser.Id
            };
            await _chatManager.SaveMessageAsync(message, currentUser.Id);
            await hubConnection.SendAsync("SendMessageAsync", message, currentUser.UserName);
            
            _currentMessage = string.Empty;
            await InvokeAsync(StateHasChanged);
            await _jsRuntime.InvokeAsync<string>("ScrollToBottom", "chat-container");
        }
    }

    /// <summary>
    /// Method <c>OnParametersSetAsync</c> Called afterOnInitialized is called
    /// </summary>
    protected override async Task OnParametersSetAsync()
    {
        if (currentUser == null)
        {
            return;
        }
        
        // best case we update the user, worst case it is still the same one. We use this to update the new friends
        currentUser = await UserService.GetUserFromUserNameAsync(currentUser.UserName) ?? currentUser;
        if (hubConnection == null)
            return;
        
        var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
        var user = authState.User;
        if (user is not { Identity.IsAuthenticated: true })
        {
            return;
        }

        if (hubConnection.State == HubConnectionState.Disconnected)
        {
            await hubConnection.StartAsync();
        }
        hubConnection.On<ChatMessage, string>("ReceiveMessage", async (message, userName) =>
        {
            if (_messages.Any(m => m.Id == message.Id)) return;
            if ((ContactId == message.ToUserId && currentUser.Id == message.FromUserId) 
                || (ContactId == message.FromUserId && currentUser.Id == message.ToUserId))
            {
                if (ContactId == message.ToUserId && currentUser.Id == message.FromUserId) // if the message is from the current user
                {
                    _messages?.Add(new ChatMessage { Id = message.Id, Message = message.Message, CreatedDate = message.CreatedDate, FromUser = new ApplicationUser { UserName = currentUser.UserName} } );
                    // Notify the recipient
                    await hubConnection.SendAsync("ChatNotificationAsync", $"New Message From {userName}", ContactId, currentUser.Id);
                }
                else if (ContactId == message.FromUserId && currentUser.Id == message.ToUserId) // if the message is for the current user
                {
                    _messages?.Add(new ChatMessage { Id = message.Id, Message = message.Message, CreatedDate = message.CreatedDate, FromUser = new ApplicationUser { UserName = _selectedContact?.UserName} });
                }
                
                await InvokeAsync(StateHasChanged);
                await _jsRuntime.InvokeAsync<string>("ScrollToBottom", "chat-container");
            }
        });
        
        _chatUsers = new List<ApplicationUser>(currentUser.Friends);
        foreach (var chatUser in _chatUsers)
        {
            FetchLastMessage(chatUser.Id);
        }
        if (!string.IsNullOrEmpty(ContactId))
        {
            _selectedContact = _chatUsers.FirstOrDefault(x => x.Id == ContactId);
            await LoadUserChat(ContactId);
        }
    }

    /// <summary>
    /// Method <c>Enter</c> When the user hits enter, the message is send
    /// </summary>
    private async Task Enter(KeyboardEventArgs e)
    {
        if (e.Code == "Enter" || e.Code == "NumpadEnter")
        {
            await SubmitAsync();
        }
    }

    /// <summary>
    /// Method <c>LoadUserChat</c> Loads the users chat
    /// </summary>
    private async Task LoadUserChat(string contactId)
    {
        _messages = null;
        _selectedContact = _chatUsers?.FirstOrDefault(x => x.Id == contactId);
        ContactId = _selectedContact?.Id ?? (await _chatManager.GetUserDetailsAsync(contactId)).Id;
        await InvokeAsync(StateHasChanged);
        _messages = await _chatManager.GetConversationAsync(currentUser.Id, contactId);
        await InvokeAsync(StateHasChanged);
        await _jsRuntime.InvokeAsync<string>("ScrollToBottom", "chat-container");
    }

    /// <summary>
    /// Method <c>LoadUserChat</c> Loads the users chat
    /// </summary>
    private async Task FetchLastMessage(string contactId)
    {
        var messages = await _chatManager.GetConversationAsync(currentUser.Id, contactId, 1);
        if (messages.Count > 0)
        {
            _lastMessagePerUser[contactId] = messages[0].Message;
            await InvokeAsync(StateHasChanged);
        }
    }
    
    private string GetImageSrc(ApplicationUser player)
    {
        return "data:" + player.ProfilePictureContentType + ";base64," + player.ProfilePictureData;
    }
}

