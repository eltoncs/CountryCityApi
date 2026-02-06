using CountryCity;
using CountryCity.Api.Extensions;
using CountryCity.Api.Middleware;
using CountryCity.Api.Tools;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.AddJWTAuthenticationService();
builder.AddSwaggerGenService();

builder.Services.InjectDependencies(builder.Configuration);

var app = builder.Build();

app.UseMiddleware<ExceptionHandling>();

//Add admin user to the db
await Utilities.SeedAdminUserAsync(app);

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();
app.MapGet("/", () => Results.Redirect("/swagger"));
app.MapControllers();

app.Run();
