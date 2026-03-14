namespace ChildrenMoviesApi.Domain.Dtos;

public class MovieDto
{
    public required int Id { get; set; }
    public required string Title { get; set; }
    public string? OriginalTitle { get; set; }
    public string? Overview { get; set; }
    public string? PosterPath { get; set; }
    public string? BackdropPath { get; set; }
    public string? OriginalLanguage { get; set; }
    public DateTime? ReleaseDate { get; set; }
}
