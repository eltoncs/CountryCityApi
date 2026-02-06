using CountryCity.Application.Abstractions;
using CountryCity.Application.Dtos;
using CountryCity.Application.Services;
using CountryCity.Domain.Common;
using CountryCity.Domain.Countries;
using FluentAssertions;
using Moq;
using Xunit;

namespace CountryCity.Application.Tests.Services;

public class CityServiceTests
{
    [Fact]
    public async Task CreateCityAsync_ShouldPersistAndReturnResponse()
    {
        var cityRepo = new Mock<ICityRepository>(MockBehavior.Strict);
        var countryRepo = new Mock<ICountryRepository>(MockBehavior.Strict);
        var uow = new Mock<IUnitOfWork>(MockBehavior.Strict);

        cityRepo.Setup(r => r.ExistsAsync("us", "nyc", It.IsAny<CancellationToken>())).ReturnsAsync(false);
        cityRepo.Setup(r => r.AddAsync(It.IsAny<City>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        countryRepo.Setup(r => r.ExistsAsync("us", It.IsAny<CancellationToken>())).ReturnsAsync(true);

        uow.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var svc = new CityService(cityRepo.Object, countryRepo.Object, uow.Object);
        var request = new CreateCityRequest("nyc", "New York City");
        var response = await svc.CreateCityAsync(request, "us", "admin", CancellationToken.None);

        response.CityId.Should().Be("NYC");
        response.CityName.Should().Be("New York City");
        cityRepo.Verify(r => r.AddAsync(It.IsAny<City>(), It.IsAny<CancellationToken>()), Times.Once);
        uow.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreateCityAsync_ShouldThrow_WhenCityAlreadyExists()
    {
        var cityRepo = new Mock<ICityRepository>(MockBehavior.Strict);
        var countryRepo = new Mock<ICountryRepository>(MockBehavior.Strict);
        var uow = new Mock<IUnitOfWork>(MockBehavior.Strict);

        cityRepo.Setup(r => r.ExistsAsync("us", "nyc", It.IsAny<CancellationToken>())).ReturnsAsync(true);
        countryRepo.Setup(r => r.ExistsAsync("us", It.IsAny<CancellationToken>())).ReturnsAsync(true);

        var svc = new CityService(cityRepo.Object, countryRepo.Object, uow.Object);
        var request = new CreateCityRequest("nyc", "New York City");

        var act = async () => await svc.CreateCityAsync(request, "us", "admin", CancellationToken.None);

        await act.Should().ThrowAsync<DomainException>();
    }

    
    [Fact]
    public async Task UpdateCityAsync_ShouldReturnFalse_WhenNotFound()
    {
        var cityRepo = new Mock<ICityRepository>(MockBehavior.Strict);
        var countryRepo = new Mock<ICountryRepository>(MockBehavior.Strict);
        var uow = new Mock<IUnitOfWork>(MockBehavior.Strict);

        cityRepo.Setup(r => r.GetByIdAsync("us", "nyc", It.IsAny<CancellationToken>())).ReturnsAsync((City?)null);

        var svc = new CityService(cityRepo.Object, countryRepo.Object, uow.Object);
        var request = new UpdateCityRequest("New Amsterdam");

        var ok = await svc.UpdateCityAsync("us", "nyc", request, CancellationToken.None);

        ok.Should().BeFalse();
    }

    [Fact]
    public async Task UpdateCityAsync_ShouldPass()
    {
        var cityRepo = new Mock<ICityRepository>(MockBehavior.Strict);
        var countryRepo = new Mock<ICountryRepository>(MockBehavior.Strict);
        var uow = new Mock<IUnitOfWork>(MockBehavior.Strict);

        cityRepo.Setup(r => r.GetByIdAsync("us", "nyc", It.IsAny<CancellationToken>())).ReturnsAsync(new City("nyc","us","New York City","admin"));
        uow.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var svc = new CityService(cityRepo.Object, countryRepo.Object, uow.Object);
        var request = new UpdateCityRequest("New Amsterdam");

        var ok = await svc.UpdateCityAsync("us", "nyc", request, CancellationToken.None);

        ok.Should().BeTrue();
    }

    [Fact]
    public async Task DeleteCityAsync_ShouldRemoveAndSaveChanges()
    {
        var cityRepo = new Mock<ICityRepository>(MockBehavior.Strict);
        var countryRepo = new Mock<ICountryRepository>(MockBehavior.Strict);
        var uow = new Mock<IUnitOfWork>(MockBehavior.Strict);

        var city = new City("nyc", "us", "United States", "admin");
        cityRepo.Setup(r => r.GetByIdAsync("us", "nyc", It.IsAny<CancellationToken>())).ReturnsAsync(city);
        cityRepo.Setup(r => r.Remove(city));
        uow.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var svc = new CityService(cityRepo.Object, countryRepo.Object, uow.Object);

        var ok = await svc.DeleteCityAsync("us", "nyc", CancellationToken.None);

        ok.Should().BeTrue();
        cityRepo.Verify(r => r.Remove(city), Times.Once);
        uow.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
