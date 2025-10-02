using sportpick_domain;
public interface IDropInThreadService{
    Task<List<DropInThread>> GetFifteenDropInThreadAsync();
    Task<bool> CreateDropInThreadAsync(DropInThread newEvent);
}
