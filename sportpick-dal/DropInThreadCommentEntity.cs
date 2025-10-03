using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using sportpick_domain;
using MongoDB.Driver.GeoJsonObjectModel;

public class DropInThreadCommentEntity
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }  // unique comment id

    [BsonRepresentation(BsonType.ObjectId)]
    public string ThreadId { get; set; }  // reference to DropInThread

    public string Username { get; set; }
    public string UserId { get; set; }
    public string UserImageUrl { get; set; }  // optional avatar
    public string Body { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
