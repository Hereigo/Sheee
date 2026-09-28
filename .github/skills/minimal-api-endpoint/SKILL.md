---
name: minimal-api-endpoint
description: 'Add or modify an HTTP endpoint in the MyApiApp ASP.NET Core minimal API. Use for "add an endpoint", "new route", "expose a GET/POST/PUT/DELETE", "return 404 from the API", "add a request DTO", "wire up validation for a route", or "test my endpoint manually". Covers the Program.cs registration pattern, TypedResults return style, record DTO conventions, MyApiApp.http request entries, and build/run verification.'
argument-hint: 'The route and behavior to add, e.g. "GET /products/{id}"'
---

# Minimal API Endpoint

Add or change routes in [MyApiApp/Program.cs](../../../MyApiApp/Program.cs) without growing the app into a layered structure.

## When to Use

- Adding a new route to the API
- Changing the response shape or status codes of an existing route
- Adding a request/response DTO for a route

Do not use this to introduce controllers, services, repositories, or new projects. If the task genuinely needs those, say so and stop.

## Procedure

1. **Read [MyApiApp/Program.cs](../../../MyApiApp/Program.cs) first.** All routes live in this single file, registered between `var app = builder.Build();` and `app.Run();`.

2. **Register the route** after the existing `app.MapGet(...)` calls and before `app.Run();`. Keep the handler inline and small.

3. **Return `TypedResults`.** Use a `Results<T1, T2>` union when the endpoint has more than one outcome:

   ```csharp
   app.MapGet("/items/{id:int}", Results<Ok<Item>, NotFound> (int id) =>
       store.TryGetValue(id, out var item)
           ? TypedResults.Ok(item)
           : TypedResults.NotFound())
       .WithName("GetItem");
   ```

   `Results<,>` and `Ok<T>` live in `Microsoft.AspNetCore.Http.HttpResults`. `ImplicitUsings` is enabled in [MyApiApp/MyApiApp.csproj](../../../MyApiApp/MyApiApp.csproj), so ASP.NET Core namespaces are already available — only add a `using` if the compiler actually complains.

4. **Declare DTOs as `record` types at the bottom of the file**, next to the existing `WeatherForecast` record. Nullable reference types are enabled: mark optional members `?`.

5. **Validate input in the handler.** Prefer route constraints (`{id:int}`) for shape, and an explicit `TypedResults.BadRequest`/`ValidationProblem` for business rules. Never trust client-supplied values used for lookups or writes.

6. **Name the route** with `.WithName("...")` so it appears in the OpenAPI document. OpenAPI is mapped in Development only, at `/openapi/v1.json`.

7. **Add a request entry** to [MyApiApp/MyApiApp.http](../../../MyApiApp/MyApiApp.http) for each new route so it can be exercised from the editor.

## Verification

Always build, and report the real output:

```powershell
dotnet build MyApiApp/MyApiApp.csproj
```

`dotnet build` from the repo root fails with MSB1003 — there is no solution file. Always pass the project path.

To exercise the endpoint manually:

```powershell
dotnet run --project MyApiApp/MyApiApp.csproj
```

There is no test project in this repo. If the user wants automated coverage, point them at the `/xunit-tests` prompt instead of hand-rolling a test setup here.

## Guardrails

- One file: routes stay in `Program.cs` unless the user explicitly asks for a different structure.
- Do not change `TargetFramework` or package versions in [MyApiApp/MyApiApp.csproj](../../../MyApiApp/MyApiApp.csproj).
- Do not add authentication, CORS, rate limiting, or middleware that the task did not ask for.
- Follow the repo rules in [AGENTS.md](../../../AGENTS.md).
