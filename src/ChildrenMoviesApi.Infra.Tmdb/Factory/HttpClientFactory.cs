using ChildrenMoviesApi.Core.Configuration;
using ChildrenMoviesApi.Infra.Tmdb.Factory.Interfaces;
using System.Net.Http.Headers;

namespace ChildrenMoviesApi.Infra.Tmdb.Factory;

internal class HttpClientFactory : IHttpClientFactory
{

    private readonly TmdbCredentials _tmdbCredentials;

    public HttpClientFactory(TmdbCredentials tmdbCredentials)
    {
        _tmdbCredentials = tmdbCredentials;
    }

    public HttpClient CreateHttpClient()
    {
        HttpClient client = new HttpClient();

        client.BaseAddress = new Uri(_tmdbCredentials.ApiBaseUrl);
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tmdbCredentials.Token);

        return client;
    }
}
