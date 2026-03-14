using ChildrenMoviesApi.Application.Intefaces;
using ChildrenMoviesApi.Domain.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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

    [HttpGet()]
    public async Task<IActionResult> Get()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized("User ID not found in token");
        }

        var movies = await _moviesApplication.GetAllByUser(userId);

        return Ok(movies);
    }

    [HttpPost]
    public async Task<IActionResult> AddMovie([FromBody] AddMovieDto addMovieDto)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized("User ID not found in token");
        }

        addMovieDto.UserId = userId;
        await _moviesApplication.AddMovieAsync(addMovieDto);
        return Ok();
    }

}