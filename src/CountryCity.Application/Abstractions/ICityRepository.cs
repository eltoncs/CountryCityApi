using CountryCity.Domain.Countries;

namespace CountryCity.Application.Abstractions;

public interface ICityRepository
{
    Task<City?> GetByIdAsync(string rawCountryId, string rawCityId, CancellationToken ct = default);
    Task<bool> ExistsAsync(string countryId, string cityId, CancellationToken ct = default);
    Task AddAsync(City city, CancellationToken ct = default);
    Task<List<City>> GetByCountryAsync(string rawCountryId, CancellationToken ct = default);
    void Remove(City city);
}
