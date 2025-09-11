namespace ChildrenMoviesApi.Models;

public class Movie
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Image { get; set; }
    public string Description { get; set; }
    public int? Year { get; set; }
    public string Type { get; set; }
    public IEnumerable<StreamService> Streams { get; set; }
    public IEnumerable<string> Tags { get; set; }
}