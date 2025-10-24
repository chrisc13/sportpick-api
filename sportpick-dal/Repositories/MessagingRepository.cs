using StackExchange.Redis;
using sportpick_domain;

public class MessagingRepository
{
    private readonly IDatabase _db;
    private readonly ISubscriber _sub;
    private const int MaxMessagesPerConversation = 50;

    public MessagingRepository(RedisProvider provider)
    {
        _db = provider.Db;
        _sub = provider.Sub;
    }

    // Add message to stream and publish to Pub/Sub
    public async Task<string> AddMessageAsync(string streamKey, string sender, string message)
    {
        var id = await _db.StreamAddAsync(streamKey, new NameValueEntry[]
        {
            new("sender", sender),
            new("message", message),
            new("timestamp", DateTime.UtcNow.ToString("o"))
        });

        // Trim stream to avoid bloat
        long length = await _db.StreamLengthAsync(streamKey);
        if (length > MaxMessagesPerConversation)
        {
            await _db.StreamTrimAsync(streamKey, MaxMessagesPerConversation);
        }

        // Publish real-time update
        var channel = $"channel:{streamKey}";
        var payload = $"{id}|{sender}|{message}";
        await _sub.PublishAsync(channel, payload);

        return id;
    }

    // Subscribe to Pub/Sub messages in real time
    public async Task SubscribeAsync(string streamKey, Action<ChatMessage> onMessage)
    {
        var channel = $"channel:{streamKey}";

        await _sub.SubscribeAsync(channel, (ch, value) =>
        {
            try
            {
                var parts = value.ToString().Split('|');
                var msg = new ChatMessage
                {
                    Id = parts.Length > 0 ? parts[0] : Guid.NewGuid().ToString(),
                    Sender = parts.Length > 1 ? parts[1] : "unknown",
                    Message = parts.Length > 2 ? parts[2] : "",
                    Timestamp = DateTime.UtcNow
                };
                onMessage(msg);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing message from {channel}: {ex.Message}");
            }
        });
    }

    // Read historical messages from Redis Stream
    public async Task<ChatMessage[]> ReadMessagesAsync(string streamKey, string lastId = "0-0")
    {
        var entries = await _db.StreamReadAsync(streamKey, lastId);

        return entries.Select(e => new ChatMessage
        {
            Id = e.Id,
            Sender = e.Values.FirstOrDefault(v => v.Name == "sender").Value.ToString(),
            Message = e.Values.FirstOrDefault(v => v.Name == "message").Value.ToString(),
            Timestamp = DateTime.TryParse(
                e.Values.FirstOrDefault(v => v.Name == "timestamp").Value.ToString(),
                out var parsed)
                ? parsed
                : DateTime.UtcNow
        }).ToArray();
    }

    // Fetch conversation previews for a user
    public async Task<IEnumerable<ConversationPreview>> GetUserConversationsAsync(string username)
    {
        var muxer = _sub.Multiplexer;
        var server = muxer.GetServer(muxer.GetEndPoints().First());

        // Find all chat keys where user is sender or receiver
        var keys = server.Keys(pattern: $"chat:{username}_*")
                         .Concat(server.Keys(pattern: $"chat:*_{username}"));

        var conversations = new List<ConversationPreview>();

        foreach (var key in keys)
        {
            var entries = await _db.StreamRangeAsync(key.ToString(), "-", "+", count: 1);
            var lastEntry = entries.LastOrDefault();
            if (lastEntry.Equals(default(StreamEntry))) continue;

            string otherUser = key.ToString().StartsWith($"chat:{username}_")
                ? key.ToString().Replace($"chat:{username}_", "")
                : key.ToString().Replace($"chat:", "").Replace($"_{username}", "");

            RedisValue GetValue(string field)
            {
                var entry = lastEntry.Values.FirstOrDefault(v => v.Name == field);
                return entry.Equals(default(NameValueEntry)) ? RedisValue.Null : entry.Value;
            }

            var chatMsg = new ChatMessage
            {
                Id = lastEntry.Id,
                Sender = GetValue("sender"),
                Message = GetValue("message"),
                Timestamp = DateTime.TryParse(GetValue("timestamp").ToString(), out var dt)
                    ? dt
                    : DateTime.MinValue
            };

            conversations.Add(new ConversationPreview
            {
                OtherUser = otherUser,
                LastMessage = chatMsg
            });
        }

        return conversations;
    }
}
