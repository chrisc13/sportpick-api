using MongoDB.Driver;
using System;
using System.Collections.Generic;
using MongoDB.Bson;
using sportpick_domain;


namespace sportpick_dal
{
    public class DropEventProvider : IDropEventProvider
    {
        private readonly IMongoCollection<DropEventEntity> _dropEvents;

        public DropEventProvider(IDatabaseProvider databaseProvider)
        {
            _dropEvents = databaseProvider.GetCollection<DropEventEntity>("events");
        }

        public async Task<List<DropEventEntity>> GetAllDropEventInfoAsync()
        {
            try
                {
                    return _dropEvents.Find(_ => true).ToList() ?? new List<DropEventEntity>();
                }
            catch (Exception ex)
            {
                // log it if needed
                Console.WriteLine($"Error fetching drop events: {ex.Message}");
                return new List<DropEventEntity>();
            }
        }

        public async Task<bool> CreateEventAsync(DropEventEntity newEvent)
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

        public async Task<bool> AttendEventAsync(Attendee attendee, string eventId)
        {
            var filter = Builders<DropEventEntity>.Filter.Eq("_id", new ObjectId(eventId));

            var update = Builders<DropEventEntity>.Update.Combine(
                Builders<DropEventEntity>.Update.Inc("CurrentPlayers", 1),
                Builders<DropEventEntity>.Update.Push("Attendees", attendee) // add attendee to array
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
