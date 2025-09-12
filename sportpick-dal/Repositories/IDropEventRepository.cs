using sportpick_domain;

namespace sportpick_dal;

public interface IDropEventRepository{
    Task<List<DropEvent>> GetAllDropEventInfoAsync();
    Task<bool> CreateEventAsync(DropEvent newEvent);
    Task<bool> AttendEventAsync(Attendee attendee, string eventId);
}