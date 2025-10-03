using sportpick_domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace sportpick_dal
{
    public interface IDropInThreadCommentRepository
    {
        Task<List<Comment>> GetCommentsByThreadIdAsync(string threadId);
        Task<bool> AddThreadCommentAsync(Comment comment, string threadId);
        Task<int> GetCommentCountByThreadIdAsync(string threadId);
    }

    public class DropInThreadCommentRepository : IDropInThreadCommentRepository
    {
        private readonly IDropInThreadCommentProvider _provider;

        public DropInThreadCommentRepository(IDropInThreadCommentProvider provider)
        {
            _provider = provider;
        }

        public async Task<List<Comment>> GetCommentsByThreadIdAsync(string threadId)
        {
            var entities = await _provider.GetCommentsByThreadIdAsync(threadId);
            var comments = new List<Comment>();
            foreach (var e in entities)
            {
                comments.Add(new Comment
                {
                    Id = e.Id,
                    Username = e.Username,
                    ThreadId = e.ThreadId,
                    UserImageUrl = e.UserImageUrl,
                    UserId = e.UserId,
                    CreatedAt = e.CreatedAt,
                    Body = e.Body
                });
            }
            return comments;
        }

        public async Task<bool> AddThreadCommentAsync(Comment comment, string threadId)
        {
            var entity = new DropInThreadCommentEntity
            {
                ThreadId = threadId,
                Username = comment.Username,
                UserId = comment.Id,
                UserImageUrl = comment.UserImageUrl,
                Body = comment.Body,
                CreatedAt = System.DateTime.UtcNow
            };
            return await _provider.AddCommentAsync(entity);
        }
        public async Task<int> GetCommentCountByThreadIdAsync(string threadId)
        {
            return await _provider.GetCommentCountByThreadIdAsync(threadId);
        }
    }
}
