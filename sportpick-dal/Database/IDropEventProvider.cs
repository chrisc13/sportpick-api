using MongoDB.Driver;
using sportpick_domain;

namespace sportpick_dal;

public interface IDropEventProvider
{
    List<DropEventEntity> GetAllDropEventInfo();
    bool CreateEvent(DropEventEntity newEvent);
}
