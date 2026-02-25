using Amazon.DynamoDBv2.Model;
using ChildrenMoviesApi.Domain.Interfaces;
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

   
}