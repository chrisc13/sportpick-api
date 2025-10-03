using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using sportpick_domain;
using MongoDB.Driver.GeoJsonObjectModel;
public class DropInThreadEntity
{
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Title { get; set; } 
        public string Body { get; set; }   
        public string CreatorImageUrl { get; set; }
        public string CreatorName { get; set; }
        public string CreatorId { get; set; }
        public DateTime CreatedAt { get; set; }
        
        public Dictionary<string, object>? ExtraFields { get; set; } 
 }
