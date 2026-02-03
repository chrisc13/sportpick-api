using sportpick_domain;
using Microsoft.Extensions.Caching.Memory;
using Geohash;
using System.Threading;
using sportpick_dal.External;

namespace sportpick_dal;
public class LocationRepository: ILocationRepository {
	private readonly IMemoryCache _locationCoordinatesCache;
	private readonly ILocationProvider _locationProvider;
	private static SemaphoreSlim _semaphore = new SemaphoreSlim(1,1);
    private readonly Geohasher _geohasher = new Geohasher();

	public LocationRepository(IMemoryCache locationCoordinatesCache, ILocationProvider locationProvider){
		_locationCoordinatesCache = locationCoordinatesCache;
		_locationProvider = locationProvider;
	}

    public async Task<GeocodeObject?> GetCoordinatesForSearch(GeocodeObject geocodeRequest)
    {
        string locationKey = GetLocationKey(geocodeRequest);

        if (_locationCoordinatesCache.TryGetValue(locationKey, out GeocodeObject? cached))
        {
            return cached;
        }

        await _semaphore.WaitAsync();
        try
        {
            if (_locationCoordinatesCache.TryGetValue(locationKey, out cached))
                return cached;

            var geocodeObject = await _locationProvider.GetCoordinatesForSearchAsync(geocodeRequest);
            if (geocodeObject != null)
            {
                var cacheOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1)
                };
                _locationCoordinatesCache.Set(locationKey, geocodeObject, cacheOptions);
            }

            return geocodeObject;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return null;
        }
        finally
        {
            _semaphore.Release();
        }
    }

	private string GetLocationKey(GeocodeObject request){
		string normalizedQuery = request.DisplayName.Trim().ToLower().Replace(" ", "");
		var geoBucket = _geohasher.Encode(request.Latitude, request.Longitude, 7); 
		var cacheKey = $"geo:{geoBucket}:{normalizedQuery}";
		return cacheKey;
	}
}