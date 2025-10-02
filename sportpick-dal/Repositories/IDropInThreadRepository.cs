using sportpick_domain;

namespace sportpick_dal;

public interface IDropInThreadRepository{
    Task<List<DropInThread>> GetFifteenDropInThreadAsync();
    Task<bool> CreateDropInThreadAsync(DropInThread newEvent);
}