# Individuals Directory

A .NET 10 Web API for managing individuals (natural persons). Built as a layered application with clean separation between API, business logic, and data access.

## Tech stack

- **.NET 10** (ASP.NET Core Web API)
- **C#** with controllers-based routing
- **OpenAPI** (built-in Microsoft.AspNetCore.OpenApi)
- In-memory repository (swappable for EF Core / SQL later)

## Solution structure

```
individuals-directory/
├── IndividualsDirectory.slnx
└── src/
    ├── IndividualsDirectory.Api/         # HTTP layer — controllers, DI composition, startup
    ├── IndividualsDirectory.Service/     # Business logic — services, DTOs, request models
    └── IndividualsDirectory.Data/        # Data access — entities, repositories
```

**Dependencies:** `Api` → `Service` → `Data`. The Data layer has no upward references.

## Getting started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)

### Run

```bash
dotnet run --project src/IndividualsDirectory.Api
```

The API will listen on the ports configured in `src/IndividualsDirectory.Api/Properties/launchSettings.json` (typically `https://localhost:7xxx` and `http://localhost:5xxx`).

OpenAPI document is exposed at `/openapi/v1.json` in Development.

### Build

```bash
dotnet build
```

### Test

```bash
dotnet test
```

## API endpoints

Base route: `/api/individuals`

| Method | Route                     | Description             |
|--------|---------------------------|-------------------------|
| GET    | `/api/individuals`        | List all individuals    |
| GET    | `/api/individuals/{id}`   | Get individual by id    |
| POST   | `/api/individuals`        | Create individual       |
| PUT    | `/api/individuals/{id}`   | Update individual       |
| DELETE | `/api/individuals/{id}`   | Delete individual       |

### Example request

```bash
curl -X POST http://localhost:5000/api/individuals \
  -H "Content-Type: application/json" \
  -d '{
    "firstName": "John",
    "lastName": "Doe",
    "personalNumber": "01001012345",
    "dateOfBirth": "1990-01-01",
    "phoneNumber": "+995555123456",
    "email": "john.doe@example.com"
  }'
```

## Notes

- Authorization is **not** configured; all endpoints are public.
- Storage is in-memory; data is lost on restart. Swap `InMemoryIndividualRepository` with an EF Core / Dapper implementation to persist.
