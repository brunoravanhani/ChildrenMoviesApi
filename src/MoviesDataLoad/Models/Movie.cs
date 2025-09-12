using System.Text.Json.Serialization;

namespace MoviesDataLoad.Models;

public class Movie
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("name")]
    public string Name { get; set; }
    [JsonPropertyName("image")]
    public string Image { get; set; }
    [JsonPropertyName("description")]
    public string Description { get; set; }
    [JsonPropertyName("year")]
    public int? Year { get; set; }
    [JsonPropertyName("type")]
    public string Type { get; set; }
    [JsonPropertyName("streams")]
    public IEnumerable<StreamService> Streams { get; set; }
    [JsonPropertyName("tags")]
    public IEnumerable<string> Tags { get; set; }
}