using CountryCity.Application.Abstractions;
using CountryCity.Application.Dtos;
using CountryCity.Application.Services;
using CountryCity.Domain.Common;
using CountryCity.Domain.Countries;
using FluentAssertions;
using Moq;
using Xunit;

namespace CountryCity.Application.Tests.Services;

public class CountryServiceTests
{
    [Fact]
    public async Task CreateCountryAsync_ShouldPersistAndReturnResponse()
    {
        var repo = new Mock<ICountryRepository>(MockBehavior.Strict);
        var uow = new Mock<IUnitOfWork>(MockBehavior.Strict);

        repo.Setup(r => r.ExistsAsync("US", It.IsAny<CancellationToken>())).ReturnsAsync(false);
        repo.Setup(r => r.AddAsync(It.IsAny<Country>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        uow.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var svc = new CountryService(repo.Object, uow.Object);
        var request = new CreateCountryRequest("US", "United States");
        var response = await svc.CreateCountryAsync(request, "admin", CancellationToken.None);

        response.CountryId.Should().Be("US");
        response.CountryName.Should().Be("United States");
        repo.Verify(r => r.AddAsync(It.IsAny<Country>(), It.IsAny<CancellationToken>()), Times.Once);
        uow.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreateCountryAsync_ShouldThrow_WhenCountryAlreadyExists()
    {
        var repo = new Mock<ICountryRepository>(MockBehavior.Strict);
        var uow = new Mock<IUnitOfWork>(MockBehavior.Strict);

        repo.Setup(r => r.ExistsAsync("US", It.IsAny<CancellationToken>())).ReturnsAsync(true);

        var svc = new CountryService(repo.Object, uow.Object);
        var request = new CreateCountryRequest("US", "United States");

        var act = async () => await svc.CreateCountryAsync(request, "admin", CancellationToken.None);

        await act.Should().ThrowAsync<DomainException>();
    }

    
    [Fact]
    public async Task UpdateCountryAsync_ShouldReturnFalse_WhenNotFound()
    {
        var repo = new Mock<ICountryRepository>(MockBehavior.Strict);
        var uow = new Mock<IUnitOfWork>(MockBehavior.Strict);

        repo.Setup(r => r.GetByIdAsync("US", It.IsAny<CancellationToken>())).ReturnsAsync((Country?)null);

        var svc = new CountryService(repo.Object, uow.Object);
        var request = new UpdateCountryRequest("United States");

        var ok = await svc.UpdateCountryAsync("US", request, CancellationToken.None);

        ok.Should().BeFalse();
    }

    [Fact]
    public async Task DeleteCountryAsync_ShouldRemoveAndSaveChanges()
    {
        var repo = new Mock<ICountryRepository>(MockBehavior.Strict);
        var uow = new Mock<IUnitOfWork>(MockBehavior.Strict);

        var country = new Country("US", "United States", "admin");
        repo.Setup(r => r.GetByIdAsync("US", It.IsAny<CancellationToken>())).ReturnsAsync(country);
        repo.Setup(r => r.Remove(country));
        uow.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var svc = new CountryService(repo.Object, uow.Object);

        var ok = await svc.DeleteCountryAsync("US", CancellationToken.None);

        ok.Should().BeTrue();
        repo.Verify(r => r.Remove(country), Times.Once);
        uow.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
