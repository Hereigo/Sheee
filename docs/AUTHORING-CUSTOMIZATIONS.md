# Authoring Prompts, Instructions, and Skills

A practical guide for writing chat customization files that actually get used. Companion to
[BEGIN.md](./BEGIN.md), which covers the concepts; this file covers the mechanics.

---

## 1. Pick the right primitive

The single most common mistake is putting content in the wrong file type. Decide with this
question: **when should the agent see this?**

| When should the agent see it? | Use | Lives in |
| --- | --- | --- |
| Every single request | agent instructions | `AGENTS.md` **or** `.github/copilot-instructions.md` |
| Whenever a matching file is edited | file instructions | `.github/instructions/*.instructions.md` |
| Only when I explicitly ask for this task | prompt | `.github/prompts/*.prompt.md` |
| Only when this multi-step workflow applies | skill | `.github/skills/<name>/SKILL.md` |

Two follow-up questions settle the remaining ambiguity:

- **Instructions or skill?** Does it apply to *most* work (instructions) or to *specific tasks* (skill)?
- **Prompt or skill?** One focused task with inputs (prompt), or a multi-step procedure with bundled
  scripts/templates (skill)?

Both prompts and skills show up as `/slash-commands` in chat.

---

## 2. The description is the discovery surface

This is the rule that decides whether your customization ever gets loaded.

The agent does **not** read your file to decide if it is relevant. It reads only the `name` and
`description` from the frontmatter — roughly 100 tokens. If your trigger words are not in the
description, the file is invisible.

```yaml
# Invisible — no trigger words
description: "Helpful guidance for the API"

# Discoverable — names the task, the verbs, and the artifacts
description: 'Add or modify an HTTP endpoint in the MyApiApp minimal API. Use for "add an endpoint",
  "new route", "expose a GET/POST", "return 404". Covers Program.cs registration, TypedResults,
  record DTOs, and build verification.'
```

Write descriptions the way a user would phrase the request, including the literal phrases in quotes.
State what it covers **and** what it does not.

---

## 3. Frontmatter cheat sheet

### Prompt (`.prompt.md`)

```yaml
---
description: "Generate xUnit tests for a given endpoint"   # recommended
name: "xUnit Tests"                                        # optional, defaults to filename
argument-hint: "Endpoint or behavior to cover"             # optional, shown in chat input
agent: "agent"                                             # optional: ask | agent | plan | custom
model: ["GPT-5 (copilot)", "Claude Sonnet 4.5 (copilot)"]  # optional, supports fallback
tools: [search, web]                                       # optional, restricts tools
---
```

### File instructions (`.instructions.md`)

```yaml
---
description: "Use when writing database migrations or schema changes."  # required for discovery
applyTo: "**/*.cs"                                                      # optional auto-attach glob
---
```

### Skill (`SKILL.md`)

```yaml
---
name: minimal-api-endpoint      # required, must match the folder name exactly
description: '...'              # required, max 1024 chars
argument-hint: '...'            # optional
user-invocable: true            # optional, false hides the slash command
disable-model-invocation: false # optional, true stops automatic loading
---
```

### Silent failures to watch for

- Unquoted colons in a value break the YAML. Quote it: `description: "Use when: X"`.
- Tabs instead of spaces break the YAML.
- A skill `name` that does not match its folder name — the skill never loads.
- None of these produce an error message. The file is just ignored.

---

## 4. Writing the body

Four principles, in priority order.

**Link, don't embed.** Never copy content that already exists in the repo. Link to it. Copied content
goes stale and burns context on every load.

```markdown
Good:  Follow the conventions in [AGENTS.md](../../AGENTS.md).
Bad:   [40 lines pasted out of AGENTS.md]
```

**Minimal by default.** Only include what the agent *cannot* discover by reading the code. An agent
can see that you use 4-space indentation. It cannot know that `dotnet build` from the repo root fails
with MSB1003 because there is no solution file. Write down the second thing, not the first.

