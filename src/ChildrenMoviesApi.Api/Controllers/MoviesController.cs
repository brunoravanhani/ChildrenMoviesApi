using ChildrenMoviesApi.Application.Intefaces;
using ChildrenMoviesApi.Domain.Dtos;
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

    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] SearchParamsDto dto)
    {
        var movies = await _moviesApplication.Search(dto);

        return Ok(movies);
    } 
    
}