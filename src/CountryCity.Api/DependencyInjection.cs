using CountryCity.Api.Middleware;
using CountryCity.Application.Abstractions;
using CountryCity.Application.Services;
using CountryCity.Infrastructure.Persistence;
using CountryCity.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CountryCity;

public static class DependencyInjection
{
    public static IServiceCollection InjectDependencies(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<AppDbContext>(opt =>
            opt.UseSqlServer(config.GetConnectionString("DefaultConnection")));

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<ICountryRepository, CountryRepository>();
        services.AddScoped<ICityRepository, CityRepository>();
        services.AddScoped<IUserRepository, UserRepository>();

        services.AddScoped<CountryService>();
        services.AddScoped<CityService>();

        services.AddTransient<ExceptionHandling>();
        return services;
    }
}
