using ChildrenMoviesApi.Api.Configuration;
using ChildrenMoviesApi.Api.ErrorHandling;
using ChildrenMoviesApi.Api.Logging;
using ChildrenMoviesApi.Application;
using ChildrenMoviesApi.Infra.Google.Configuration;
using ChildrenMoviesApi.Infra.Tmdb.Configuration;
using ChildrenMoviesApi.Infrastructure;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddApiConfiguration(builder.Configuration);

        builder.Services.AddInfrastructure(builder.Configuration);

        builder.Services.AddScoped<ChildrenMoviesApi.Core.Logging.ILogger, CustomLogger>();

        builder.Services.ApplicationDI();
        builder.Services.TmdbDI();
        builder.Services.GoogleAuthDI();

        // CORS
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowMoviePoints", policy =>
            {
                policy.WithOrigins("localhost:5173", "http://localhost:5173", "https://localhost:5173")
                      .AllowAnyHeader()
                      .AllowAnyMethod();
            });
        });

        builder.Services.AddControllers();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddProblemDetails();
        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

        builder.Services.AddBearerAuthentication(builder.Configuration);

        builder.Services.AddAuthorization();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseCors("AllowMoviePoints");

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
