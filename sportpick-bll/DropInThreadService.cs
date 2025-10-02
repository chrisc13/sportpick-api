using sportpick_dal;
using sportpick_domain;
using System;

namespace sportpick_bll;
public class DropInThreadService : IDropInThreadService{

    IDropInThreadRepository _dropInThreadRepository;

    public DropInThreadService(IDropInThreadRepository dropInThreadRepository){
        _dropInThreadRepository = dropInThreadRepository;
    }

    public async Task<List<DropInThread>> GetFifteenDropInThreadAsync(){
        List<DropInThread> dropInThreads = new List<DropInThread>();
        dropInThreads = await _dropInThreadRepository.GetFifteenDropInThreadAsync();

        return dropInThreads;
    }

/*
    public async Task<List<DropInThread>> GetTopThreePopularAsync()
    {
        var dropInThreads = await _dropInThreadRepository.GetFifteenDropInThreadInfoAsync();

        if (dropInThreads == null || dropInThreads.Count == 0)
            return new List<DropInThread>();

        // Order descending by CurrentPlayers
        dropInThreads = dropInThreads.OrderByDescending(de => de.Start).ToList();

        return dropInThreads.Take(3).ToList();
    }
    public async Task<List<DropInThread>> GetTopThreeUpcomingAsync()
    {
        var dropInThreads = await _dropInThreadRepository.GetTopThreeUpcomingAsync();

        if (dropInThreads == null || dropInThreads.Count == 0)
            return new List<DropInThread>();

        // Order descending by CurrentPlayers
        dropInThreads = dropInThreads.OrderByDescending(de => de.Start).ToList();

        return dropInThreads.Take(3).ToList();
    }
*/

    public async Task<bool> CreateDropInThreadAsync(DropInThread newDropInThread){
        var result = await _dropInThreadRepository.CreateDropInThreadAsync(newDropInThread);
        if (!result){
            return false;
        }
        return true;
    }

}