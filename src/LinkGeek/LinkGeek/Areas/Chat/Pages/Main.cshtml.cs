using System.Security.Claims;
using LinkGeek.AppIdentity;
using LinkGeek.Services;
using LinkGeek.Shared;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR.Client;

namespace LinkGeek.Areas.Chat.Pages;

[Authorize]
public class ChatModel : PageModel
{
    public ChatModel(ChatService chatManager, UserManager<ApplicationUser> userManager)
    {
        _chatManager = chatManager;
        _userManager = userManager;
    }

    [CascadingParameter] public HubConnection hubConnection { get; set; }
    [Parameter] public string CurrentMessage { get; set; }
    [Parameter] public string CurrentUserId { get; set; }
    [Parameter] public string CurrentUserEmail { get; set; }
    private readonly ChatService _chatManager;
    private readonly  UserManager<ApplicationUser> _userManager;
    private List<ChatMessage> messages;
    
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
            var userId = User.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier).Select(a => a.Value).FirstOrDefault();
            await _chatManager.SaveMessageAsync(chatHistory, userId);
            chatHistory.FromUserId = CurrentUserId;
            await hubConnection.SendAsync("SendMessageAsync", chatHistory, CurrentUserEmail);
            CurrentMessage = string.Empty;
        }
    }
    protected async Task OnInitializedAsync()
    {
        if (hubConnection == null)
        {
            hubConnection = new HubConnectionBuilder().WithUrl(HttpContext.Request.GetDisplayUrl() +"/signalRHub").Build();
        }
        if (hubConnection.State == HubConnectionState.Disconnected)
        {
            await hubConnection.StartAsync();
        }
        hubConnection.On<ChatMessage, string>("ReceiveMessage", async (message, userName) =>
        {
            if ((ContactId == message.ToUserId && CurrentUserId == message.FromUserId) || (ContactId == message.FromUserId && CurrentUserId == message.ToUserId))
            {
                   
                if ((ContactId == message.ToUserId && CurrentUserId == message.FromUserId))
                {
                    messages.Add(new ChatMessage { Message = message.Message, CreatedDate = message.CreatedDate, FromUser = new ApplicationUser() { Email = CurrentUserEmail } } );
                    await hubConnection.SendAsync("ChatNotificationAsync", $"New Message From {userName}", ContactId, CurrentUserId);
                }
                else if ((ContactId == message.FromUserId && CurrentUserId == message.ToUserId))
                {
                    messages.Add(new ChatMessage { Message = message.Message, CreatedDate = message.CreatedDate, FromUser = new ApplicationUser() { Email = ContactEmail } });
                }
            }
        });
        await GetUsersAsync();
        var user = await _userManager.GetUserAsync(User);
        CurrentUserId = user.Id;
        CurrentUserEmail = user.Email;
        if (!string.IsNullOrEmpty(ContactId))
        {
            await LoadUserChat(ContactId);
        }
    }
    
    public List<ApplicationUser> ChatUsers = new List<ApplicationUser>();
    [Parameter] public string ContactEmail { get; set; }
    [Parameter] public string ContactId { get; set; }
    async Task LoadUserChat(string contactId)
    {
        var contact = await _chatManager.GetUserDetailsAsync(contactId);
        ContactId = contact.Id;
        ContactEmail = contact.Email;
        messages = new List<ChatMessage>();
        var userId = User.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier).Select(a => a.Value).FirstOrDefault();
        messages = await _chatManager.GetConversationAsync(userId, contactId);
    }
    private async Task GetUsersAsync()
    {
        var userId = User.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier).Select(a => a.Value).FirstOrDefault();
        var allUsers = _chatManager.GetUsersAsync(userId);
    }
}

