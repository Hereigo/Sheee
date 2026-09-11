# CleanArch — ASP.NET Core Minimal API (.NET 9)

Four-project clean architecture solution. Read this file before searching the codebase; it
already describes the layout and conventions, so avoid re-exploring for basic facts.

## Layout & dependency rule

| Project | References | Contents |
| --- | --- | --- |
| `src/CleanArch.Domain` | none | `Entities/Product.cs` — invariants enforced in ctor/methods |
| `src/CleanArch.Application` | Domain | `Abstractions/IProductRepository.cs` (port), `Products/ProductModels.cs` (DTOs), `Products/ProductService.cs` |
| `src/CleanArch.Infrastructure` | Application | `Persistence/InMemoryProductRepository.cs` (`internal` adapter) |
| `src/CleanArch.Web` | Application, Infrastructure | `Program.cs` composition root, `Endpoints/ProductEndpoints.cs` |

Dependencies point inward only. Never reference Infrastructure from Application/Domain, and
never reference Web from anywhere. Each layer registers itself via a `DependencyInjection.cs`
`Add{Layer}()` extension called from `Program.cs`.

## Conventions

- `net9.0`, `Nullable` + `ImplicitUsings` enabled. No `using` directives needed for common BCL namespaces.
- Types are `sealed`; DTOs are `record`s; services use primary-constructor injection.
- Endpoints: `MapGroup` + `TypedResults` with `Results<T1,T2>` unions (no `[ProducesResponseType]` attributes).
- Every async method takes a `CancellationToken` and flows it through to the repository.
- Input shape validation lives in the endpoint; business invariants live in the domain entity.
- Errors surface as RFC 9457 via `AddProblemDetails()` + `UseExceptionHandler()`.
- 4-space indent, allman braces, braces always on `if` blocks.

## Where to put new code

- New entity/business rule → Domain.
- New use case, DTO, or port interface → Application.
- New persistence/external adapter → Infrastructure, registered in its `DependencyInjection.cs`.
- New HTTP route → a `{Feature}Endpoints.cs` static class in `Web/Endpoints`, mapped in `Program.cs`.

## Commands

```powershell
dotnet build CleanArch.sln
dotnet run --project src/CleanArch.Web
```

OpenAPI (Development only): `/openapi/v1.json`.

## Gotchas

- There is no test project yet; validate changes with `dotnet build`.
- Persistence is in-memory and registered as a singleton — state resets on restart.
- Do not run `dotnet add package Microsoft.EntityFrameworkCore*` without pinning a `9.0.*`
  version; the default resolve picks 10.0.x, which is incompatible with `net9.0`.
