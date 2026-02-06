using CountryCity.Application.Abstractions;
using CountryCity.Application.Auth;
using CountryCity.Domain.Auth;
using CountryCity.Infrastructure.Persistence;

namespace CountryCity.Api.Tools
{
    public static class Utilities
    {
        static public async Task SeedAdminUserAsync(WebApplication app)
        {
            using var scope = app.Services.CreateScope();

            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            await db.Database.EnsureCreatedAsync();

            var users = scope.ServiceProvider.GetRequiredService<IUserRepository>();
            var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

            if (!await users.AnyUsersAsync())
            {
                var (hash, salt) = PasswordHasher.HashPassword("123456");
                await users.AddAsync(new User(Guid.NewGuid(), "admin", hash, salt));
                await uow.SaveChangesAsync();
            }
        }
    }
}
