---
description: "Create or extend xUnit tests for the MyApiApp minimal API"
name: "xUnit Tests"
argument-hint: "Endpoint, helper, or behavior to cover"
agent: "agent"
---

Write xUnit tests for the target the user names. If no target is given, ask which endpoint or behavior to cover.

## Test project setup (only if `tests/MyApiApp.Tests` does not exist yet)

```powershell
dotnet new xunit -o tests/MyApiApp.Tests
dotnet add tests/MyApiApp.Tests reference MyApiApp/MyApiApp.csproj
dotnet add tests/MyApiApp.Tests package Microsoft.AspNetCore.Mvc.Testing
```

- Match the app's `TargetFramework` in [MyApiApp/MyApiApp.csproj](../../MyApiApp/MyApiApp.csproj); do not upgrade or downgrade it.
- Endpoint tests use `WebApplicationFactory<Program>`, which requires `public partial class Program { }` at the end of [MyApiApp/Program.cs](../../MyApiApp/Program.cs). Add that single line if it is missing.

## Rules

- Test real behavior through the HTTP pipeline or the public API surface. Do not assert on mocks.
- One behavior per `[Fact]`; use `[Theory]` + `[InlineData]` for input variations.
- Name tests `MethodOrRoute_Scenario_ExpectedResult`.
- Cover the happy path plus the error and edge cases that the code can actually produce.
- Do not add helpers, abstractions, or base classes for a single test.
- Do not add test-only members to production code beyond the `partial class Program` hook.
- Follow the repo conventions in [AGENTS.md](../../AGENTS.md).

## Validation

Run and report the real output:

```powershell
dotnet test
```

Do not claim the tests pass without showing the test run result.
