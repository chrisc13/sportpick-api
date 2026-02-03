using sportpick_domain;

namespace sportpick_dal.External;

public interface ILocationProvider {
	public Task<GeocodeObject?> GetCoordinatesForSearchAsync(GeocodeObject geocodeObject);
}
