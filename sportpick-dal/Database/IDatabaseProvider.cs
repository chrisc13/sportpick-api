using MongoDB.Driver;

namespace sportpick_dal
{
    public interface IDatabaseProvider
    {
        IMongoCollection<T> GetCollection<T>(string collectionName);
    }
}
