using System.ComponentModel.DataAnnotations;

namespace CountryCity.Application.Dtos;

public sealed record CreateCityRequest(
    [Required] string CityId,
    [Required] string CityName
);

public sealed record UpdateCityRequest(
    [Required] string CityName
);

public sealed record CityResponse(
    string CityId,
    string CityName,
    DateTime CreateDate,
    string CreatedBy
);
