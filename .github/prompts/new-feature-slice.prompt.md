---
description: "Scaffold a new vertical slice across Domain → Application → Infrastructure → Web, mirroring the existing Product slice"
argument-hint: "entity name, e.g. Order"
agent: agent
---

Add a complete vertical slice for the entity named in my request. Mirror the existing
`Product` slice exactly — read those files first, then follow the same shapes.

Reference files:
[Product entity](../../src/CleanArch.Domain/Entities/Product.cs) ·
[IProductRepository](../../src/CleanArch.Application/Abstractions/IProductRepository.cs) ·
[ProductModels](../../src/CleanArch.Application/Products/ProductModels.cs) ·
[ProductService](../../src/CleanArch.Application/Products/ProductService.cs) ·
[InMemoryProductRepository](../../src/CleanArch.Infrastructure/Persistence/InMemoryProductRepository.cs) ·
[ProductEndpoints](../../src/CleanArch.Web/Endpoints/ProductEndpoints.cs)

Work in this order:

1. **Domain** — `src/CleanArch.Domain/Entities/{Entity}.cs`: `sealed class`, `Id` with
   `Guid.NewGuid()` initializer and private setters. Invariants enforced in the constructor and
   in dedicated mutator methods that throw `ArgumentException` / `ArgumentOutOfRangeException`.

2. **Application**
   - `Abstractions/I{Entity}Repository.cs` — port returning `IReadOnlyList<T>` / `T?`, every
     method taking `CancellationToken cancellationToken = default`.
   - `{Entities}/{Entity}Models.cs` — `sealed record Create{Entity}Request` and
     `sealed record {Entity}Response`.
   - `{Entities}/{Entity}Service.cs` — `sealed class` with primary-constructor injection, a
     `private static ToResponse` mapper, and no domain types crossing the boundary.
   - Register the service in `DependencyInjection.cs` with `AddScoped<{Entity}Service>()`.

3. **Infrastructure** — `Persistence/InMemory{Entity}Repository.cs`, `internal sealed`, and
   register it in `DependencyInjection.cs` as `AddSingleton<I{Entity}Repository, InMemory{Entity}Repository>()`.

4. **Web** — `Endpoints/{Entity}Endpoints.cs`, a `public static class` with a
   `Map{Entity}Endpoints(this IEndpointRouteBuilder app)` extension using `MapGroup("/{entities}")`
   + `.WithTags(...)`, `TypedResults` inside `Results<T1,T2>` unions, and shape validation
   (null/empty/negative checks) returning `TypedResults.ValidationProblem`. Call the mapping
   from `Program.cs`.

Then run `dotnet build CleanArch.sln` and fix any errors.

Constraints:
- Do not add NuGet packages.
- Do not modify the `Product` slice or any unrelated file.
- Do not reference Infrastructure from Application or Domain.
- Flow `CancellationToken` through every async call.
