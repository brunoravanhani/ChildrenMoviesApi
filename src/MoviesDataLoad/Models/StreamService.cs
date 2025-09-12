using System.Text.Json.Serialization;

namespace MoviesDataLoad.Models;

public class StreamService
{
    [JsonPropertyName("name")]
    public string Name { get; set; }
    [JsonPropertyName("link")]
    public string Link { get; set; }
}