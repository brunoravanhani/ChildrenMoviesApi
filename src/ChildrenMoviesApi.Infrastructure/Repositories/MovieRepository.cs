using Amazon.DynamoDBv2.Model;
using ChildrenMoviesApi.Domain.Interfaces;
using ChildrenMoviesApi.Infrastructure.Mappers;
using ChildrenMoviesApi.Infrastructure.Interfaces;
using ChildrenMoviesApi.Domain.Entity;

namespace ChildrenMoviesApi.Infrastructure.Repositories;

public class MovieRepository : IMovieRepository
{

    private readonly IDynamoDbReader _dynamoReader;
    private const string TableName = "children-movies-database";

    public MovieRepository(IDynamoDbReader dynamoReader)
    {
        _dynamoReader = dynamoReader;
    }

    public async Task<IEnumerable<Movie>> GetAll()
    {
        List<Dictionary<string, AttributeValue>> items = await _dynamoReader.ScanAsync(TableName);

        var movies = items.Select(MovieDocumentMapper.ToMovie).ToList();

        return movies;
    }

    public async Task<Movie> Get(int id)
    {
        var movieItem = await _dynamoReader.GetItemAsync(TableName, id);

        var movies = MovieDocumentMapper.ToMovie(movieItem);

        return movies;
    }

    public async Task<bool> Save(Movie movie)
    {
        var result = await _dynamoReader.PutItemAsync(TableName, movie.ToDynamoObject());

        return result;
    }
}