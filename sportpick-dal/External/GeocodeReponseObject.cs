using System.Text.Json.Serialization;

public class GeocodeResponseObject
{
    [JsonPropertyName("lat")]
    public string Lat { get; set; } = string.Empty;

    [JsonPropertyName("lon")]
    public string Lon { get; set; } = string.Empty;

    [JsonPropertyName("display_name")]
    public string DisplayName { get; set; } = string.Empty;

    // Convenience properties to convert string to double
    [JsonIgnore]
    public double Latitude => double.TryParse(Lat, out var d) ? d : 0.0;

    [JsonIgnore]
    public double Longitude => double.TryParse(Lon, out var d) ? d : 0.0;
}
