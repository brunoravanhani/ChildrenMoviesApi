using ChildrenMoviesApi.Domain.Dtos;
using ChildrenMoviesApi.Domain.Entity;

namespace ChildrenMoviesApi.Domain.Interfaces.Gateways;

public interface ISearchMovieGateway
{
    Task<IEnumerable<Movie>> Search(SearchParamsDto request);
}
