using Amazon.DynamoDBv2.Model;

namespace ChildrenMoviesApi.Infra.Interfaces;

public interface IDynamoDbReader : IDisposable
{
    Task<List<Dictionary<string, AttributeValue>>> ScanAsync(string tableName);
}