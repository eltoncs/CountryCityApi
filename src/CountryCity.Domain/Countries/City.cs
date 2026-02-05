using CountryCity.Domain.Common;
namespace CountryCity.Domain.Countries;

public class City
{
    private City() { }

    public City(string cityId, string countryId, string cityName, string createdBy)
    {
        SetCityId(cityId);
        SetCountryId(countryId);
        UpdateName(cityName);

        CreateDate = DateTime.UtcNow;
        CityName = cityName.Trim();
        CreatedBy = createdBy;
    }

    public string CityId { get; private set; } = default!;
    public string CountryId { get; private set; } = default!;
    public string CityName { get; private set; } = default!;
    public string CreatedBy { get; private set; } = default!;
    public DateTime CreateDate { get; private set; }

    public void UpdateName(string cityName)
    {
        if (string.IsNullOrWhiteSpace(cityName))
        {
            throw new DomainException("cityName is required.");
        }

        CityName = cityName.Trim();
    }

    private void SetCityId(string cityId)
    {
        if (string.IsNullOrWhiteSpace(cityId)) throw new DomainException("cityId cannot be empty.");
        if (cityId.Length != 3) throw new DomainException("cityId must have three characters.");

        CityId = cityId.Trim().ToUpperInvariant();
    }

    private void SetCountryId(string countryId)
    {
        if (string.IsNullOrWhiteSpace(countryId)) throw new DomainException("countryId cannot be empty.");
        if (countryId.Length != 2) throw new DomainException("countryId must have two characters.");

        CountryId = countryId.Trim().ToUpperInvariant();
    }
}