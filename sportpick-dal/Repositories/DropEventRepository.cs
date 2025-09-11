using sportpick_domain;

namespace sportpick_dal;

public class DropEventRepository : IDropEventRepository{

    private IDropEventProvider _dropEventProvider;

    public DropEventRepository(IDropEventProvider dropEventProvider){
        _dropEventProvider = dropEventProvider;
    }

    public List<DropEvent> GetAllDropEventInfo()
    {
        var allDropEvents = new List<DropEvent>();

        var collection = _dropEventProvider.GetAllDropEventInfo();
        foreach (var item in collection)
        {
            allDropEvents.Add(DropEventMapper.ToDomain(item));
        }

        return allDropEvents;
    }


    public bool CreateEvent(DropEvent newEvent){
        var newEntity = DropEventMapper.ToEntity(newEvent); 
        return _dropEventProvider.CreateEvent(newEntity);
    }
    
    public bool AttendEvent(Attendee attendee, string eventId){
        return _dropEventProvider.AttendEvent(attendee, eventId);
    }
    
}