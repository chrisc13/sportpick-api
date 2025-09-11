using sportpick_dal;
using sportpick_domain;
using System;

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
        dropEvents = dropEvents.OrderByDescending(de => de.Start).ToList();

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

    public bool AttendEvent(Attendee attendee, string eventId){
        if (String.IsNullOrEmpty(attendee.Username) || String.IsNullOrEmpty(attendee.Id) || String.IsNullOrEmpty(eventId)){
            return false;
        }

        var result = _dropEventRepository.AttendEvent(attendee, eventId);
        if (!result){
            return false;
        }
        return true;
    }



}