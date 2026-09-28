# Sheee — repo guidance for AI coding agents

This repository is a small ASP.NET Core app centered on [MyApiApp/Program.cs](../MyApiApp/Program.cs). Keep changes minimal and aligned with the existing minimal-API style unless the task explicitly calls for a larger structure.

## Project shape

- Main application: [MyApiApp/](../MyApiApp)
- Startup and endpoint registration: [MyApiApp/Program.cs](../MyApiApp/Program.cs)
- Runtime target and package metadata: [MyApiApp/MyApiApp.csproj](../MyApiApp/MyApiApp.csproj)
- Project context and notes: [docs/BEGIN.md](../docs/BEGIN.md)

## Working conventions

- Prefer a minimal API approach over introducing controllers, layers, or new abstractions unless the task clearly needs them.
- Keep endpoint logic small and readable; do not over-engineer feature setup for this repo.
- Preserve the .NET target framework and package versions unless a task explicitly requires a planned update.
- When adding endpoints, keep the existing ASP.NET Core minimal-API patterns and use the built-in `TypedResults`/`Results<T1,T2>` style when appropriate.
- Prefer the smallest change that solves the task and validate it with a build before finishing.

## Commands

```powershell
dotnet build
dotnet run --project MyApiApp/MyApiApp.csproj
```

## Guardrails

- Do not add large frameworks or redesign the app structure without a clear requirement.
- Do not make unrelated package-version or framework upgrades.
- Keep changes scoped to the feature or fix being requested.
- Prefer existing patterns in the app over introducing new conventions.
