using sportpick_domain;

namespace sportpick_bll.Location;
public interface ILocationService{
	public Task<GeocodeObject?> GetCoordinatesForSearchAsync(GeocodeObject request); 
}