using CountryCity.Application.Dtos;
using CountryCity.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CountryCity.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/country")]
public sealed class CountryController : ControllerBase
{
    private readonly CountryService _service;

    public CountryController(CountryService service)
    {
        _service = service;
    }

    /// <summary>
    /// Add a new country
    /// </summary>
    /// <param name="req"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(typeof(CountryResponse), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create(
        [FromBody] CreateCountryRequest req, 
        CancellationToken ct)
    {
        string userName = User.Identity!.Name!;
        CountryResponse created = await _service.CreateCountryAsync(req, userName, ct);
        return CreatedAtAction(nameof(Get), new { countryId = created.CountryId }, created);
    }

    [HttpGet("{countryId}")]
    [ProducesResponseType(typeof(CountryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(
        [FromRoute] string countryId, 
        CancellationToken ct)
    {
        CountryResponse? country = await _service.GetCountryAsync(countryId, ct);
        return country is null ? NotFound() : Ok(country);
    }

    [HttpGet("getAll")]
    [ProducesResponseType(typeof(CountryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        IReadOnlyList<CountryResponse>? countries = await _service.GetCountriesAsync(ct);
        return countries is null ? NotFound() : Ok(countries);
    }

    [HttpPut("{countryId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
        [FromRoute] string countryId, 
        [FromBody] UpdateCountryRequest req, 
        CancellationToken ct)
    {
        var ok = await _service.UpdateCountryAsync(countryId, req, ct);
        return ok ? NoContent() : NotFound();
    }

    [HttpDelete("{countryId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(
        [FromRoute] string countryId, 
        CancellationToken ct)
    {
        var ok = await _service.DeleteCountryAsync(countryId, ct);
        return ok ? NoContent() : NotFound();
    }
}
