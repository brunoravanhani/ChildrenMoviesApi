using Amazon.DynamoDBv2.Model;

namespace ChildrenMoviesApi.Application.Infra.Interfaces;

public interface IDynamoDbReader : IDisposable
{
    Task<List<Dictionary<string, AttributeValue>>> ScanAsync(string tableName);
}