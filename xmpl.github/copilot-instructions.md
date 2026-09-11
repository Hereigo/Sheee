# WebApplication222 — Project Guidelines

ASP.NET Core **10.0** MVC web application with ASP.NET Core Identity, Entity Framework Core, and SQL Server (LocalDB). Server-rendered Razor views with Bootstrap and jQuery.

## Technology

- .NET 10
- C#
- ASP.NET Core
- Entity Framework Core
- xUnit

## Architecture

- **Pattern**: MVC. Controllers in [Controllers/](Controllers/), views in [Views/](Views/), view models in [Models/](Models/).

- **Data access**: EF Core via `ApplicationDbContext` in [Data/ApplicationDbContext.cs](Data/ApplicationDbContext.cs). Migrations live in [Data/Migrations/](Data/Migrations/).

- **Auth**: ASP.NET Core Identity with the default `IdentityUser`. Identity UI is scaffolded under [Areas/Identity/](Areas/Identity/). Sign-in requires a confirmed account (`RequireConfirmedAccount = true`).

- **Startup**: Single-file minimal hosting in [Program.cs](Program.cs) — services and middleware pipeline are configured there.

- **Front-end**: Server-rendered Razor (`.cshtml`), shared layout in [Views/Shared/_Layout.cshtml](Views/Shared/_Layout.cshtml). Client libraries (Bootstrap, jQuery, jQuery-validation) are under [wwwroot/lib/](wwwroot/lib/).

- Follow Clean Architecture.
- Keep business logic out of controllers.
- Use dependency injection rather than manually constructing services.
- Domain projects must not depend on Infrastructure.
- Prefer async/await for I/O operations.

## Build and Run

Requires the .NET 10 SDK.

```powershell
dotnet restore
dotnet build
dotnet run                 # launches with the profile in Properties/launchSettings.json
```

App URLs (Development): `https://localhost:7049` and `http://localhost:5249`.

## Database and Migrations

Uses SQL Server LocalDB; the connection string `DefaultConnection` is in [appsettings.json](appsettings.json).

```powershell
dotnet ef migrations add <Name> --output-dir Data/Migrations
dotnet ef database update
```

Requires the EF Core CLI: `dotnet tool install --global dotnet-ef`.

## Conventions

- Enable nullable reference types.
- Prefer explicit types when they improve readability; otherwise use `var`.
- Use records for immutable DTOs where appropriate.
- Do not suppress compiler warnings without a documented reason.

- **Nullable** reference types and **implicit usings** are enabled (see [WebApplication222.csproj](WebApplication222.csproj)). Respect nullability annotations; don't suppress warnings without cause.
- Namespaces are `WebApplication222.*` and match folder structure.
- Controllers return `IActionResult` and end in `Controller`; actions map to views of the same name under `Views/<Controller>/`.
- Keep secrets out of source. Local secrets use user-secrets (`UserSecretsId` is set in the csproj); never commit real connection strings or credentials.
- New EF entities: add a `DbSet<>` to `ApplicationDbContext`, then create a migration — do not hand-edit the model snapshot.

## Security

- Follow OWASP Top 10. Identity handles auth; use `[Authorize]` on protected actions/controllers.
- Preserve anti-forgery protection on POST forms (Razor generates it by default; keep `[ValidateAntiForgeryToken]` where present).
- Validate/annotate models and rely on model binding validation rather than manual parsing.

## Testing

- Every new business rule must have unit tests.
- Use integration tests for database behavior.
- Run `dotnet build` and `dotnet test` after significant changes.

## Before changing code

- Inspect existing patterns in the relevant project.
- Prefer extending existing abstractions over introducing parallel implementations.
- Do not introduce a new NuGet package when the existing framework or dependencies provide the required functionality.