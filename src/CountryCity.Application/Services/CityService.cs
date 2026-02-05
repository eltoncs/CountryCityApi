using CountryCity.Application.Abstractions;
using CountryCity.Application.Dtos;
using CountryCity.Domain.Common;
using CountryCity.Domain.Countries;

namespace CountryCity.Application.Services;

public sealed class CityService
{
    private readonly ICityRepository _repo;
    private readonly IUnitOfWork _uow;

    public CityService(ICityRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<CityResponse> CreateCityAsync(
        CreateCityRequest req, 
        string countryId, 
        string createdBy, 
        CancellationToken ct)
    {
        if (await _repo.ExistsAsync(req.CityId, countryId, ct))
        {
            throw new DomainException($"City '{req.CityId}' already exists.");
        }            

        City? city = new City(req.CityId, countryId, req.CityName, createdBy);
        await _repo.AddAsync(city, ct);
        await _uow.SaveChangesAsync(ct);

        return new CityResponse(city.CityId, city.CityName, city.CreateDate, city.CreatedBy);
    }

    public async Task<IReadOnlyList<CityResponse>?> GetCitiesAsync(
        string countryId, 
        CancellationToken ct)
    {
        List<City>? cities = await _repo.GetByCountryAsync(countryId, ct);

        return cities.Select(c => new CityResponse(c.CityId, c.CityName, c.CreateDate, c.CreatedBy)).ToList();
    }

    public async Task<CityResponse?> GetCityAsync(
        string countryId, 
        string cityId, 
        CancellationToken ct)
    {
        City? city = await _repo.GetByIdAsync(cityId, countryId, ct);

        if (city is null)
        {
            return null;
        }

        return new CityResponse(city.CityId, city.CityName, city.CreateDate, city.CreatedBy);
    }

    public async Task<bool> UpdateCityAsync(
        string countryId, 
        string cityId, 
        UpdateCityRequest req, 
        CancellationToken ct)
    {
        City? city = await _repo.GetByIdAsync(cityId, countryId, ct);

        if (city is null)
        {
            return false;
        }

        city.UpdateName(req.CityName);

        await _uow.SaveChangesAsync(ct);
        return true;
    }

    public async Task<bool> DeleteCityAsync(
        string cityId, 
        string countryId, 
        CancellationToken ct)
    {
        City? city = await _repo.GetByIdAsync(cityId, countryId, ct);

        if (city is null)
        {
            return false;
        }

        _repo.Remove(city);
        await _uow.SaveChangesAsync(ct);
        return true;
    }
}
