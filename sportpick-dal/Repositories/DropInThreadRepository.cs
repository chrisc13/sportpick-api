using sportpick_domain;

namespace sportpick_dal;

public class DropInThreadRepository : IDropInThreadRepository{

    private IDropInThreadProvider _dropInThreadProvider;

    public DropInThreadRepository(IDropInThreadProvider dropInThreadProvider){
        _dropInThreadProvider = dropInThreadProvider;
    }

    public async Task<List<DropInThread>> GetFifteenDropInThreadAsync()
    {
        var allDropInThreads = new List<DropInThread>();

        var collection = await _dropInThreadProvider.GetFifteenDropInThreadAsync();
        foreach (var item in collection)
        {
            allDropInThreads.Add(DropInThreadMapper.ToDomain(item));
        }

        return allDropInThreads;
    }
    
    public async Task<DropInThread> GetDropInThreadByIdAsync(string id)
    {
        var dropInThread = new DropInThread();

        var entity = await _dropInThreadProvider.GetDropInThreadByIdAsync(id);
        if (entity == null){
            return dropInThread;
        }

        return DropInThreadMapper.ToDomain(entity);
    }

    public async Task<bool> CreateDropInThreadAsync(DropInThread newDropInThread){
        var newEntity = DropInThreadMapper.ToEntity(newDropInThread); 
        return await _dropInThreadProvider.CreateDropInThreadAsync(newEntity);
    }
    

    
}