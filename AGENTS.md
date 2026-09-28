# AGENTS.md

## Project
This is a small ASP.NET Core minimal API project.

## Rules
- Prefer minimal API style over controllers, layers, or new abstractions.
- Keep endpoint logic small and readable.
- Use the existing `Program.cs` pattern unless a feature clearly needs a different structure.
- Prefer `Results<T>` / `TypedResults` patterns when returning responses.
- Keep changes scoped and avoid unrelated refactors.
- Do not add large frameworks or architectural complexity without explicit requirement.

## Validation
- Run: `dotnet build`
- If the task changes behavior, validate with the smallest relevant build command.

## Guardrails
- Preserve the target framework and package versions unless a task explicitly requires an update.
- Do not redesign the repo for a feature that can be solved with a small endpoint or helper.
