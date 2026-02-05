using CountryCity.Application.Abstractions;
using CountryCity.Domain.Auth;
using CountryCity.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CountryCity.Infrastructure.Repositories;

public sealed class UserRepository : IUserRepository
{
    private readonly AppDbContext _db;

    public UserRepository(AppDbContext db) => _db = db;

    public Task<User?> GetByUsernameAsync(
        string username, 
        CancellationToken ct = default)
    {
        string? user = username.Trim().ToLowerInvariant();
        return _db.Users.SingleOrDefaultAsync(x => x.Username.ToLower() == user, ct);
    }

    public Task<bool> AnyUsersAsync(CancellationToken ct = default)
    {
        return _db.Users.AnyAsync(ct);
    }        

    public Task AddAsync(User user, CancellationToken ct = default) 
    {
        return _db.Users.AddAsync(user, ct).AsTask();
    }        
}
