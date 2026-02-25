using ChildrenMoviesApi.Core.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ChildrenMoviesApi.Api.ErrorHandling;

public sealed class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "Unhandled exception occurred");

        var problem = exception switch
        {
            NotFoundException ex => CreateProblemDetails(
                httpContext,
                HttpStatusCode.NotFound,
                ex.Message),

            InvalidRequetException ex => CreateProblemDetails(
                httpContext,
                HttpStatusCode.BadRequest,
                ex.Message),

            _ => CreateProblemDetails(
                httpContext,
                HttpStatusCode.InternalServerError,
                "An unexpected error occurred.")
        };

        httpContext.Response.StatusCode = problem.Status!.Value;

        await httpContext.Response.WriteAsJsonAsync(problem, cancellationToken);

        return true;
    }

    private static ProblemDetails CreateProblemDetails(
        HttpContext context,
        HttpStatusCode statusCode,
        string message)
    {
        return new ProblemDetails
        {
            Status = (int)statusCode,
            Title = message,
            Instance = context.Request.Path,
            Type = $"https://httpstatuses.com/{(int)statusCode}"
        };
    }
}
