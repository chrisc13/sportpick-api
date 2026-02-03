using sportpick_domain;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using System.Net.Http;
using sportpick_dal.External;

namespace sportpick_dal.External;
public class LocationProvider: ILocationProvider {
	private readonly HttpClient _httpClient;
	private readonly string _locationIqKey;
	
    public LocationProvider(HttpClient httpClient, IConfiguration config)
    {
        _httpClient = httpClient;
        _locationIqKey = config["AccessKeys:LocationAPIKey"];
    }

	public async Task<GeocodeObject?> GetCoordinatesForSearchAsync(GeocodeObject geocodeRequest){
        if (geocodeRequest == null || string.IsNullOrWhiteSpace(geocodeRequest.DisplayName))
            return null;

        try
        {
            const double milesToDegrees = 1.0 / 69.0 * 50.0; 

            double minLat = geocodeRequest.Latitude - milesToDegrees;
            double maxLat = geocodeRequest.Latitude + milesToDegrees;
            double minLon = geocodeRequest.Longitude - milesToDegrees;
            double maxLon = geocodeRequest.Longitude + milesToDegrees;

            var url = $"https://us1.locationiq.com/v1/search.php?key={_locationIqKey}" +
                    $"&q={Uri.EscapeDataString(geocodeRequest.DisplayName)}" +
                    $"&format=json" +
                    $"&viewbox={minLon},{maxLat},{maxLon},{minLat}" +
                    $"&bounded=1" +
                    $"&addressdetails=1";

            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                return null;

            var content = await response.Content.ReadAsStringAsync();

            GeocodeResponseObject[]? results;
            try
            {
                results = JsonSerializer.Deserialize<GeocodeResponseObject[]>(content);
            }
            catch (JsonException)
            {
                return null;
            }

            if (results == null || results.Length == 0)
                return null;

            var first = results.FirstOrDefault(res => res != null);

            if (first == null) return null;

            var geocodeObject = new GeocodeObject
            {
                Latitude = first.Latitude,
                Longitude = first.Longitude,
                DisplayName = first.DisplayName
            };

            return geocodeObject;
        }
        catch (HttpRequestException)
        {
            return null;
        }
    }

}
