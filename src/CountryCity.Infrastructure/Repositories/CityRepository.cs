using CountryCity.Application.Abstractions;
using CountryCity.Domain.Countries;
using CountryCity.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CountryCity.Infrastructure.Repositories;

public sealed class CityRepository : ICityRepository
{
    private readonly AppDbContext _db;
    public CityRepository(AppDbContext db) => _db = db;

    public Task<bool> ExistsAsync(
        string countryId, 
        string cityId, 
        CancellationToken ct = default)
    {
        return _db.Cities.AnyAsync(c => c.CountryId == countryId.Trim().ToUpper() && c.CityId == cityId, ct);
    }

    public Task AddAsync(
        City city, 
        CancellationToken ct = default)
    {
        return _db.Cities.AddAsync(city, ct).AsTask();
    }        

    public void Remove(City city)
    {
        _db.Cities.Remove(city);
    }

    public Task<City?> GetByIdAsync(
        string rawCountryId, 
        string rawCityId, 
        CancellationToken ct = default)
    {
        string? cityId = rawCityId.Trim().ToUpper();
        string? countryId = rawCountryId.Trim().ToUpper();

        return _db.Cities.SingleOrDefaultAsync(c => c.CountryId == countryId && c.CityId == cityId, ct);
    }

    public Task<List<City>> GetByCountryAsync(
        string rawCountryId, 
        CancellationToken ct = default)
    {
        string? countryId = rawCountryId.Trim().ToUpper();

        return _db.Cities
            .Where(c => c.CountryId == countryId)
            .OrderBy(c => c.CityName)
            .ToListAsync(ct);
    }
}
