using ChildrenMoviesApi.Domain.Dtos;
using ChildrenMoviesApi.Domain.Entity;

namespace ChildrenMoviesApi.Application.Intefaces;

public interface IMoviesApplication
{
    Task<IEnumerable<Movie>> Search(SearchParamsDto searchParams);
}