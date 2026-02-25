namespace ChildrenMoviesApi.Infra.Tmdb.Factory.Interfaces;

internal interface IHttpClientFactory
{
    HttpClient CreateHttpClient();
}
