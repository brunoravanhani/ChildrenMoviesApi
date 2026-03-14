
namespace ChildrenMoviesApi.Domain.Entity;

public class Movie
{
    public int Id { get; private set; }
    public string? BackdropPath { get; private set; }
    public string? PosterPath { get; private set; }
    public string? OriginalLanguage { get; private set; }
    public string? OriginalTitle { get; private set; }
    public string? Overview { get; private set; }
    public DateTime? ReleaseDate { get; private set; }
    public string? Title { get; private set; }

    protected Movie() { }

    public Movie(
        int id,
        string? backdropPath,
        string? posterPath,
        string? originalLanguage,
        string? originalTitle,
        string? overview,
        string? releaseDate,
        string? title)
    {
        Id = id;
        BackdropPath = backdropPath;
        PosterPath = posterPath;
        OriginalLanguage = originalLanguage;
        OriginalTitle = originalTitle;
        Overview = overview;
        ReleaseDate = string.IsNullOrWhiteSpace(releaseDate) ? null : DateTime.Parse(releaseDate!);
        Title = title;
    }
}