using sportpick_domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace sportpick_dal
{
    public interface IDropInThreadLikeRepository
    {
        Task<List<Like>> GetLikesByThreadIdAsync(string threadId);
        Task<bool> AddLikeAsync(Like like, string threadId);
        Task<bool> RemoveLikeAsync(string likeId);
        Task<int> GetLikeCountByThreadIdAsync(string threadId);
    }

    public class DropInThreadLikeRepository : IDropInThreadLikeRepository
    {
        private readonly IDropInThreadLikeProvider _provider;

        public DropInThreadLikeRepository(IDropInThreadLikeProvider provider)
        {
            _provider = provider;
        }

        public async Task<List<Like>> GetLikesByThreadIdAsync(string threadId)
        {
            var entities = await _provider.GetLikesByThreadIdAsync(threadId);
            var likes = new List<Like>();
            foreach (var e in entities)
            {
                likes.Add(new Like
                {
                    Id = e.Id,
                    Username = e.Username,
                    UserId = e.UserId,
                    CreatedAt = e.CreatedAt
                });
            }
            return likes;
        }

        public async Task<bool> AddLikeAsync(Like like, string threadId)
        {
            var entity = new DropInThreadLikeEntity
            {
                ThreadId = threadId,
                Username = like.Username,
                UserId = like.UserId,
                CreatedAt = System.DateTime.UtcNow
            };
            return await _provider.AddLikeAsync(entity);
        }

        public async Task<bool> RemoveLikeAsync(string likeId)
        {
            return await _provider.RemoveLikeAsync(likeId);
        }
        public async Task<int> GetLikeCountByThreadIdAsync(string threadId)
        {
            return await _provider.GetLikeCountByThreadIdAsync(threadId);
        }
    }
}
