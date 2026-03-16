namespace ChildrenMoviesApi.Domain.Entity;

public class UserMovie : EntityBase
{
    public string UserId { get; private set; }
    public int MovieId { get; private set; }
    public int Points { get; private set; }
    public DateTime CreatedDate { get; private set; }
    public Movie Movie { get; private set; }

    protected UserMovie()
    {

    }

    public static UserMovie Create(string userId, Movie movie, int points = 1)
    {
        return new UserMovie
        {
            UserId = userId,
            MovieId = movie.Id,
            Movie = movie,
            Points = points,
            CreatedDate = DateTime.UtcNow
        };
    }

    public void UpdatePoints(int points)
    {
        Points = points;
    }

}
