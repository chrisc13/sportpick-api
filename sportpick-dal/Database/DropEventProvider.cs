using MongoDB.Driver;
using System;
using System.Collections.Generic;
using MongoDB.Bson;
using sportpick_domain;
using MongoDB.Driver.GeoJsonObjectModel;


namespace sportpick_dal
{
    public class DropEventProvider : IDropEventProvider
    {
        private readonly IMongoCollection<DropEventEntity> _dropEvents;

        public DropEventProvider(IDatabaseProvider databaseProvider)
        {
            _dropEvents = databaseProvider.GetCollection<DropEventEntity>("events");
        }

        public async Task<List<DropEventEntity>> GetFifteenDropEventInfoAsync()
        {
            try
                {
                    return _dropEvents.Find(_ => true).Limit(15).ToList() ?? new List<DropEventEntity>();
                }
            catch (Exception ex)
            {
                // log it if needed
                Console.WriteLine($"Error fetching drop events: {ex.Message}");
                return new List<DropEventEntity>();
            }
        }
        public async Task<List<DropEventEntity>> GetTopThreeUpcomingAsync()
        {
            try{
                var now = DateTime.UtcNow;
                var upcomingEvents = await _dropEvents
                .Find(e => e.Start >= now)         // only future events
                .SortBy(e => e.Start)             // earliest first
                .Limit(3)                         // top 3
                .ToListAsync();

                return upcomingEvents;
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
            var filter = Builders<DropEventEntity>.Filter.Eq("_id", new ObjectId(eventId)) & Builders<DropEventEntity>.Filter.Ne("Attendees.Username", attendee.Username);


            var update = Builders<DropEventEntity>.Update.Combine(
                Builders<DropEventEntity>.Update.Inc("CurrentPlayers", 1),
                Builders<DropEventEntity>.Update.Push("Attendees", attendee) // add attendee to array
            );

            try
            {
                var result = await _dropEvents.UpdateOneAsync(filter, update);
                return result.ModifiedCount > 0; // true if attendee was added
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Mongo insert error: {ex.Message}");
                return false;
            }
        }

        public async Task<List<DropEventEntity>> GetNearbyEventsAsync(
            double maxDistanceMiles,
            (double latitude, double longitude) location
        )
        {
            double maxDistanceMeters = maxDistanceMiles * 1609.34;

            var point = GeoJson.Point(
                GeoJson.Geographic(location.longitude, location.latitude)
            );

            var geoFilter = Builders<DropEventEntity>.Filter.NearSphere(
                x => x.GeoLocation,
                point,
                maxDistance: maxDistanceMeters
            );
               var now = DateTime.UtcNow;
                var futureFilter = Builders<DropEventEntity>.Filter.Gte(x => x.Start, now);

                // Combine filters
                var combinedFilter = Builders<DropEventEntity>.Filter.And(geoFilter, futureFilter);

            try
            {
                return await _dropEvents.Find(combinedFilter).Limit(15).ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Mongo find by geo error: {ex.Message}");
                return new List<DropEventEntity>();
            }
        }


    }
}
