using MongoDB.Driver;
using System;
using System.Collections.Generic;
using MongoDB.Bson;
using sportpick_domain;
using MongoDB.Driver.GeoJsonObjectModel;


namespace sportpick_dal
{
    public class DropInThreadProvider : IDropInThreadProvider
    {
        private readonly IMongoCollection<DropInThreadEntity> _dropInThreads;

        public DropInThreadProvider(IDatabaseProvider databaseProvider)
        {
            _dropInThreads = databaseProvider.GetCollection<DropInThreadEntity>("threads");
        }

        public async Task<List<DropInThreadEntity>> GetFifteenDropInThreadAsync()
        {
            try
                {
                    return _dropInThreads.Find(_ => true).Limit(15).ToList() ?? new List<DropInThreadEntity>();
                }
            catch (Exception ex)
            {
                // log it if needed
                Console.WriteLine($"Error fetching drop in threads: {ex.Message}");
                return new List<DropInThreadEntity>();
            }
        }

        public async Task<bool> CreateDropInThreadAsync(DropInThreadEntity newEvent)
        {
            try
            {
                _dropInThreads.InsertOne(newEvent);
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
