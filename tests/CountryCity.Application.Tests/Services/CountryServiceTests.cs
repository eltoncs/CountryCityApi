//using CountryCity.Application.Abstractions;
//using CountryCity.Application.Dtos;
//using CountryCity.Application.Services;
//using CountryCity.Domain.Common;
//using CountryCity.Domain.Countries;
//using FluentAssertions;
//using Moq;
//using Xunit;

//namespace CountryCity.Application.Tests.Services;

//public class CountryServiceTests
//{
//    [Fact]
//    public async Task CreateCountryAsync_ShouldPersistAndReturnResponse()
//    {
//        var repo = new Mock<ICountryRepository>(MockBehavior.Strict);
//        var uow = new Mock<IUnitOfWork>(MockBehavior.Strict);

//        repo.Setup(r => r.ExistsAsync("US", It.IsAny<CancellationToken>())).ReturnsAsync(false);
//        repo.Setup(r => r.AddAsync(It.IsAny<Country>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
//        uow.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

//        var svc = new CountryService(repo.Object, uow.Object);

//        var res = await svc.CreateCountryAsync(new CreateCountryRequest("US", "United States", DateTime.UtcNow), CancellationToken.None);

//        res.CountryId.Should().Be("US");
//        res.CountryName.Should().Be("United States");
//        repo.Verify(r => r.AddAsync(It.IsAny<Country>(), It.IsAny<CancellationToken>()), Times.Once);
//        uow.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
//    }

//    [Fact]
//    public async Task CreateCountryAsync_ShouldThrow_WhenCountryAlreadyExists()
//    {
//        var repo = new Mock<ICountryRepository>(MockBehavior.Strict);
//        var uow = new Mock<IUnitOfWork>(MockBehavior.Strict);

//        repo.Setup(r => r.ExistsAsync("US", It.IsAny<CancellationToken>())).ReturnsAsync(true);

//        var svc = new CountryService(repo.Object, uow.Object);

//        var act = async () => await svc.CreateCountryAsync(new CreateCountryRequest("US", "United States", DateTime.UtcNow), CancellationToken.None);

//        await act.Should().ThrowAsync<DomainException>();
//    }

//    [Fact]
//    public async Task AddCityAsync_ShouldAddCity_AndSaveChanges()
//    {
//        var repo = new Mock<ICountryRepository>(MockBehavior.Strict);
//        var uow = new Mock<IUnitOfWork>(MockBehavior.Strict);

//        var country = new Country("US", "United States", DateTime.UtcNow);

//        repo.Setup(r => r.GetByIdAsync("US", It.IsAny<CancellationToken>())).ReturnsAsync(country);
//        uow.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

//        var svc = new CountryService(repo.Object, uow.Object);

//        var res = await svc.AddCityAsync("US", new CreateCityRequest(Guid.Empty, "New York"), CancellationToken.None);

//        res.Should().NotBeNull();
//        res!.CityId.Should().NotBe(Guid.Empty);
//        country.Cities.Should().ContainSingle(c => c.CityId == res.CityId);
//        uow.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
//    }

//    [Fact]
//    public async Task UpdateCountryAsync_ShouldReturnFalse_WhenNotFound()
//    {
//        var repo = new Mock<ICountryRepository>(MockBehavior.Strict);
//        var uow = new Mock<IUnitOfWork>(MockBehavior.Strict);

//        repo.Setup(r => r.GetByIdAsync("US", It.IsAny<CancellationToken>())).ReturnsAsync((Country?)null);

//        var svc = new CountryService(repo.Object, uow.Object);

//        var ok = await svc.UpdateCountryAsync("US", new UpdateCountryRequest("X", DateTime.UtcNow), CancellationToken.None);

//        ok.Should().BeFalse();
//    }

//    [Fact]
//    public async Task DeleteCountryAsync_ShouldRemoveAndSaveChanges()
//    {
//        var repo = new Mock<ICountryRepository>(MockBehavior.Strict);
//        var uow = new Mock<IUnitOfWork>(MockBehavior.Strict);

//        var country = new Country("US", "United States", DateTime.UtcNow);
//        repo.Setup(r => r.GetByIdAsync("US", It.IsAny<CancellationToken>())).ReturnsAsync(country);
//        repo.Setup(r => r.Remove(country));
//        uow.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

//        var svc = new CountryService(repo.Object, uow.Object);

//        var ok = await svc.DeleteCountryAsync("US", CancellationToken.None);

//        ok.Should().BeTrue();
//        repo.Verify(r => r.Remove(country), Times.Once);
//        uow.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
//    }
//}
