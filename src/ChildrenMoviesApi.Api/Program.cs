using ChildrenMoviesApi.Api.ErrorHandling;
using ChildrenMoviesApi.Api.Logging;
using ChildrenMoviesApi.Application;
using ChildrenMoviesApi.Application.Intefaces;
using ChildrenMoviesApi.Application.Services;
using ChildrenMoviesApi.Core.Configuration;
using ChildrenMoviesApi.Infra.Tmdb.Configuration;
using System.Runtime;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);


        var tmdbCredentials = new TmdbCredentials();
        builder.Configuration.GetSection("Tmdb").Bind(tmdbCredentials);

        builder.Services.AddSingleton(tmdbCredentials);

        builder.Services.AddScoped<ChildrenMoviesApi.Core.Logging.ILogger, CustomLogger>();
        
        builder.Services.ApplicationDI();
        builder.Services.TmdbDI();

        builder.Services.AddControllers();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddProblemDetails();
        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(); 
        }

        app.UseHttpsRedirection();

        app.MapControllers();

        app.Run();
    }
}
