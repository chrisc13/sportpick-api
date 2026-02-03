using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using sportpick_bll.Location;


[ApiController]
[Route("api/[controller]")]
public class LocationController : ControllerBase
{
    private readonly ILocationService _locationService;

    public LocationController(ILocationService locationService)
    {
        _locationService = locationService;
    }

    /* Forward geocoding: address → lat/lng
    [HttpGet("geocode")]
    public async Task<IActionResult> Geocode([FromQuery] string address)
    {
        if (string.IsNullOrWhiteSpace(address))
            return BadRequest(new { error = "Missing address" });

        var client = _httpClientFactory.CreateClient();

        var url = $"https://us1.locationiq.com/v1/search.php?key={_locationIqKey}&q={Uri.EscapeDataString(address)}&format=json";

        var response = await client.GetAsync(url);
        if (!response.IsSuccessStatusCode)
            return StatusCode(500, new { error = "Geocoding failed" });

        var content = await response.Content.ReadAsStringAsync();
        var results = JsonSerializer.Deserialize<LocationIQResult[]>(content);

        if (results == null || results.Length == 0)
            return NotFound(new { error = "No results found" });

        return Ok(new
        {
            lat = results[0].Lat,
            lng = results[0].Lon,
            displayName = results[0].DisplayName
        });
    }*/

    [HttpPost("geocode2")]
    public async Task<IActionResult> Geocode2([FromBody] GeocodeObject geocodeRequest)
    {
        if (string.IsNullOrWhiteSpace(geocodeRequest.DisplayName) || geocodeRequest.Latitude == 0.0 || geocodeRequest.Longitude == 0.0){
			return BadRequest(new { error = "Missing search address or user coordinates" });
		}
		GeocodeObject? geocodeObject = await _locationService.GetCoordinatesForSearchAsync(geocodeRequest);
		if (geocodeObject == null){
			return NotFound();
		}
		return Ok(geocodeObject);
    }
    
}


public class LocationIQResult
{
    [JsonPropertyName("lat")]
    public string Lat { get; set; }

    [JsonPropertyName("lon")]
    public string Lon { get; set; }

    [JsonPropertyName("display_name")]
    public string DisplayName { get; set; }
}
