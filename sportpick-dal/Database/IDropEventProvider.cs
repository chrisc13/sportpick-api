using MongoDB.Driver;
using sportpick_domain;

namespace sportpick_dal;

public interface IDropEventProvider
{
    Task<List<DropEventEntity>> GetAllDropEventInfoAsync();
    Task<bool> CreateEventAsync(DropEventEntity newEvent);
    Task<bool> AttendEventAsync(Attendee attendee ,string eventId);
}
