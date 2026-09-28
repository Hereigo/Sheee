---
name: add-crud-feature
description: "Use when building a full CRUD feature (entity + controller + views + migration) in this ASP.NET Core MVC app. Multi-step workflow that wires up data access, actions, and Razor views end to end."
---
# Add a CRUD Feature

A repeatable workflow for adding a complete Create/Read/Update/Delete feature to this
ASP.NET Core 10 MVC project (EF Core + SQL Server + Identity). Follow the steps in order.

## 1. Define the entity
- Create the entity class in `Models/` (or `Data/Entities/`), namespace `WebApplication222.*`.
- Add data-annotation validation (`[Required]`, `[StringLength]`, `[Range]`, etc.).
- Use nullability annotations that reflect required vs optional columns.

## 2. Register on the DbContext
- Add `public DbSet<Entity> Entities { get; set; }` to `Data/ApplicationDbContext.cs`.
- Configure keys/relationships in `OnModelCreating` (keep `base.OnModelCreating(builder)` — Identity needs it).

## 3. Generate the migration
```powershell
dotnet ef migrations add Add<Entity> --output-dir Data/Migrations
```
- Review `Up`/`Down`. Ensure it is reversible and has no unintended data loss.
- Apply with `dotnet ef database update` after review.

## 4. Create the controller
- `Controllers/<Entity>Controller.cs`, actions: `Index`, `Details`, `Create` (GET/POST), `Edit` (GET/POST), `Delete` (GET/POST).
- Inject `ApplicationDbContext`; use async EF Core calls.
- POST actions: `[HttpPost]` + `[ValidateAntiForgeryToken]`, and bind only the fields you intend to accept (avoid over-posting).
- Add `[Authorize]` if the feature requires authentication.

## 5. Create the views
- `Views/<Entity>/{Index,Details,Create,Edit,Delete}.cshtml`, strongly typed with `@model`.
- Use tag helpers (`asp-for`, `asp-action`, `asp-validation-for`) and the shared layout.
- Include `_ValidationScriptsPartial` on forms for client-side validation.

## 6. Validate
- Run `dotnet build` and resolve warnings/errors.
- Manually smoke-test the CRUD flow with `dotnet run`.

See the checklist in [checklist.md](./checklist.md) before marking the feature done.
