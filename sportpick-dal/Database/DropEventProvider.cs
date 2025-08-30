using MongoDB.Driver;
using System;
using System.Collections.Generic;
using MongoDB.Bson;


namespace sportpick_dal
{
    public class DropEventProvider : IDropEventProvider
    {
        private readonly IMongoCollection<DropEventEntity> _dropEvents;

        public DropEventProvider(IDatabaseProvider databaseProvider)
        {
            _dropEvents = databaseProvider.GetCollection<DropEventEntity>("events");
        }

        public List<DropEventEntity> GetAllDropEventInfo()
        {
            // fetch all documents from the collectionk
            return _dropEvents.Find(_ => true).ToList();
        }

        public bool CreateEvent(DropEventEntity newEvent)
        {
            try
            {
                _dropEvents.InsertOne(newEvent);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Mongo insert error: {ex.Message}");
                return false;
            }
        }

        public bool AttendEvent(string eventId, string username)
        {
            var filter = Builders<DropEventEntity>.Filter.Eq("_id", new ObjectId(eventId));

            var update = Builders<DropEventEntity>.Update.Combine(
                Builders<DropEventEntity>.Update.Inc("CurrentPlayers", 1)
                //Builders<DropEventEntity>.Update.Push("Attendees", userId) // add attendee to array
            );


            try
            {
                _dropEvents.UpdateOne(filter, update);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Mongo insert error: {ex.Message}");
                return false;
            }
        }
    }
}
