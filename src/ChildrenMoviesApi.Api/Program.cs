using ChildrenMoviesApi.Api.Logging;
using ChildrenMoviesApi.Application.Intefaces;
using ChildrenMoviesApi.Application.Services;
using ChildrenMoviesApi.Domain.Configuration;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.Configure<AwsCredentials>(builder.Configuration.GetSection("Aws"));

        builder.Services.AddScoped<ChildrenMoviesApi.Application.Logging.ILogger, CustomLogger>();
        builder.Services.AddScoped<IMoviesApplication, MoviesApplication>();

        builder.Services.AddControllers();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

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
