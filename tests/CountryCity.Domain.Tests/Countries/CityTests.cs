using CountryCity.Domain.Common;
using CountryCity.Domain.Countries;
using FluentAssertions;
using Xunit;

namespace CountryCity.Domain.Tests.Countries;

public class CityTests
{
    [Fact]
    public void Ctor_ShouldThrow_WhenIdEmpty()
    {
        var act = () => new City(string.Empty, "us", "New York City", "admin");
        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void Ctor_ShouldThrow_WhenIdCityIdIsNotThreeCharsLength()
    {
        var act = () => new City("abcd", "us", "New York City", "admin");
        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void Ctor_ShouldThrow_WhenIdCountryIdIsNotTwoCharsLength()
    {
        var act = () => new City("nyc", "usa", "New York City", "admin");
        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void Ctor_ShouldTrimName()
    {
        var id = "nyc";
        var city = new City(id, "us", "  New York City  ", "admin");
        city.CityName.Should().Be("New York City");
    }

    [Fact]
    public void UpdateName_ShouldThrow_WhenEmpty()
    {
        var id = "nyc";
        var city = new City(id, "us", "New York City", "admin");
        var act = () => city.UpdateName(" ");
        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void Ctor_ShouldPass()
    {
        var city = new City("nyc", "us", "New York City", "admin");

        Assert.Equal("US", city.CountryId);
        Assert.Equal("NYC", city.CityId);
        Assert.Equal("New York City", city.CityName);
        Assert.Equal("admin", city.CreatedBy);
        Assert.NotEqual(default, city.CreateDate);
    }
}
