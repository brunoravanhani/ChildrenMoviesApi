using ChildrenMoviesApi.Application.Models;

namespace ChildrenMoviesApi.Application.Infra.Interfaces;

public interface IMovieRepository
{
    Task<IEnumerable<Movie>> GetAll();
}