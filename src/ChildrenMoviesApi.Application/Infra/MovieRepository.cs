using Amazon.DynamoDBv2.Model;
using ChildrenMoviesApi.Application.Infra.Interfaces;
using ChildrenMoviesApi.Application.Mappers;
using ChildrenMoviesApi.Application.Models;

namespace ChildrenMoviesApi.Application.Infra;

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
}