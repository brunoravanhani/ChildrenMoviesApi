namespace ChildrenMoviesApi.Core.Logging;

public interface ILogger
{
    void LogInformation(string message);
    void LogWarning(Exception e, string message);
}