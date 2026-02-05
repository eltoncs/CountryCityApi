using CountryCity.Domain.Common;
using CountryCity.Domain.Countries;
using FluentAssertions;
using Xunit;

namespace CountryCity.Domain.Tests.Countries;

public class CityTests
{
    //[Fact]
    //public void Ctor_ShouldThrow_WhenIdEmpty()
    //{
    //    var act = () => new City(string.Empty, "X", "X", "X");
    //    act.Should().Throw<DomainException>();
    //}

    //[Fact]
    //public void Ctor_ShouldTrimName()
    //{
    //    var id = Guid.NewGuid();
    //    var city = new City(id, "  Boston  ");
    //    city.CityName.Should().Be("Boston");
    //}

    //[Fact]
    //public void UpdateName_ShouldThrow_WhenEmpty()
    //{
    //    var city = new City(Guid.NewGuid(), "X");
    //    var act = () => city.UpdateName(" ");
    //    act.Should().Throw<DomainException>();
    //}
}
