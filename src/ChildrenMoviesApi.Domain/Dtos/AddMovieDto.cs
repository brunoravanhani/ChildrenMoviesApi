using System.Text.Json.Serialization;

namespace ChildrenMoviesApi.Domain.Dtos;

public class AddMovieDto : MovieDto
{
    [JsonIgnore]
    public string? UserId { get; set; } = null!;
}
