namespace CountryCity.Api.Auth;

public sealed class JwtOptions
{
    public const string SectionName = "Jwt";

    public string Issuer { get; init; } = "CountryCityApi";
    public string Audience { get; init; } = "CountryCityApiClients";
    public string Key { get; init; } = "CHANGE_ME_TO_A_LONG_RANDOM_SECRET_32_CHARS_MIN";
    public int ExpMinutes { get; init; } = 60;
}
