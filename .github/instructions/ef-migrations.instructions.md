---
description: "Use when adding EF Core entities, changing the model, writing database migrations, or updating the schema for this SQL Server project."
---
# EF Core Migration Guidelines

Data lives in [Data/ApplicationDbContext.cs](../../Data/ApplicationDbContext.cs); migrations in [Data/Migrations/](../../Data/Migrations/).

## Workflow

1. Add or change the entity class and expose it as a `DbSet<>` on `ApplicationDbContext`.
2. Configure relationships/keys in `OnModelCreating` (call `base.OnModelCreating(builder)` first — Identity relies on it).
3. Generate the migration:
   ```powershell
   dotnet ef migrations add <DescriptiveName> --output-dir Data/Migrations
   ```
4. Review the generated `Up`/`Down` before applying, then update the database:
   ```powershell
   dotnet ef database update
   ```

## Rules

- Never hand-edit `ApplicationDbContextModelSnapshot.cs` — regenerate via the CLI.
- Migrations must be reversible: verify the `Down` method is correct.
- Avoid dropping columns/tables in the same migration that removes their code — split across releases.
- Don't put data-loss operations in a migration without confirming with the user first.
- Keep the connection string out of migrations; it comes from `DefaultConnection` in [appsettings.json](../../appsettings.json).
