using Microsoft.AspNetCore.SignalR;
using sportpick_domain;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Collections.Concurrent;

[Authorize]
public class ChatHub : Hub
{
    private readonly MessagingService _messagingService;

    // Track subscriptions to avoid duplicate callbacks
    private static readonly ConcurrentDictionary<string, bool> _subscribedStreams = new();

    public ChatHub(MessagingService messagingService)
    {
        _messagingService = messagingService;
    }

    private string GetCurrentUsername()
    {
        return Context.User?.FindFirst(ClaimTypes.Name)?.Value ?? "unknown";
    }

    // Join a conversation (SignalR group + Redis subscription)
// Join a conversation (SignalR group + Redis subscription + fetch old messages)
    public async Task<IEnumerable<sportpick_domain.ChatMessage>> JoinConversation(string otherUser)
    {
        string currentUser = GetCurrentUsername();
        string streamKey = GetStreamKey(currentUser, otherUser);

        // Add connection to the group
        await Groups.AddToGroupAsync(Context.ConnectionId, streamKey);

        // Subscribe only once per streamKey
        if (!_subscribedStreams.ContainsKey(streamKey))
        {
            _subscribedStreams[streamKey] = true;

            await _messagingService.SubscribeToStream(streamKey, async (msg) =>
            {
                try
                {
                    await Clients.Group(streamKey)
                        .SendAsync("ReceiveMessage", msg.Id, msg.Sender, msg.Message, msg.Timestamp);
                }
                catch { }
            });
        }

        // Fetch old messages from Redis
        var oldMessages = await _messagingService.GetMessages(currentUser, otherUser, "0-0");
        return oldMessages;
    }


    // Send a message
    public async Task SendMessage(string otherUser, string message)
    {
        string sender = GetCurrentUsername();
        string streamKey = GetStreamKey(sender, otherUser);

        // Store message in Redis
        var msgId = await _messagingService.SendMessage(sender, otherUser, sender, message);

        // Broadcast immediately to group
        await Clients.Group(streamKey)
            .SendAsync("ReceiveMessage", msgId, sender, message, DateTime.UtcNow);
    }

    // Get historical messages
    public async Task<IEnumerable<sportpick_domain.ChatMessage>> GetMessages(string otherUser, string lastId = "0-0")
    {
        string currentUser = GetCurrentUsername();
        return await _messagingService.GetMessages(currentUser, otherUser, lastId);
    }

    // Get user's conversation previews
    public async Task<IEnumerable<ConversationPreview>> GetUserConversations()
    {
        string currentUser = GetCurrentUsername();
        return await _messagingService.GetUserConversationsAsync(currentUser);
    }

    // Helper: consistent Redis stream key
    private string GetStreamKey(string user1, string user2)
    {
        var users = new[] { user1, user2 }.OrderBy(u => u).ToArray();
        return $"chat:{users[0]}_{users[1]}";
    }
}
