# Individuals Directory

Web API for the individuals directory task. .NET 10, EF Core, SQL Server.

## Setup

1. Update `ConnectionStrings:Default` in `src/IndividualsDirectory.Api/appsettings.json`.
2. Create the schema (pick one):
   - `dotnet ef database update -p src/IndividualsDirectory.Data -s src/IndividualsDirectory.Api`
   - or run `scripts/CreateDatabase.sql` against your DB
3. (Optional) Run `scripts/SeedDemoData.sql` for sample individuals/contacts/connections.
4. `dotnet run --project src/IndividualsDirectory.Api`

Swagger at `/swagger`.

## Endpoints

All under `/api/individuals`:

- `POST /` — create
- `PUT /{id}` — edit basic info (PATCH-style, only sent fields change)
- `DELETE /{id}` — delete
- `GET /{id}` — full details
- `GET /quick-search` — LIKE on firstName / lastName / personalNumber, OR
- `GET /detailed-search` — exact match on every field, AND
- `POST /{id}/image` — multipart upload (field name `File`)
- `GET /{id}/image` — stream the image
- `PUT /{id}/connections` — set the list (server diffs add/remove)
- `GET /{id}/connections/grouped` — this person's connections grouped by type
- `GET /connection-counts` — system-wide connection-counts report

## Notes

- Connections are bidirectional. Adding (A → B) creates two rows; deleting from either side removes both.
- Validation is FluentValidation, fired by a global action filter. Domain checks (city exists, personal number unique, no self-connection) live in the service and surface as 400 via the exception middleware.
- Localization: send `Accept-Language: ka` for Georgian. Default is English.
- Images saved to `uploads/individuals/{guid}.{ext}` on disk. DB only stores the GUID. URL is built in the controller.
- Cities ship as reference data via `HasData`. Demo individuals are in `SeedDemoData.sql` instead — kept out of migrations.

## Layout

```
src/
  IndividualsDirectory.Api/       controllers, validators, middleware, filters
  IndividualsDirectory.Service/   business logic, DTOs, image storage
  IndividualsDirectory.Data/      EF context, entities, repos, UoW, migrations
scripts/
  CreateDatabase.sql              schema + cities (auto-generated from migrations)
  SeedDemoData.sql                optional demo data
```
