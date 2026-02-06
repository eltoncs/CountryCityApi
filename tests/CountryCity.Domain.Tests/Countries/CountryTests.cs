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
        var country = new Country("us", "United States of America", "admin");
        country.CountryId.Should().Be("US");
    }

    [Fact]
    public void Ctor_ShouldThrow_WhenCountryIdMissing()
    {
        var act = () => new Country("", "United States of America", "admin");
        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void Ctor_ShouldThrow_WhenCountryIdIsNotTwoCharsLength()
    {
        var act = () => new Country("usa", "United States of America", "admin");
        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void UpdateName_ShouldThrow_WhenNameEmpty()
    {
        var country = new Country("US", "United States of America", "admin");
        var act = () => country.UpdateName("  ");
        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void Ctor_ShouldPass()
    {
        var country = new Country("us", "United States of America", "admin");
        Assert.Equal("US", country.CountryId);
        Assert.Equal("United States of America", country.CountryName);
        Assert.Equal("admin", country.CreatedBy);
        Assert.NotEqual(default, country.CreateDate);
        Assert.Empty(country.Cities);
    }
}
