using Amazon.DynamoDBv2;
using Amazon.Runtime;
using ChildrenMoviesApi.Application.Infra.Interfaces;
using ChildrenMoviesApi.Domain.Configuration;

namespace ChildrenMoviesApi.Application.Infra;

public class ContextFactory : IDisposable
{
    private readonly IDynamoDbReader _dynamoDbReader;
    public IMovieRepository MovieRepository { get; }

    public ContextFactory(AwsCredentials awsCredentials)
    {

        if (!string.IsNullOrEmpty(awsCredentials.AccessKey)
            && !string.IsNullOrEmpty(awsCredentials.SecretKey))
        {
            var awsBasicCredentials = new BasicAWSCredentials(awsCredentials.AccessKey, awsCredentials.SecretKey);
            var dynamoDBClient = new AmazonDynamoDBClient(awsBasicCredentials, Amazon.RegionEndpoint.USEast1);
            _dynamoDbReader = new DynamoDBReader(dynamoDBClient);
        } 
        else
        {
            _dynamoDbReader = new DynamoDBReader(new AmazonDynamoDBClient());
        }

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