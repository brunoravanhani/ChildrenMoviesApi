using ChildrenMoviesApi.Domain.Interfaces;
using ChildrenMoviesApi.Infra.MySQL.Data;
using ChildrenMoviesApi.Infra.MySQL.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ChildrenMoviesApi.Infra.MySQL;

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
