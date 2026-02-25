using System.Text.Json.Serialization;

namespace ChildrenMoviesApi.Infra.Tmdb.Models;

public class PaginatedResponseDto<T>
{
    public int Page { get; set; }
    public IEnumerable<T> Results { get; set; }
    [JsonPropertyName("total_pages")]
    public int TotalPages { get; set; }
    [JsonPropertyName("total_results")]
    public int TotalResults { get; set; }

}
