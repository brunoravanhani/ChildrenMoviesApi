using Amazon.DynamoDBv2.Model;

namespace ChildrenMoviesApi.Infrastructure.Interfaces;

public interface IDynamoDbReader : IDisposable
{
    Task<List<Dictionary<string, AttributeValue>>> ScanAsync(string tableName);
    Task<Dictionary<string, AttributeValue>> GetItemAsync(string tableName, int id);
    Task<bool> PutItemAsync(string tableName, Dictionary<string, AttributeValue> data);
}