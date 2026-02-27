using ChildrenMoviesApi.Domain.Dtos;
using ChildrenMoviesApi.Domain.Entity;

namespace ChildrenMoviesApi.Application.Intefaces;

public interface IMoviesService
{
    Task<IEnumerable<Movie>> Search(SearchParamsDto searchParams);
}