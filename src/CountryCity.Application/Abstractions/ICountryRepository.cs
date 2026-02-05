using CountryCity.Domain.Countries;

namespace CountryCity.Application.Abstractions;

public interface ICountryRepository
{
    Task<Country?> GetByIdAsync(string countryId, CancellationToken ct = default);
    Task<List<Country>> GetAll(CancellationToken ct = default);
    Task<bool> ExistsAsync(string countryId, CancellationToken ct = default);
    Task AddAsync(Country country, CancellationToken ct = default);
    void Remove(Country country);
}
