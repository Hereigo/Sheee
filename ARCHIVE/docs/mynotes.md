Created `copilot-instructions.md`.

It front-loads the facts an agent would otherwise burn tokens rediscovering: project layout and dependency rule in one table, file-level pointers for each layer, the code conventions actually used (sealed types, records, primary constructors, `TypedResults` unions, `CancellationToken` flow), where new code belongs, build/run commands, and the EF Core version pitfall noted in the README.

-----

Here's a staged roadmap. Adopt in this order — each step pays off before the next adds complexity.

## Stage 1 — you're here: repo instructions ✅

`copilot-instructions.md` (or `AGENTS.md` at root). Always loaded, so keep it under ~50 lines. Two rules that matter most:

- **Facts the agent can't cheaply discover** (dependency rules, gotchas, build commands) — not things it can read from a `.csproj`.
- **Prune it.** Every line costs tokens on *every* request. If a rule never changes agent behavior, delete it.

> `AGENTS.md` vs `copilot-instructions.md`: same job, `AGENTS.md` is the cross-tool standard (Copilot, Cursor, Codex all read it). If you might switch tools, prefer `AGENTS.md`. Don't maintain both.

## Stage 2 — file-scoped instructions (do this next)

`.github/instructions/*.instructions.md`. Loaded **only** when matching files are touched — this is the main token-saving mechanism.

Split by concern, one file each:

```
.github/instructions/
  csharp-style.instructions.md      applyTo: "**/*.cs"
  tests.instructions.md             applyTo: "tests/**/*.cs"
  endpoints.instructions.md         applyTo: "src/**/Endpoints/*.cs"
  domain.instructions.md            applyTo: "src/CleanArch.Domain/**"
```

Example:

```markdown
---
description: "xUnit test conventions for this solution"
applyTo: "tests/**/*.cs"
---
- xUnit + FluentAssertions. One `[Fact]` per behavior, `[Theory]` for input tables.
- Name: `Method_Scenario_ExpectedResult`.
- Test Application/Domain directly; use `WebApplicationFactory<Program>` only for endpoint tests.
- Never assert on `InMemoryProductRepository` internals — go through `IProductRepository`.
```

Rules for doing it right:
- **Never `applyTo: "**"`** — that's just repo instructions with extra steps, and it burns context every turn.
- `description` is how the agent finds the file when no glob matches — write it as "Use when…" with trigger keywords.
- One concern per file. Mixed files get loaded for irrelevant reasons.

## Stage 3 — prompt files for repetitive tasks

`.github/prompts/*.prompt.md`, invoked with `/name` in chat. Your repo has an obvious candidate: adding a feature means touching all four layers in the same order every time.

`.github/prompts/new-feature-slice.prompt.md`:

```markdown
---
description: "Scaffold a new vertical slice across Domain → Application → Infrastructure → Web"
argument-hint: "entity name, e.g. Order"
agent: agent
---
Add a vertical slice for the entity named in my request. Follow the existing
`Product` slice exactly. In order:

1. Domain: sealed entity, invariants in ctor + mutator methods.
2. Application: `I{Entity}Repository` port, request/response records, `{Entity}Service`.
3. Infrastructure: `InMemory{Entity}Repository` (internal), register in `DependencyInjection.cs`.
4. Web: `{Entity}Endpoints.cs` with `MapGroup` + `TypedResults` unions, map in `Program.cs`.

Then run `dotnet build CleanArch.sln` and fix errors.
Do not add packages. Do not touch the `Product` slice.
```

This is a big win: one slash command replaces a 200-word prompt you'd otherwise retype.

## Stage 4 — a read-only reviewer agent

`.github/agents/*.agent.md`. The point is **context isolation + tool restriction**. For clean architecture, a dependency-rule cop is genuinely useful because the violation is easy to introduce and easy to miss:

```markdown
---
description: "Use when reviewing changes for clean-architecture violations, layering, or dependency-rule breaks"
tools: [read, search]
---
You audit this solution's dependency rule. Report only; never edit.

Check for:
- `using CleanArch.Infrastructure` inside Application or Domain
- `ProjectReference` edges that point outward
- Domain types leaking through Web endpoints instead of DTOs
- Business rules living in services/endpoints instead of the entity

Output a numbered findings list: file, line, rule broken, suggested fix. If clean, say "No violations."
```

`tools: [read, search]` means it physically cannot edit — much stronger than telling the main agent "please don't."

## Stage 5 — hooks (only when guidance isn't enough)

`.github/hooks/*.json`. Instructions *suggest*; hooks *enforce* via shell commands at lifecycle events. Reach for them only when a rule must never be violated:

```json
{
  "hooks": {
    "PostToolUse": [
      { "type": "command", "command": "dotnet format CleanArch.sln --no-restore", "timeout": 60 }
    ]
  }
}
```

Useful `PreToolUse` case for .NET: block `dotnet add package` without a pinned version (your README's EF Core 10.0.x trap). Exit code `2` blocks the tool call.

Keep hooks fast — they run inline and stall the agent.

## Skills — skip for now

`SKILL.md` = multi-step workflow *with bundled assets* (scripts, templates). Worth it when you have, say, an EF Core migration procedure with helper scripts. With four projects and no tests yet, instructions + prompts cover you.

---

## What actually moves the needle

| Technique | Token effect | Effort |
| --- | --- | --- |
| Trim repo instructions | Saves on **every** turn | Low |
| `applyTo`-scoped instructions | Large — loads only when relevant | Low |
| Prompt files | Saves your typing, keeps prompts consistent | Low |
| Custom agents | Large — subagent context doesn't pollute yours | Medium |
| Hooks | Neutral, but prevents costly wrong turns | Medium |

Two habits that matter more than any file:

1. **Say "read `copilot-instructions.md` first, don't explore"** when starting a task. Agents burn most tokens on redundant searching.
2. **Start a new chat per task.** A long thread re-sends its whole history every turn; this is usually the single biggest cost driver.

---
---
**`new-feature-slice.prompt.md`**

Type `/new-feature-slice Order` in chat. It links the six `Product` reference files as context, walks the four layers in dependency order with the exact conventions your code uses (sealed, records, primary ctors, `AddScoped` vs `AddSingleton`, `Results<T1,T2>` unions), then builds and fixes errors.

**`dependency-rule.agent.md`**

Pick it from the agent selector, or the main agent delegates to it automatically when you ask for an architecture review. `tools: [read, search]` means it structurally cannot edit files. Seven concrete violation classes, a fixed output format, and `No violations.` as the clean-result response so you can tell "clean" from "didn't really look."

Two notes on how they were written:

- The prompt uses **relative markdown links** rather than pasted code — the agent pulls those files only when the prompt runs.

- The agent's `description` is keyword-heavy ("dependency-rule breaks", "layering", "domain logic leaking") because that string is the only thing the main agent sees when deciding whether to delegate.


Try `/new-feature-slice Order`, then run the reviewer on the result — a good way to see both working together.

