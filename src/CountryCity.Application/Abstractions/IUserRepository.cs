using CountryCity.Domain.Auth;

namespace CountryCity.Application.Abstractions;

public interface IUserRepository
{
    Task<User?> GetByUsernameAsync(string username, CancellationToken ct = default);
    Task<bool> AnyUsersAsync(CancellationToken ct = default);
    Task AddAsync(User user, CancellationToken ct = default);
}
