using MongoDB.Driver;
using sportpick_domain;

namespace sportpick_dal;

public interface IDropInThreadProvider
{
    Task<List<DropInThreadEntity>> GetFifteenDropInThreadAsync();
    Task<DropInThreadEntity> GetDropInThreadByIdAsync(string id);
    Task<bool> CreateDropInThreadAsync(DropInThreadEntity newEvent);
}
