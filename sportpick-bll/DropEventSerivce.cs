using sportpick_dal;
using sportpick_domain;

namespace sportpick_bll;
public class DropEventService : IDropEventService{

    IDropEventRepository _dropEventRepository;

    public DropEventService(IDropEventRepository dropEventRepository){
        _dropEventRepository = dropEventRepository;
    }

    public List<DropEvent> GetAllDropEvents(){
        List<DropEvent> dropEvents = new List<DropEvent>();
        dropEvents = _dropEventRepository.GetAllDropEventInfo();

        return dropEvents;
    }

    public List<DropEvent> GetTopThreePopular()
    {
        var dropEvents = _dropEventRepository.GetAllDropEventInfo();

        if (dropEvents == null || dropEvents.Count == 0)
            return new List<DropEvent>();

        // Order descending by CurrentPlayers
        dropEvents = dropEvents.OrderByDescending(de => de.Date).ToList();

        // Take up to 3 items
        return dropEvents.Take(3).ToList();
    }


    public bool CreateEvent(DropEvent newEvent){
        var result = _dropEventRepository.CreateEvent(newEvent);
        if (!result){
            return false;
        }
        return true;
    }



}