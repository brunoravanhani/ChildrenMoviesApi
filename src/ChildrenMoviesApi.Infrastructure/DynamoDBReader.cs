using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using ChildrenMoviesApi.Infrastructure.Interfaces;

namespace ChildrenMoviesApi.Application.Infra;

public class DynamoDBReader : IDynamoDbReader
{
    private const string PartitionKeyDefault = "id";

    private bool disposed = false;
    private readonly IAmazonDynamoDB _dynamoDBClient;

    public DynamoDBReader(IAmazonDynamoDB dynamoDBClient)
    {
        _dynamoDBClient = dynamoDBClient;
    }

    public async Task<List<Dictionary<string, AttributeValue>>> ScanAsync(string tableName)
    {
        var scanRequest = new ScanRequest
        {
            TableName = tableName
        };

        var response = await _dynamoDBClient.ScanAsync(scanRequest);

        return response.Items;
    }

    public async Task<Dictionary<string, AttributeValue>> GetItemAsync(string tableName, int id)
    {
        var key = new Dictionary<string, AttributeValue>
        {
            { PartitionKeyDefault, new AttributeValue { S = id.ToString() } }
        };

        var request = new GetItemRequest
        {
            TableName = tableName,
            Key = key,
            ConsistentRead = false
        };

        var response = await _dynamoDBClient.GetItemAsync(request);

        return response.Item;
    }
    
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposed)
        {
            _dynamoDBClient.Dispose();

            disposed = true;
        }
    }

    ~DynamoDBReader()
    {
        Dispose();
    }

}