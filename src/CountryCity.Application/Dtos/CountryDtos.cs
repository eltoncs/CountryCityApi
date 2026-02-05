using System.ComponentModel.DataAnnotations;

namespace CountryCity.Application.Dtos;

public sealed record CreateCountryRequest(
    [Required] string CountryId,
    [Required] string CountryName
);

public sealed record UpdateCountryRequest(
    [Required] string CountryName
);

public sealed record CountryResponse(
    string CountryId,
    string CountryName,
    DateTime CreationDate,
    string CreatedBy
);
