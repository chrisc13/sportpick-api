using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using sportpick_domain;

public class DropEventEntity
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }   // Mongo’s internal _id, now your main ID

    public string EventName { get; set; }   
    public string EventDetails { get; set; }
    public string Sport { get; set; }
    public string Location { get; set; }     
    public string? LocationDetails { get; set; }   
    public DateTime Start { get; set; }
    public DateTime? End { get; set; }            
    public int? MaxPlayers { get; set; }       
    public int? CurrentPlayers { get; set; } 
    public List<Attendee>? Attendees { get; set; } 
    public string OrganizerName { get; set; } 
    public string OrganizerId { get; set; }  
    public double? Latitude { get; set; }          
    public double? Longitude { get; set; }         
    
    public Dictionary<string, object>? ExtraFields { get; set; } 
}
