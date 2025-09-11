using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Amazon.Lambda.Core;
using ChildrenMoviesApi.Infra.Interfaces;
using ChildrenMoviesApi.Models;

namespace ChildrenMoviesApi.Infra;

public class MovieRepository : IMovieRepository
{

    private readonly IDynamoDbReader _dynamoReader;
    private const string TableName = "children-movies-database";

    public MovieRepository(IDynamoDbReader dynamoReader)
    {

        _dynamoReader = dynamoReader;
    }

    public async Task<IEnumerable<Movie>> GetAll(ILambdaContext? context = null)
    {
        context?.Logger.LogInformation($"Scaning table {TableName}");

        List<Dictionary<string, AttributeValue>> items = await _dynamoReader.ScanAsync(TableName);

        context?.Logger.LogInformation($"Mapping object Movie");

        var movies = items.Select(MovieDocumentMapper.ToMovie).ToList();

        context?.Logger.LogInformation($"Mapping completed");

        return movies;
    }
}