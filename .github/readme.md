> ### Created a full set of AI-agent customization files tailored to this ASP.NET Core 10 MVC + EF Core + Identity project:


| File | Type | Purpose |
|------|------|---------|
| `copilot-instructions.md` | Project instructions | Always-on architecture, build/run, migration, and security conventions |
| `csharp.instructions.md` | File instructions (`**/*.cs`) | C#/controller/EF Core rules |
| `razor-views.instructions.md` | File instructions (`**/*.cshtml`) | Razor view + anti-forgery + tag-helper rules |
| `ef-migrations.instructions.md` | On-demand instructions | Safe EF Core migration workflow |
| `add-controller.prompt.md` | Prompt (`/add-controller`) | Scaffold a controller + views |
| `add-entity.prompt.md` | Prompt (`/add-entity`) | Add entity + migration |
| `code-reviewer.agent.md` | Custom agent | Read-only security/convention reviewer |
| `SKILL.md` + `checklist.md` | Skill (`/add-crud-feature`) | Multi-step CRUD feature workflow with bundled checklist |
| `post-edit-reminder.json` | Hook | Post-edit build/format reminder (cross-platform example) |
| `ci.yml` | GitHub Actions | .NET 10 restore + build CI (test step ready to enable) |


> Notes:

- I used `copilot-instructions.md` (the recommended, cross-editor primary) rather than `AGENTS.md` — best practice is to keep only one project-wide instructions file, so pick this one or rename it to `AGENTS.md`, not both.

- The hook and CI workflow are working examples; the CI test step is commented out since there's no test project yet. Add one (e.g. `WebApplication222.Tests`) and uncomment to enable it.

- All content reflects the real stack (net10.0, LocalDB connection, Identity with `RequireConfirmedAccount`, ports 7049/5249).
