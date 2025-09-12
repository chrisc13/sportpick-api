using sportpick_domain;

namespace sportpick_dal;

public class DropEventRepository : IDropEventRepository{

    private IDropEventProvider _dropEventProvider;

    public DropEventRepository(IDropEventProvider dropEventProvider){
        _dropEventProvider = dropEventProvider;
    }

    public async Task<List<DropEvent>> GetAllDropEventInfoAsync()
    {
        var allDropEvents = new List<DropEvent>();

        var collection = await _dropEventProvider.GetAllDropEventInfoAsync();
        foreach (var item in collection)
        {
            allDropEvents.Add(DropEventMapper.ToDomain(item));
        }

        return allDropEvents;
    }


    public async Task<bool> CreateEventAsync(DropEvent newEvent){
        var newEntity = DropEventMapper.ToEntity(newEvent); 
        return await _dropEventProvider.CreateEventAsync(newEntity);
    }
    
    public async Task<bool> AttendEventAsync(Attendee attendee, string eventId){
        return await _dropEventProvider.AttendEventAsync(attendee, eventId);
    }
    
}