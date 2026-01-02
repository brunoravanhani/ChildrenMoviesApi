using Amazon.DynamoDBv2.Model;
using ChildrenMoviesApi.Domain.Interfaces;
using ChildrenMoviesApi.Infrastructure.Mappers;
using ChildrenMoviesApi.Infrastructure.Interfaces;
using ChildrenMoviesApi.Domain.Entity;
using ChildrenMoviesApi.Domain.Configuration;

namespace ChildrenMoviesApi.Infrastructure.Repositories;

public class MovieRepository : IMovieRepository
{

    private readonly IDynamoDbReader _dynamoReader;
    private readonly string TableName;

    public MovieRepository(IDynamoDbReader dynamoReader, DatabaseTables databaseTables)
    {
        _dynamoReader = dynamoReader;
        TableName = databaseTables.Movie;   
    }

    public async Task<IEnumerable<Movie>> GetAll()
    {
        List<Dictionary<string, AttributeValue>> items = await _dynamoReader.ScanAsync(TableName);

        var movies = items.Select(MovieDocumentMapper.ToMovie).ToList();

        return movies;
    }

    public async Task<Movie> Get(Guid id)
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