using CountryCity.Application.Dtos;
using CountryCity.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace CountryCity.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/country/{countryId}/cities")]
public sealed class CityController : ControllerBase
{
    private readonly CityService _service;
    private readonly IOutputCacheStore _cache;

    public CityController(CityService service, IOutputCacheStore cache)
    {
        _service = service;
        _cache = cache;
    }

    [HttpPost]
    [ProducesResponseType(typeof(CityResponse), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create(
        [FromRoute] string countryId, 
        [FromBody] CreateCityRequest req, 
        CancellationToken ct)
    {
        string userName = User.Identity!.Name!;
        CityResponse created = await _service.CreateCityAsync(req, countryId, userName, ct);

        await _cache.EvictByTagAsync($"cities:{countryId}", ct);

        return CreatedAtAction(nameof(GetCity), new { countryId, cityId = created.CityId }, created);
    }

    [HttpGet("getAllFromCountry")]
    [ProducesResponseType(typeof(IEnumerable<CityResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [OutputCache(PolicyName = "CitiesByCountryPolicy")]
    public async Task<IActionResult> GetAll(
        [FromRoute] string countryId, 
        CancellationToken ct)
    {
        IReadOnlyList<CityResponse>? cities = await _service.GetCitiesAsync(countryId, ct);
        return Ok(cities ?? Array.Empty<CityResponse>());
    }

    [HttpGet("getCity/{cityId}")]
    [ProducesResponseType(typeof(IEnumerable<CityResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCity(
        [FromRoute] string countryId, 
        [FromRoute] string cityId, 
        CancellationToken ct)
    {
        CityResponse? cities = await _service.GetCityAsync(countryId, cityId, ct);
        return cities is null ? NotFound() : Ok(cities);
    }

    [HttpPut("{cityId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
        [FromRoute] string countryId, 
        [FromRoute] string cityId, 
        [FromBody] UpdateCityRequest req, 
        CancellationToken ct)
    {
        var ok = await _service.UpdateCityAsync(countryId, cityId, req, ct);

        await _cache.EvictByTagAsync("cities", ct);
        return ok ? NoContent() : NotFound();
    }

    [HttpDelete("{cityId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteCity(
        [FromRoute] string countryId, 
        [FromRoute] string cityId, 
        CancellationToken ct)
    {
        var ok = await _service.DeleteCityAsync(countryId, cityId, ct);
        await _cache.EvictByTagAsync($"cities:{countryId}", ct);
        return ok ? NoContent() : NotFound();
    }
}
