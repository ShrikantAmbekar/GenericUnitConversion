# Generic Unit Conversion API

RESTful **ASP.NET Core** Web API that converts numerical values between measurement units for **length**, **weight/mass**, and **temperature**.

Repository: [github.com/ShrikantAmbekar/GenericUnitConversion](https://github.com/ShrikantAmbekar/GenericUnitConversion)

## Features

- Versioned REST API (`/api/v1`)
- Categories: length, weight, temperature (extensible design)
- Case-insensitive unit codes
- Swagger / OpenAPI in Development
- Layered architecture with dependency injection
- Strategy-based conversion engine and in-memory unit catalog
- FluentValidation for request input
- Global exception handling with consistent JSON errors
- NUnit unit and integration tests
- GitHub Actions CI (`.github/workflows/dotnet-ci.yml`)

## Technology stack

| Component | Choice |
|-----------|--------|
| Framework | ASP.NET Core Web API, **.NET 9** |
| Language | C# |
| Validation | FluentValidation |
| API docs | Swashbuckle (Swagger) |
| Testing | NUnit, Moq, `WebApplicationFactory` |
| CI | GitHub Actions, `actions/setup-dotnet@v4` |

## Supported conversions

Examples aligned with the technical challenge:

| Category | Example |
|----------|---------|
| Length | meter ↔ feet, kilometer ↔ mile |
| Temperature | celsius ↔ fahrenheit, celsius ↔ kelvin |
| Weight | kilogram ↔ pound, gram ↔ ounce |

### Unit catalog

| Category | Units |
|----------|-------|
| Length | `meter`, `kilometer`, `feet`, `inch`, `mile` |
| Weight | `kilogram`, `gram`, `pound`, `ounce` |
| Temperature | `celsius`, `fahrenheit`, `kelvin` |

## Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download) or newer

## Installation and run

```bash
git clone https://github.com/ShrikantAmbekar/GenericUnitConversion.git
cd GenericUnitConversion
dotnet restore
dotnet build
dotnet run --project src/UnitConversion.Api
```

**Swagger UI (Development):** `https://localhost:<port>/swagger` (port shown in console / `launchSettings.json`).

## API endpoints

| Method | Path | Description |
|--------|------|-------------|
| POST | `/api/v1/conversions` | Convert between two units (required by challenge) |
| GET | `/api/v1/units` | List all supported units |
| GET | `/api/v1/units/{category}` | Units for `Length`, `Weight`, or `Temperature` |

### Request example

```http
POST /api/v1/conversions
Content-Type: application/json

{
  "value": 10,
  "fromUnit": "meter",
  "toUnit": "feet"
}
```

### Success response

```json
{
  "originalValue": 10,
  "fromUnit": "meter",
  "toUnit": "feet",
  "convertedValue": 32.8084,
  "category": "Length"
}
```

### Validation and error responses

Missing `fromUnit` / `toUnit` → **400** with validation message.

```json
{
  "status": 400,
  "message": "Conversion between different categories is not supported."
}
```

| Status | Cause |
|--------|--------|
| 200 | Success |
| 400 | Validation, cross-category, invalid values (e.g. negative length/weight, Kelvin below absolute zero) |
| 404 | Unknown unit |
| 500 | Unexpected error (logged server-side) |

## Tests

```bash
dotnet test UnitConversion.sln
```

- **Unit tests:** conversion strategies and application service
- **Integration tests:** HTTP API (200 / 400 / 404 scenarios)

## Project structure

```
UnitConversion.sln
src/UnitConversion.Api              Controllers, middleware, Program, Swagger
src/UnitConversion.Application    DTOs, validators, IConversionService
src/UnitConversion.Domain           Entities, enums, domain exceptions
src/UnitConversion.Infrastructure   InMemoryUnitProvider, conversion strategies
tests/UnitConversion.UnitTests
tests/UnitConversion.IntegrationTests
.github/workflows/dotnet-ci.yml
```

## Design decisions (README / challenge)

1. **Base-unit factors** for length and weight to support many units without pairwise rules.
2. **Strategy pattern** for non-linear temperature conversions.
3. **`IUnitProvider`** so units can move from in-memory to a database later.
4. Units are **hardcoded in memory** for this version, as allowed by the challenge, with an architecture that scales to hundreds of units.

## CI/CD

On push to `main` or `feature/**`, and on pull requests to `main`, GitHub Actions runs:

1. Checkout  
2. Setup .NET 9.x  
3. `dotnet restore`  
4. `dotnet build --configuration Release`  
5. `dotnet test --configuration Release`  

Workflow file: [.github/workflows/dotnet-ci.yml](.github/workflows/dotnet-ci.yml)

## License

See [LICENSE](LICENSE) in the repository root.