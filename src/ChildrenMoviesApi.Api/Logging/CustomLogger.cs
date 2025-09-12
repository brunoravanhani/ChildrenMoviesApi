using ChildrenMoviesApi.Application.Logging;

namespace ChildrenMoviesApi.Api.Logging;

public class CustomLogger : Application.Logging.ILogger
{
    public void LogInformation(string message)
    {
        Console.WriteLine(message);
    }

    public void LogWarning(Exception e, string message)
    {
        Console.WriteLine(message);
        Console.WriteLine(e);
    }
}