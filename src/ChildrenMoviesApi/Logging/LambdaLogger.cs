using Amazon.Lambda.Core;
using ChildrenMoviesApi.Application.Logging;

namespace ChildrenMoviesApi.Logging;

public class LambdaLogger : ILogger
{

    private readonly ILambdaContext _lambdaContext;

    public LambdaLogger(ILambdaContext lambdaContext)
    {
        _lambdaContext = lambdaContext;
    }

    public void LogInformation(string message)
    {
        _lambdaContext.Logger.LogInformation(message);
    }

    public void LogWarning(Exception e, string message)
    {
        _lambdaContext.Logger.LogWarning(e, message);
    }
}