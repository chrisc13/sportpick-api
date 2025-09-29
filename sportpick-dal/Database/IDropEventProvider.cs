using MongoDB.Driver;
using sportpick_domain;

namespace sportpick_dal;

public interface IDropEventProvider
{
    Task<List<DropEventEntity>> GetFifteenDropEventInfoAsync();
    Task<List<DropEventEntity>> GetTopThreeUpcomingAsync();
    Task<bool> CreateEventAsync(DropEventEntity newEvent);
    Task<bool> AttendEventAsync(Attendee attendee, string eventId);
    Task<List<DropEventEntity>> GetNearbyEventsAsync(double maxDistance, (double latitude, double longitude) location);
}
