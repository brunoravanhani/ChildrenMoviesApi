using ChildrenMoviesApi.Core.Configuration;
using ChildrenMoviesApi.Core.Exceptions;
using ChildrenMoviesApi.Core.Logging;
using ChildrenMoviesApi.Domain.Dtos;
using ChildrenMoviesApi.Domain.Entity;
using ChildrenMoviesApi.Domain.Interfaces.Services;
using ChildrenMoviesApi.Infra.Tmdb.Factory.Interfaces;
using ChildrenMoviesApi.Infra.Tmdb.Models;
using System.Text;
using System.Text.Json;

namespace ChildrenMoviesApi.Infra.Tmdb.Services;

internal class SearchMovieGateway : ISearchMovieGateway
{
    private readonly string className = nameof(SearchMovieGateway);
    private readonly ILogger _logger;
    private readonly HttpClient _client;

    public SearchMovieGateway(ILogger logger, IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _client = httpClientFactory.CreateHttpClient();
    }

    public async Task<IEnumerable<Movie>> Search(SearchParamsDto request)
    {
        string methodName = nameof(Search);

        string logMessage = $"Class: {className} | Method: {methodName} | Request: {JsonSerializer.Serialize(request, JsonDefaults.Options)}";

        _logger.LogInformation(logMessage);

        try
        {
            using var response = await _client.GetAsync(CreateSearchURL(request));

            response.EnsureSuccessStatusCode();
            var body = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<PaginatedResponseDto<MovieDto>>(body, JsonDefaults.Options);
            return MapMovie(result);
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                _logger.LogWarning(ex, $"Class: {className} | Method: {methodName} | Movie not found {request.Query}");
                throw new NotFoundException();
            } 

            _logger.LogWarning(ex, $"Class: {className} | Method: {methodName} | Response status code does not indicate success.");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, $"Class: {className} | Method: {methodName} | Error: {ex.Message}");
            throw;
        }
        finally
        {
            _logger.LogInformation($"Class: {className} | Method: {methodName} | Finished");
        }
    }

    private static IEnumerable<Movie> MapMovie(PaginatedResponseDto<MovieDto>? result)
    {
        return result.Results.Select(x =>
        {
            return new Movie(
                x.Id,
                x.BackdropPath,
                x.PosterPath,
                x.OriginalLanguage,
                x.OriginalTitle,
                x.Overview,
                x.ReleaseDate,
                x.Title
            );
        });
    }

    private static string CreateSearchURL(SearchParamsDto request)
    {
        StringBuilder sb = new StringBuilder("search/movie?");
        sb.Append("include_adult=false&language=pt-BR&page=1");
        sb.Append("&query=");
        sb.Append(request.Query);

        return sb.ToString();
    }
}
