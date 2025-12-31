using ChildrenMoviesApi.Application.Intefaces;
using Microsoft.AspNetCore.Mvc;

namespace ChildrenMoviesApi.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class MoviesController : ControllerBase
{
    
    private readonly IMoviesApplication _moviesApplication;

    public MoviesController(IMoviesApplication moviesApplication)
    {
        _moviesApplication = moviesApplication;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var movies = await _moviesApplication.QueryMovies();

        return Ok(movies);
    } 

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var movie = await _moviesApplication.GetMovie(id);

        return Ok(movie);
    } 
}