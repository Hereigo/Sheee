---
description: "Use when writing or editing C# code in this ASP.NET Core MVC project — controllers, services, EF Core data access, and models."
applyTo: "**/*.cs"
---
# C# / ASP.NET Core Conventions

- Nullable reference types are enabled — annotate nullability and avoid `!` null-forgiving unless justified.
- Implicit usings are on; don't add redundant `using` directives for common namespaces.
- Use file-scoped or block namespaces consistent with the file you're editing; namespaces mirror folder paths under `WebApplication222`.
- Prefer constructor injection for services; register them in [Program.cs](../../Program.cs).
- Controller actions return `IActionResult`; keep them thin and push logic into services.
- Use `async`/`await` with EF Core (`ToListAsync`, `FirstOrDefaultAsync`, `SaveChangesAsync`) for all database calls.
- Never build SQL by string concatenation — use LINQ or parameterized queries.
- Access `ApplicationDbContext` through DI, never instantiate it directly.
