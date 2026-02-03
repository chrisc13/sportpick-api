using sportpick_domain;
using sportpick_dal;

namespace sportpick_bll.Location;
public class LocationService: ILocationService {
	private readonly ILocationRepository _locationRepo;
	
	public LocationService(ILocationRepository locationRepo){
		_locationRepo = locationRepo;
	}

	public async Task<GeocodeObject?> GetCoordinatesForSearchAsync(GeocodeObject request){
		return await _locationRepo.GetCoordinatesForSearch(request);
	} 
}
