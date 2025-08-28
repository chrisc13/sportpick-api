using MongoDB.Driver;
using sportpick_domain;
using System;
using System.Collections.Generic;

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
    }
}
