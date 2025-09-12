using Amazon.Lambda.Core;
using Amazon.Lambda.Serialization.SystemTextJson;
using ChildrenMoviesApi.Models;
using ChildrenMoviesApi.Infra;

[assembly: LambdaSerializer(typeof(DefaultLambdaJsonSerializer))]
namespace ChildrenMoviesApi;

public class Function
{

    public async Task<IEnumerable<Movie>> FunctionHandler(ILambdaContext context)
    {
        try
        {
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
