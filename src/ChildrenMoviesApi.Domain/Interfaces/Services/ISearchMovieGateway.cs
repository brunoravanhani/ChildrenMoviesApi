using ChildrenMoviesApi.Domain.Dtos;
using ChildrenMoviesApi.Domain.Entity;

namespace ChildrenMoviesApi.Domain.Interfaces.Services;

public interface ISearchMovieGateway
{
    Task<IEnumerable<Movie>> Search(SearchParamsDto request);
}
