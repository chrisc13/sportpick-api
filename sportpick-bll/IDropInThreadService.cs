using sportpick_domain;
public interface IDropInThreadService{
    Task<List<DropInThread>> GetFifteenDropInThreadAsync();
    Task<DropInThread> GetDropInThreadByIdAsync(string id);
    Task<bool> AddThreadCommentAsync(Comment newComment, string threadId);
    Task<bool> AddThreadLikeAsync(Like like, string threadId);
    Task<bool> RemoveThreadLikeAsync(string likeid);
    Task<bool> CreateDropInThreadAsync(DropInThread newEvent);
}
