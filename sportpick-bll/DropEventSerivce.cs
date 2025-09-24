using sportpick_dal;
using sportpick_domain;
using System;

namespace sportpick_bll;
public class DropEventService : IDropEventService{

    IDropEventRepository _dropEventRepository;

    public DropEventService(IDropEventRepository dropEventRepository){
        _dropEventRepository = dropEventRepository;
    }

    public async Task<List<DropEvent>> GetAllDropEventsAsync(){
        List<DropEvent> dropEvents = new List<DropEvent>();
        dropEvents = await _dropEventRepository.GetAllDropEventInfoAsync();

        return dropEvents;
    }

    public async Task<List<DropEvent>> GetTopThreePopularAsync()
    {
        var dropEvents = await _dropEventRepository.GetAllDropEventInfoAsync();

        if (dropEvents == null || dropEvents.Count == 0)
            return new List<DropEvent>();

        // Order descending by CurrentPlayers
        dropEvents = dropEvents.OrderByDescending(de => de.Start).ToList();

        return dropEvents.Take(3).ToList();
    }


    public async Task<bool> CreateEventAsync(DropEvent newEvent){
        var result = await _dropEventRepository.CreateEventAsync(newEvent);
        if (!result){
            return false;
        }
        return true;
    }

    public async Task<bool> AttendEventAsync(Attendee attendee, string eventId){
        if (String.IsNullOrEmpty(attendee.Username) || String.IsNullOrEmpty(attendee.Id) || String.IsNullOrEmpty(eventId)){
            return false;
        }

        var result = await _dropEventRepository.AttendEventAsync(attendee, eventId);
        if (!result){
            return false;
        }
        return true;
    }

    public async Task<List<DropEvent>> GetNearbyEventsAsync(double maxDistance, (double latitude, double longitude) location){
        List<DropEvent> dropEvents = new List<DropEvent>();
        dropEvents = await _dropEventRepository.GetNearbyEventsAsync(maxDistance, location);

        return dropEvents;
    }

}