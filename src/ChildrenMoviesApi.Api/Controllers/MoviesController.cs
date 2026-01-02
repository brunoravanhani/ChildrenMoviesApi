using ChildrenMoviesApi.Application.Intefaces;
using ChildrenMoviesApi.Domain.Entity;
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
    public async Task<IActionResult> Get(Guid id)
    {
        var movie = await _moviesApplication.GetMovie(id);

        return Ok(movie);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Movie movie)
    {
        await _moviesApplication.SaveMovie(movie);

        return Ok($"Movie {movie.Name} added successfully");
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(Guid id, [FromBody] Movie movie)
    {
        await _moviesApplication.UpdateMovie(id, movie);

        return Ok($"Movie {movie.Name} added successfully");
    } 
}