using CountryCity.Domain.Countries;
using CountryCity.Domain.Auth;
using Microsoft.EntityFrameworkCore;

namespace CountryCity.Infrastructure.Persistence;

public sealed class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Country> Countries => Set<Country>();

    public DbSet<City> Cities => Set<City>();

    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Country>(b =>
        {
            b.ToTable("Countries");
            b.HasKey(x => x.CountryId);

            b.Property(x => x.CountryId).HasMaxLength(2).IsRequired();
            b.Property(x => x.CountryName).HasMaxLength(200).IsRequired();
            b.Property(x => x.CreateDate).IsRequired();
            b.Property(x => x.CreatedBy).HasMaxLength(100).IsRequired();

            b.HasMany(x => x.Cities)
             .WithOne()
             .HasForeignKey("CountryId")
             .OnDelete(DeleteBehavior.Cascade);

            b.Navigation(x => x.Cities)
             .HasField("_cities")
             .UsePropertyAccessMode(PropertyAccessMode.Field);
        });

        modelBuilder.Entity<City>(b =>
        {
            b.ToTable("Cities");
            b.HasKey(x => x.CityId);

            b.Property(x => x.CityId).HasMaxLength(3).IsRequired();
            b.Property(x => x.CityName).HasMaxLength(200).IsRequired();
            b.Property(x => x.CreateDate).IsRequired();
            b.Property(x => x.CreatedBy).IsRequired();

            b.Property<string>("CountryId").HasMaxLength(2).IsRequired();
            b.HasIndex(nameof(City.CityId), "CountryId").HasDatabaseName("IX_Cities_CityId_CountryId");
        });

        modelBuilder.Entity<User>(b =>
        {
            b.ToTable("Users");
            b.HasKey(x => x.UserId);

            b.Property(x => x.Username).HasMaxLength(100).IsRequired();
            b.HasIndex(x => x.Username).IsUnique();

            b.Property(x => x.PasswordHash).HasMaxLength(400).IsRequired();
            b.Property(x => x.PasswordSalt).HasMaxLength(200).IsRequired();
        });
    }
}
