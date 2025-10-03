using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using sportpick_domain;
using MongoDB.Driver.GeoJsonObjectModel;

public class DropInThreadLikeEntity
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }  // unique like id

    [BsonRepresentation(BsonType.ObjectId)]
    public string ThreadId { get; set; }  // reference to DropInThread

    public string Username { get; set; }  // who liked it
    public string UserId { get; set; }    // optional, user reference
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
