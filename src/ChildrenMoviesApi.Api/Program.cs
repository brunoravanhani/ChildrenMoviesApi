using ChildrenMoviesApi.Api.Logging;
using ChildrenMoviesApi.Application.Intefaces;
using ChildrenMoviesApi.Application.Services;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        builder.Services.AddScoped<ChildrenMoviesApi.Application.Logging.ILogger, CustomLogger>();
        builder.Services.AddScoped<IMoviesApplication, MoviesApplication>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();
        
        app.MapGet("/", async (IMoviesApplication application) =>
        {
            return await application.QueryMovies();
        })
        .WithName("GetMovies");

        app.Run();
    }
}
