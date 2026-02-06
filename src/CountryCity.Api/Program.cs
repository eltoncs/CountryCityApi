using CountryCity;
using CountryCity.Api.Extensions;
using CountryCity.Api.Middleware;
using CountryCity.Application.Abstractions;
using CountryCity.Application.Auth;
using CountryCity.Domain.Auth;
using CountryCity.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.AddJWTAuthenticationService();
builder.AddSwaggerGenService();

builder.Services.InjectDependencies(builder.Configuration);

var app = builder.Build();

app.UseMiddleware<ExceptionHandling>();

await SeedAdminUserAsync(app);

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();
app.MapGet("/", () => Results.Redirect("/swagger"));
app.MapControllers();

app.Run();

static async Task SeedAdminUserAsync(WebApplication app)
{
    using var scope = app.Services.CreateScope();

    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.EnsureCreatedAsync(); // or MigrateAsync

    var users = scope.ServiceProvider.GetRequiredService<IUserRepository>();
    var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

    if (!await users.AnyUsersAsync())
    {
        var (hash, salt) = PasswordHasher.HashPassword("123456");
        await users.AddAsync(new User(Guid.NewGuid(), "admin", hash, salt));
        await uow.SaveChangesAsync();
    }
}
