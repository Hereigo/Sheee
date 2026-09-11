# CRUD Feature — Done Checklist

- [ ] Entity created with validation attributes and correct nullability
- [ ] `DbSet<>` added to `ApplicationDbContext`; relationships configured in `OnModelCreating`
- [ ] Migration generated, `Up`/`Down` reviewed, database updated
- [ ] Model snapshot NOT hand-edited
- [ ] Controller with Index/Details/Create/Edit/Delete actions; async EF Core calls
- [ ] `[ValidateAntiForgeryToken]` on all POST actions; over-posting prevented
- [ ] `[Authorize]` applied where the feature needs a signed-in user
- [ ] Strongly typed views with tag helpers and validation UI
- [ ] `dotnet build` clean (no new warnings)
- [ ] CRUD flow smoke-tested via `dotnet run`
- [ ] No secrets or real connection strings committed
