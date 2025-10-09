using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class AppUserEntity
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }   // Mongo’s internal _id, now your main ID

    public string Username { get; set; }
    public string Password { get; set; }
    public string ProfileImageUrl { get; set; }
}
