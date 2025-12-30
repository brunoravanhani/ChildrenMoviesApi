using Amazon.DynamoDBv2.Model;

namespace ChildrenMoviesApi.Infrastructure.Interfaces;

public interface IDynamoDbReader : IDisposable
{
    Task<List<Dictionary<string, AttributeValue>>> ScanAsync(string tableName);
}