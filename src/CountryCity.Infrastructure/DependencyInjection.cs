using CountryCity.Application.Abstractions;
using CountryCity.Application.Services;
using CountryCity.Infrastructure.Persistence;
using CountryCity.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CountryCity.Infrastructure;

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

        return services;
    }
}
