using Amazon.Lambda.Core;
using Amazon.Lambda.Serialization.SystemTextJson;
using ChildrenMoviesApi.Application.Models;
using ChildrenMoviesApi.Application.Logging;
using ChildrenMoviesApi.Application.Intefaces;
using ChildrenMoviesApi.Application.Services;

[assembly: LambdaSerializer(typeof(DefaultLambdaJsonSerializer))]
namespace ChildrenMoviesApi;

public class Function
{

    public async Task<IEnumerable<Movie>> FunctionHandler(ILambdaContext context)
    {
        ILogger logger = new Logging.LambdaLogger(context);
        IMoviesApplication application = new MoviesApplication(logger);

        return await application.QueryMovies();
    }
}
