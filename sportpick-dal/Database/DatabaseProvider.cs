using MongoDB.Driver;
using Microsoft.Extensions.Configuration;

namespace sportpick_dal
{
 public class DatabaseProvider : IDatabaseProvider
    {
        private readonly IMongoDatabase _database;

        public DatabaseProvider(IConfiguration config)
        {
            var connectionString = Environment.GetEnvironmentVariable("MongoDb") 
                                ?? config.GetConnectionString("MongoDb");

            if (string.IsNullOrEmpty(connectionString))
                throw new Exception("MongoDB connection string not configured.");

            var client = new MongoClient(connectionString);
            _database = client.GetDatabase("dropin");
        }

        public IMongoCollection<T> GetCollection<T>(string collectionName)
        {
            return _database.GetCollection<T>(collectionName);
        }
    }
}