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

    // Add message, trim stream, and publish
    public async Task<string> AddMessageAsync(string streamKey, string sender, string message)
    {
        var id = await _db.StreamAddAsync(streamKey, new NameValueEntry[]
        {
            new NameValueEntry("sender", sender),
            new NameValueEntry("message", message),
            new NameValueEntry("timestamp", DateTime.UtcNow.ToString("o"))
        });

        // Trim if over limit
        long length = await _db.StreamLengthAsync(streamKey);
        if (length > MaxMessagesPerConversation){
            await _db.StreamTrimAsync(streamKey, MaxMessagesPerConversation);
        }
        // Publish for real-time delivery
        await _sub.PublishAsync(streamKey, $"{id}:{sender}:{message}");

        return id;
    }

    // Read messages from a stream
    public async Task<ChatMessage[]> ReadMessagesAsync(string streamKey, string lastId = "0-0")
    {
        var entries = await _db.StreamReadAsync(streamKey, lastId);

        return entries.Select(e => new ChatMessage
        {
            Id = e.Id,
            Sender = e.Values.FirstOrDefault(v => v.Name == "sender").Value.ToString(),
            Message = e.Values.FirstOrDefault(v => v.Name == "message").Value.ToString(),
            Timestamp = DateTimeOffset.FromUnixTimeMilliseconds(long.Parse(e.Id.ToString().Split('-')[0])).UtcDateTime}).ToArray();
    }

public async Task<IEnumerable<ConversationPreview>> GetUserConversationsAsync(string username)
{
    var db = _db;
    var muxer = _sub.Multiplexer;
    var server = muxer.GetServer(muxer.GetEndPoints().First());

    // Get all keys where user is either first or second in the chat
    var keys = server.Keys(pattern: $"chat:{username}_*")
                     .Concat(server.Keys(pattern: $"chat:*_{username}"));

    var conversations = new List<ConversationPreview>();

    foreach (var key in keys)
    {
        // Get the last message in this stream
        var entries = await db.StreamRangeAsync(key.ToString(), "-", "+", count: 1);
        var lastEntry = entries.LastOrDefault();

        if (lastEntry.Equals(default(StreamEntry))) continue;

        // Determine the other user in this conversation
        string otherUser = key.ToString().StartsWith($"chat:{username}_")
            ? key.ToString().Replace($"chat:{username}_", "")
            : key.ToString().Replace($"chat:", "").Replace($"_{username}", "");

        // Extract values safely
        RedisValue GetValue(string field)
        {
            var entry = lastEntry.Values.FirstOrDefault(v => v.Name == field);
            return entry.Equals(default(NameValueEntry)) ? RedisValue.Null : entry.Value;
        }

       
        // Map last message
        var chatMsg = new ChatMessage
        {
            Id = lastEntry.Id,
            Sender = GetValue("sender"),
            Message = GetValue("message"),
            Timestamp = DateTime.TryParse(GetValue("timestamp").ToString(), out var dt) ? dt : DateTime.MinValue
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
