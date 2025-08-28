using MongoDB.Driver;
using Microsoft.Extensions.Configuration;

namespace sportpick_dal
{
 public class DatabaseProvider : IDatabaseProvider
    {
        private readonly IMongoDatabase _database;

        public DatabaseProvider(IConfiguration config)
            {
                var connectionString = config.GetConnectionString("MongoDb");
                var client = new MongoClient(connectionString);
                _database = client.GetDatabase("dropin"); // your DB name
            }
        public IMongoCollection<T> GetCollection<T>(string collectionName)
        {
            return _database.GetCollection<T>(collectionName);
        }
    }
}