using ChildrenMoviesApi.Application.Intefaces;
using ChildrenMoviesApi.Domain.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChildrenMoviesApi.Api.Controllers;

[Authorize]
[Route("[controller]")]
[ApiController]
public class MoviesController : ControllerBase
{
    
    private readonly IMoviesService _moviesApplication;

    public MoviesController(IMoviesService moviesApplication)
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