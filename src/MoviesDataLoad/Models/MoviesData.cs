using System.Text.Json.Serialization;

namespace MoviesDataLoad.Models;

public class MoviesData
{
    [JsonPropertyName("data")]
    public IEnumerable<Movie> Data { get; set; }
}