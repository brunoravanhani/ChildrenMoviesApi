using Amazon.DynamoDBv2;
using ChildrenMoviesApi.Application.Infra.Interfaces;

namespace ChildrenMoviesApi.Application.Infra;

public class ContextFactory : IDisposable
{
    private readonly IDynamoDbReader _dynamoDbReader;
    public IMovieRepository MovieRepository { get; }

    public ContextFactory()
    {

        var dynamoDBClient = new AmazonDynamoDBClient();
        _dynamoDbReader = new DynamoDBReader(dynamoDBClient);

        MovieRepository = new MovieRepository(_dynamoDbReader);
    }

    private bool disposed = false;

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposed)
        {
            _dynamoDbReader.Dispose();

            disposed = true;
        }
    }

    ~ContextFactory()
    {
        Dispose();
    }
}