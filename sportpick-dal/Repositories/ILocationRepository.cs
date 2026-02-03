using sportpick_domain;

namespace sportpick_dal;
public interface ILocationRepository{
	public Task<GeocodeObject?> GetCoordinatesForSearch(GeocodeObject request); 
}