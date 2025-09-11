using Amazon.Lambda.Core;
using Amazon.Lambda.Serialization.SystemTextJson;
using ChildrenMoviesApi.Models;
using ChildrenMoviesApi.Infra;
using Amazon.DynamoDBv2;
using Amazon;
using Amazon.DynamoDBv2.Model;

[assembly: LambdaSerializer(typeof(DefaultLambdaJsonSerializer))]
namespace ChildrenMoviesApi;

public class Function
{

    public async Task<IEnumerable<Movie>> FunctionHandler(ILambdaContext context)
    {
        try
        {
            // context?.Logger.LogInformation($"Setting up");
            // AmazonDynamoDBConfig clientConfig = new AmazonDynamoDBConfig();
            // clientConfig.RegionEndpoint = RegionEndpoint.USEast1;
            // var client = new AmazonDynamoDBClient(clientConfig);

            // context?.Logger.LogInformation($"Create Request");
            // var scanRequest = new ScanRequest
            // {
            //     TableName = "children-movies-database"
            // };

            // context?.Logger.LogInformation($"Init Scan");

            // var response = await client.ScanAsync(scanRequest);

            // context?.Logger.LogInformation($"Finished Scan");

            // return $"Query result: {response.Count} - Status Code: {response.HttpStatusCode}";

            context?.Logger.LogInformation("Setting up context");

            using var dbContext = new ContextFactory(context);

            context?.Logger.LogInformation("Querying data");

            var movies = await dbContext.MovieRepository.GetAll(context);

            context?.Logger.LogInformation($"Found {movies.Count()} items");

            return movies;
        }
        catch (Exception e)
        {
            context.Logger.LogWarning(e, "Error executing query");
            return [];
        }

    }
}
