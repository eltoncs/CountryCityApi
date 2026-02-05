using CountryCity.Domain.Common;
using CountryCity.Domain.Countries;
using FluentAssertions;
using Xunit;

namespace CountryCity.Domain.Tests.Countries;

public class CountryTests
{
    [Fact]
    public void Ctor_ShouldNormalizeCountryIdToUpper()
    {
        var country = new Country("us", "United States", "admin");
        country.CountryId.Should().Be("US");
    }

    [Fact]
    public void Ctor_ShouldThrow_WhenCountryIdMissing()
    {
        var act = () => new Country("", "Name", "admin");
        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void UpdateName_ShouldThrow_WhenNameEmpty()
    {
        var country = new Country("US", "United States", "admin");
        var act = () => country.UpdateName("  ");
        act.Should().Throw<DomainException>();
    }
}
