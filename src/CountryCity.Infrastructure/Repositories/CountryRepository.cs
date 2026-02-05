using CountryCity.Application.Abstractions;
using CountryCity.Domain.Countries;
using CountryCity.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CountryCity.Infrastructure.Repositories;

public sealed class CountryRepository : ICountryRepository
{
    private readonly AppDbContext _db;
    public CountryRepository(AppDbContext db) => _db = db;

    public Task<bool> ExistsAsync(
        string countryId, 
        CancellationToken ct = default)
    {
        return _db.Countries.AnyAsync(c => c.CountryId == countryId.Trim().ToUpper(), ct);
    }

    public Task<List<Country>> GetAll(CancellationToken ct = default)
    {
        return _db.Countries.OrderBy(c => c.CountryName).ToListAsync(ct);
    }

    public Task AddAsync(
        Country country, 
        CancellationToken ct = default)
    {
        return _db.Countries.AddAsync(country, ct).AsTask();
    }        

    public void Remove(Country country)
    {
        _db.Countries.Remove(country);
    }

    public Task<Country?> GetByIdAsync(
        string countryId, 
        CancellationToken ct = default)
    {
        string? id = countryId.Trim().ToUpper();
        return _db.Countries.Include(c => c.Cities).SingleOrDefaultAsync(c => c.CountryId == id, ct);
    }
}