**Concise and actionable.** Every line should change behavior. Delete lines that merely describe.

**Show, don't tell.** A five-line code example beats a paragraph of prose.

---

## 5. Structure that works

### Prompt — a single task

```markdown
Write xUnit tests for the target the user names. If no target is given, ask which endpoint to cover.

## Setup (only if the test project does not exist)
{exact commands}

## Rules
- Test real behavior; do not assert on mocks.
- One behavior per [Fact]; use [Theory] + [InlineData] for variations.

## Validation
Run `dotnet test` and report the real output.
```

### Skill — a repeatable workflow

```markdown
# Skill Title

## When to Use
{bulleted triggers, plus an explicit "do not use this for..."}

## Procedure
1. {numbered, concrete steps}

## Verification
{the exact command, and what failure looks like}

## Guardrails
{what must not change}
```

Keep `SKILL.md` under ~500 lines. Push detail into `./references/*.md` and link it — those files load
only when the agent actually needs them. Bundle scripts in `./scripts/`, templates in `./assets/`,
and always reference them with relative `./` paths, one level deep.

---

## 6. Always include a verification step

This is the highest-value line in most customizations. Give the exact command and demand real output:

```markdown
## Verification

    dotnet build MyApiApp/MyApiApp.csproj

`dotnet build` from the repo root fails with MSB1003 — there is no solution file.
Do not claim success without showing the command output.
```

Without this, an agent will happily report "done" on code that does not compile.

---

## 7. Anti-patterns

| Anti-pattern | Why it hurts | Fix |
| --- | --- | --- |
| `applyTo: "**"` | Loads into every request, relevant or not | Use a specific glob, or move it to agent instructions |
| Vague description | Never discovered, never loaded | Use "Use when..." plus literal user phrasings |
| Multi-task prompt | "create and test and deploy" does none well | One prompt, one task |
| Duplicating docs | Goes stale, wastes context | Link instead |
| Kitchen-sink instructions | Signal drowns in noise | Keep only what applies to *every* task |
| Obvious rules | Linters already enforce them | Delete them |
| Both `AGENTS.md` and `copilot-instructions.md` | Conflicting sources of truth | Pick one |
| Monolithic `SKILL.md` | Blows the context budget on load | Split into `./references/` |

---

## 8. Workflow for creating one

1. Notice friction — you corrected the agent on the same thing twice.
2. Decide the primitive using the table in section 1.
3. Write the description first. If you cannot state a crisp trigger, the customization is too vague.
4. Write the body: procedure, verification, guardrails.
5. Test it by starting a **new chat** and phrasing the request naturally. If it does not load, the
   description is the problem, not the body.
6. Refine when it misfires.

---

## 9. This repo's customizations as examples

| File | Primitive | Why |
| --- | --- | --- |
| [AGENTS.md](../AGENTS.md) | agent instructions | Applies to every request |
| [.github/prompts/xunit-tests.prompt.md](../.github/prompts/xunit-tests.prompt.md) | prompt | One focused task, run on demand |
| [.github/skills/minimal-api-endpoint/SKILL.md](../.github/skills/minimal-api-endpoint/SKILL.md) | skill | Multi-step procedure with verification |

> **Open item:** this repo currently has both [AGENTS.md](../AGENTS.md) and
> [.github/copilot-instructions.md](../.github/copilot-instructions.md). Guidance is to keep only one.
> Their content overlaps today, so consolidate into `AGENTS.md` and delete the other when convenient.

---

## 10. Reference

- [Custom instructions](https://code.visualstudio.com/docs/copilot/customization/custom-instructions)
- [Prompt files](https://code.visualstudio.com/docs/copilot/customization/prompt-files)
- [Agent skills](https://code.visualstudio.com/docs/copilot/customization/agent-skills)

Use `/chronicle improve` to mine your own past sessions for friction worth encoding.
