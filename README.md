
# 🌍 CountryCity API

> A .NET 8 REST API using **DDD/Clean Architecture**, **JWT authentication**, and **ASP.NET Core OutputCache** with **tag-based eviction**.

This API manages **Countries** and their **Cities**, and demonstrates how to cache list endpoints while keeping data fresh using explicit tag eviction after writes.

---

## ✨ Features

- 🧱 Domain-Driven Design (DDD) + clean boundaries
- 🔐 JWT Bearer Authentication (`[Authorize]` on controllers)
- ⚡ Output caching **only for selected GET endpoints**
- 🧹 Cache invalidation using `IOutputCacheStore.EvictByTagAsync(...)`
- 🗄️ EF Core + SQL Server
- ✅ Unit tests (Domain + Application)

---

## 🗂️ Solution Structure

```
CountryCity.sln
├─ src/
│  ├─ CountryCity.Domain/
│  ├─ CountryCity.Application/
│  ├─ CountryCity.Infrastructure/
│  └─ CountryCity.Api/
└─ tests/
   ├─ CountryCity.Domain.Tests/
   └─ CountryCity.Application.Tests/
```

### Layer responsibilities

| Project | Responsibility |
|---|---|
| `CountryCity.Domain` | Entities and domain rules (no external dependencies). |
| `CountryCity.Application` | DTOs + application services (use-cases). |
| `CountryCity.Infrastructure` | Persistence (EF Core), repositories, DbContext. |
| `CountryCity.Api` | Controllers, auth, output caching, Swagger, middleware. |
| `CountryCity.Domain.Tests` | Unit tests for domain rules/invariants. |
| `CountryCity.Application.Tests` | Unit tests for application services/use-cases. |

---

## 🔐 Authentication

All endpoints are protected:
Get a jwt token: 
 user: admin
 pw: 123456

```csharp
[Authorize]
```

The authenticated username (`User.Identity!.Name!`) is used when creating entities (e.g., for audit fields like `CreatedBy`).

---

## ⚡ Output Caching (what is cached)

Only endpoints explicitly decorated with `[OutputCache]` are cached.

### Cached endpoints

| Endpoint | Attribute | Policy |
|---|---|---|
| `GET /api/country/getAll` | `[OutputCache]` | `CountryByIdPolicy` |
| `GET /api/country/{countryId}/cities/getAllFromCountry` | `[OutputCache]` | `CitiesByCountryPolicy` |

> Note: `GET /api/country/{countryId}` and `GET /api/country/{countryId}/cities/getCity/{cityId}` are **not cached**.

---

## 🧹 Cache eviction (tag-based)

After write operations, cache is explicitly evicted using tags:

### CountryController

- **Create country** evicts: `country`
- **Update/Delete country** evicts: `country:{countryId}`

### CityController

- **Create/Delete city** evicts: `cities:{countryId}`

⚠️ **Inconsistency:** `Update city` currently evicts `cities` (without `{countryId}`).  
If your cached city list uses country-specific tags (e.g., `cities:{countryId}`), the update eviction should likely be:

```csharp
await _cache.EvictByTagAsync($"cities:{countryId}", ct);
```

---

## 📌 API Endpoints

### Countries

| Method | Route |
|---|---|
| POST | `/api/country` |
| GET | `/api/country/getAll` ✅ cached |
| GET | `/api/country/{countryId}` |
| PUT | `/api/country/{countryId}` |
| DELETE | `/api/country/{countryId}` |

### Cities

| Method | Route |
|---|---|
| POST | `/api/country/{countryId}/cities` |
| GET | `/api/country/{countryId}/cities/getAllFromCountry` ✅ cached |
| GET | `/api/country/{countryId}/cities/getCity/{cityId}` |
| PUT | `/api/country/{countryId}/cities/{cityId}` |
| DELETE | `/api/country/{countryId}/cities/{cityId}` |

---

## ▶️ Run locally

```bash
dotnet restore
dotnet ef database update --project src/CountryCity.Infrastructure --startup-project src/CountryCity.Api
dotnet run --project src/CountryCity.Api
```

Swagger:

```
https://localhost:<port>/swagger
```

---

## ✅ Run tests

```bash
dotnet test
```

---

## 📄 License

MIT
