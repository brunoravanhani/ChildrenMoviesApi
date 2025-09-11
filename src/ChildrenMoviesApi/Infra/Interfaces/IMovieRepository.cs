using Amazon.Lambda.Core;
using ChildrenMoviesApi.Models;

namespace ChildrenMoviesApi.Infra.Interfaces;

public interface IMovieRepository
{
    Task<IEnumerable<Movie>> GetAll(ILambdaContext? context = null);
}