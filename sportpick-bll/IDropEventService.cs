using sportpick_domain;
namespace sportpick_bll;

public interface IDropEventService
{
    Task<List<DropEvent>> GetFifteenDropEventInfoAsync();  
    Task<List<DropEvent>> GetTopThreePopularAsync();
    Task<List<DropEvent>> GetTopThreeUpcomingAsync();    
    Task<bool> CreateEventAsync(DropEvent newEvent);
    Task<bool> AttendEventAsync(Attendee attendee, string eventId);
    Task<List<DropEvent>> GetNearbyEventsAsync(double maxDistance, (double latitude, double longitude) location);
}
