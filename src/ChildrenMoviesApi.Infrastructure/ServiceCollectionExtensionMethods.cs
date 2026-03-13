using ChildrenMoviesApi.Domain.Interfaces;
using ChildrenMoviesApi.Infrastructure.Data;
using ChildrenMoviesApi.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ChildrenMoviesApi.Infrastructure;

public static class ServiceCollectionExtensionMethods
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ChildrenMoviesDbContext>(options =>
            options.UseMySql(
                configuration.GetConnectionString("DefaultConnection"),
                ServerVersion.AutoDetect(configuration.GetConnectionString("DefaultConnection"))));
        
        services.AddScoped<IMovieRepository, MovieRepository>();
        
        return services;
    }
}
