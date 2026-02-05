namespace CountryCity.Domain.Auth;

public class User
{
    private User() { }

    public User(Guid userId, string username, string passwordHash, string passwordSalt)
    {
        UserId = userId;
        Username = username;
        PasswordHash = passwordHash;
        PasswordSalt = passwordSalt;
    }

    public Guid UserId { get; private set; }
    public string Username { get; private set; } = default!;
    public string PasswordHash { get; private set; } = default!;
    public string PasswordSalt { get; private set; } = default!;
}
