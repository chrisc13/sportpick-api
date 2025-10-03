using MongoDB.Driver;
using MongoDB.Bson;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace sportpick_dal
{
    public interface IDropInThreadLikeProvider
    {
        Task<List<DropInThreadLikeEntity>> GetLikesByThreadIdAsync(string threadId);
        Task<bool> AddLikeAsync(DropInThreadLikeEntity like);
        Task<bool> RemoveLikeAsync(string likeId);
        Task<int> GetLikeCountByThreadIdAsync(string threadId);
    }

    public class DropInThreadLikeProvider : IDropInThreadLikeProvider
    {
        private readonly IMongoCollection<DropInThreadLikeEntity> _likes;

        public DropInThreadLikeProvider(IDatabaseProvider databaseProvider)
        {
            _likes = databaseProvider.GetCollection<DropInThreadLikeEntity>("threadslikes");
        }

        public async Task<List<DropInThreadLikeEntity>> GetLikesByThreadIdAsync(string threadId)
        {
            return await _likes
                .Find(l => l.ThreadId == threadId)
                .ToListAsync();
        }

        public async Task<bool> AddLikeAsync(DropInThreadLikeEntity like)
        {
            await _likes.InsertOneAsync(like);
            return true;
        }

        public async Task<bool> RemoveLikeAsync(string likeId)
        {
            var result = await _likes.DeleteOneAsync(l => l.Id == likeId);
            return result.DeletedCount > 0;
        }
        public async Task<int> GetLikeCountByThreadIdAsync(string threadId)
        {
            return (int)await _likes.CountDocumentsAsync(l => l.ThreadId == threadId);
        }
    }
}