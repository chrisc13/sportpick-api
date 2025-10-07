using sportpick_domain;
using System.Linq;

public class MessagingService
{
    private readonly MessagingRepository _repo;

    public MessagingService(MessagingRepository repo)
    {
        _repo = repo;
    }

    // Helper to generate a consistent stream key for any pair of users
    private string GetChatKey(string user1, string user2)
    {
        var users = new[] { user1, user2 }.OrderBy(u => u).ToArray();
        return $"chat:{users[0]}_{users[1]}";
    }

    // Send a message in a conversation
    public async Task<string> SendMessage(string user1, string user2, string sender, string message)
    {
        string streamKey = GetChatKey(user1, user2);
        return await _repo.AddMessageAsync(streamKey, sender, message);
    }

    // Read messages for a conversation
    public async Task<IEnumerable<ChatMessage>> GetMessages(string user1, string user2, string lastId = "0-0")
    {
        string streamKey = GetChatKey(user1, user2);
        return await _repo.ReadMessagesAsync(streamKey, lastId);
    }

    // Get all conversations for a user (only last message per conversation)
    public async Task<IEnumerable<ConversationPreview>> GetUserConversationsAsync(string username)
    {
        return await _repo.GetUserConversationsAsync(username);
    }
}
