using sportpick_domain;

namespace sportpick_dal;

public interface IDropEventRepository{
    List<DropEvent> GetAllDropEventInfo();
    bool CreateEvent(DropEvent newEvent);
    bool AttendEvent(Attendee attendee, string eventId);
}