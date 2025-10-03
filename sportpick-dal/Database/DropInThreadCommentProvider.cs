using MongoDB.Driver;
using MongoDB.Bson;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace sportpick_dal
{
    public interface IDropInThreadCommentProvider
    {
        Task<List<DropInThreadCommentEntity>> GetCommentsByThreadIdAsync(string threadId);
        Task<bool> AddCommentAsync(DropInThreadCommentEntity comment);
        Task<int> GetCommentCountByThreadIdAsync(string threadId);
    }

    public class DropInThreadCommentProvider : IDropInThreadCommentProvider
    {
        private readonly IMongoCollection<DropInThreadCommentEntity> _comments;

        public DropInThreadCommentProvider(IDatabaseProvider databaseProvider)
        {
            _comments = databaseProvider.GetCollection<DropInThreadCommentEntity>("threadscomments");
        }

        public async Task<List<DropInThreadCommentEntity>> GetCommentsByThreadIdAsync(string threadId)
        {
            return await _comments
                .Find(c => c.ThreadId == threadId)
                .SortBy(c => c.CreatedAt)
                .ToListAsync();
        }

        public async Task<bool> AddCommentAsync(DropInThreadCommentEntity comment)
        {
            await _comments.InsertOneAsync(comment);
            return true;
        }
        public async Task<int> GetCommentCountByThreadIdAsync(string threadId)
        {
            return (int)await _comments.CountDocumentsAsync(c => c.ThreadId == threadId);
        }
    }

    
}
