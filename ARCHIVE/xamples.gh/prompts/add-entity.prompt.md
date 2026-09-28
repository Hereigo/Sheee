---
description: "Add a new EF Core entity, register it on the DbContext, and generate a migration."
argument-hint: "Entity name and its properties"
agent: "agent"
---
Add a new persisted entity to this project end to end.

Steps:
1. Create the entity class in [Models/](../../Models/) (or a `Data/Entities` folder) with data-annotation validation attributes where appropriate.
2. Add a `DbSet<Entity>` to [Data/ApplicationDbContext.cs](../../Data/ApplicationDbContext.cs) and configure keys/relationships in `OnModelCreating` (keep `base.OnModelCreating(builder)`).
3. Generate the migration:
   ```powershell
   dotnet ef migrations add Add<Entity> --output-dir Data/Migrations
   ```
4. Show me the generated `Up`/`Down` and pause for review before running `dotnet ef database update`.

Rules: reversible migrations only, never edit the model snapshot by hand, and confirm before any data-loss operation. See the EF migration instructions for details.

Ask me for the entity name and properties if I didn't provide them.
