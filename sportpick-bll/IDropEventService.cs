using sportpick_domain;
namespace sportpick_bll;

public interface IDropEventService
{
    List<DropEvent> GetAllDropEvents();  
    List<DropEvent> GetTopThreePopular();
    bool CreateEvent(DropEvent newEvent);
    bool AttendEvent(string eventId, string username);
}
