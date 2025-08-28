using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class DropEventEntity
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }   // Mongo’s internal _id, now your main ID

    public string EventName { get; set; }
    public string SportType { get; set; }
    public string LocationName { get; set; }
    public string City { get; set; }
    public DateTime Date { get; set; }
    public string StartTime { get; set; }
    public string EndTime { get; set; }
    public int MaxPlayers { get; set; }
    public int CurrentPlayers { get; set; }
    public string OrganizerId { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}
