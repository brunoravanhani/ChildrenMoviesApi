using ChildrenMoviesApi.Application.Intefaces;
using ChildrenMoviesApi.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChildrenMoviesApi.Application;

public static class Initialize
{
    public static IServiceCollection ApplicationDI(this IServiceCollection services)
    {
        services.AddScoped<IMoviesApplication, MoviesApplication>();

        return services;
    }
}
