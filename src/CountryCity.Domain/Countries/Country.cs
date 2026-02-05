using CountryCity.Domain.Common;

namespace CountryCity.Domain.Countries;

public class Country
{
    private readonly List<City> _cities = new();

    private Country() { }

    public Country(string countryId, string countryName, string createdBy)
    {
        SetId(countryId);
        UpdateName(countryName);
        CreateDate = DateTime.UtcNow;
        CreatedBy = createdBy;
    }

    public string CountryId { get; private set; } = default!;
    public string CountryName { get; private set; } = default!;
    public string CreatedBy { get; private set; } = default!;
    public DateTime CreateDate { get; private set; }

    public IReadOnlyCollection<City> Cities => _cities.AsReadOnly();

    public void UpdateName(string countryName)
    {
        if (string.IsNullOrWhiteSpace(countryName))
        {
            throw new DomainException("countryName is required.");
        }

        CountryName = countryName.Trim();        
    }

    private void SetId(string countryId)
    {
        if (string.IsNullOrWhiteSpace(countryId)) throw new DomainException("countryId is required.");
        if (countryId.Length != 2) throw new DomainException("countryId must have two characters.");

        CountryId = countryId.Trim().ToUpperInvariant();
    }
}
