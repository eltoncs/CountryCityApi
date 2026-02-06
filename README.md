# Country / City REST API (DDD + EF Core + UnitOfWork + Swagger + JWT)

A .NET 8 REST API to manage **Countries** and their **Cities** using:
- DDD-style layering (Domain / Application / Infrastructure / API)
- EF Core (Code First) with SQL Server LocalDB
- Unit of Work
- Swagger/OpenAPI
- JWT Bearer Authentication
- Unit tests (Domain + Application)

## Quick start

### 1) Prereqs
- .NET SDK 8.x
- SQL Server LocalDB (Windows)

### 2) Restore & run
```bash
dotnet restore
dotnet run --project src/CountryCity.Api
```

Swagger UI:
- https://localhost:<port>/swagger

## Authentication (Bearer token)

On the **first run**, the app automatically seeds a default user:

- **username:** `admin`
- **password:** `123456`

Get a token:
- POST `api/auth/token`
```json
{ "username": "admin", "password": "123456" }
```

Use it in requests:
- `Authorization: Bearer <token>`

Swagger also supports Authorize (top-right).

> Security note: replace the seed user and password + store credentials securely in real apps.

## Create the database (Code First)

```bash
dotnet tool install --global dotnet-ef
dotnet ef migrations add InitialCreate --project src/CountryCity.Api --startup-project src/CountryCity.Api --output-dir Migrations
dotnet ef database update --project src/CountryCity.Api --startup-project src/CountryCity.Api
```

## Endpoints (matching your examples)

### Country
- POST `api/country` (create country)
- GET `api/country/{countryId}`
- PUT `api/country/{countryId}`
- DELETE `api/country/{countryId}`

Payload for create:
```json
{
  "countryId": "us",
  "countryName": "United States"
}
```

### Cities by Country (friendly routes)
- POST `api/{countryId}` (add city to a country, e.g. POST `api/US`)
- GET `api/{countryId}` (get all cities for a country, e.g. GET `api/US`)
- PUT `api/{countryId}/{cityId}` (update city)
- DELETE `api/{countryId}/{cityId}` (delete city)

Payload for city create:
```json
{
  "cityId": "nyc",
  "cityName": "New York"
}
```

## Run tests
```bash
dotnet test
```

## Security note
Change `Jwt:Key` in `appsettings*.json` to a long random secret (32+ chars).
