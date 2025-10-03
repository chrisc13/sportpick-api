using sportpick_dal;
using sportpick_domain;
using System;
using System.Collections.Generic;

namespace sportpick_bll;
public class DropInThreadService : IDropInThreadService{

    IDropInThreadRepository _dropInThreadRepository;
    IDropInThreadCommentRepository _dropInThreadCommentRepository;
    IDropInThreadLikeRepository _dropInThreadLikeRepository;

    public DropInThreadService(IDropInThreadRepository dropInThreadRepository, IDropInThreadCommentRepository dropInThreadCommentRepository, 
        IDropInThreadLikeRepository dropInThreadLikeRepository){
        _dropInThreadRepository = dropInThreadRepository;
        _dropInThreadCommentRepository = dropInThreadCommentRepository;
        _dropInThreadLikeRepository = dropInThreadLikeRepository;
    }

    public async Task<List<DropInThread>> GetFifteenDropInThreadAsync(){
        List<DropInThread> dropInThreads = new List<DropInThread>();
        dropInThreads = await _dropInThreadRepository.GetFifteenDropInThreadAsync();
        foreach (var thread in dropInThreads)
        {
            thread.CommentCount = await _dropInThreadCommentRepository.GetCommentCountByThreadIdAsync(thread.Id);
            thread.LikeCount = await _dropInThreadLikeRepository.GetLikeCountByThreadIdAsync(thread.Id);
        }
        return dropInThreads;
    }

    public async Task<DropInThread?> GetDropInThreadByIdAsync(string id)
    {
        // Get the main thread entity from the repository
        var dropInThread = await _dropInThreadRepository.GetDropInThreadByIdAsync(id);
        if (dropInThread == null) return null;

        // Fetch comments and likes
        var comments = await _dropInThreadCommentRepository.GetCommentsByThreadIdAsync(id);
        var likes = await _dropInThreadLikeRepository.GetLikesByThreadIdAsync(id);

        // Map them into the domain object
        dropInThread.Comments = comments ?? new List<Comment>();
        dropInThread.Likes = likes ?? new List<Like>();
        dropInThread.CommentCount = dropInThread.Comments.Count;
        dropInThread.LikeCount = dropInThread.Likes.Count;

        return dropInThread;
    }
    public async Task<bool> AddThreadCommentAsync(Comment newComment, string threadId)
    {
        return await _dropInThreadCommentRepository.AddThreadCommentAsync(newComment, threadId);
    }
    public async Task<bool> AddThreadLikeAsync(Like like, string threadId)
    {
        return await _dropInThreadLikeRepository.AddLikeAsync(like, threadId);
    }
    public async Task<bool> RemoveThreadLikeAsync(string likeid)
    {
        return await _dropInThreadLikeRepository.RemoveLikeAsync(likeid);
    }

    public async Task<bool> CreateDropInThreadAsync(DropInThread newDropInThread){
        var result = await _dropInThreadRepository.CreateDropInThreadAsync(newDropInThread);
        if (!result){
            return false;
        }
        return true;
    }

}