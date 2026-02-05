using CountryCity.Application.Abstractions;
using CountryCity.Application.Dtos;
using CountryCity.Domain.Common;
using CountryCity.Domain.Countries;

namespace CountryCity.Application.Services;

public sealed class CountryService
{
    private readonly ICountryRepository _repo;
    private readonly IUnitOfWork _uow;

    public CountryService(ICountryRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<CountryResponse> CreateCountryAsync(
        CreateCountryRequest req, 
        string createdBy, 
        CancellationToken ct)
    {
        if (await _repo.ExistsAsync(req.CountryId, ct))
        {
            throw new DomainException($"Country '{req.CountryId}' already exists.");
        }
            

        Country? country = new Country(req.CountryId, req.CountryName, createdBy);

        await _repo.AddAsync(country, ct);
        await _uow.SaveChangesAsync(ct);

        return new CountryResponse(country.CountryId, country.CountryName, country.CreateDate, country.CreatedBy);
    }

    public async Task<CountryResponse?> GetCountryAsync(
        string countryId, 
        CancellationToken ct)
    {
        Country? country = await _repo.GetByIdAsync(countryId, ct);

        return country is null ? null : new CountryResponse(country.CountryId, country.CountryName, country.CreateDate, country.CreatedBy);
    }

    public async Task<IReadOnlyList<CountryResponse>?> GetCountriesAsync(CancellationToken ct)
    {
        List<Country> countries = await _repo.GetAll(ct);

        return countries.Select(c => new CountryResponse(c.CountryId, c.CountryName, c.CreateDate, c.CreatedBy)).ToList();
    }

    public async Task<bool> UpdateCountryAsync(
        string countryId, 
        UpdateCountryRequest req, 
        CancellationToken ct)
    {
        Country? country = await _repo.GetByIdAsync(countryId, ct);

        if (country is null)
        {
            return false;
        }

        country.UpdateName(req.CountryName);

        await _uow.SaveChangesAsync(ct);
        return true;
    }

    public async Task<bool> DeleteCountryAsync(
        string countryId, 
        CancellationToken ct)
    {
        Country? country = await _repo.GetByIdAsync(countryId, ct);

        if (country is null)
        {
            return false;
        }

        _repo.Remove(country);
        await _uow.SaveChangesAsync(ct);
        return true;
    }
}
