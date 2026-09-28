---
description: "Scaffold a new MVC controller with its Razor views following this project's conventions."
argument-hint: "Controller name and the actions/pages it needs"
agent: "agent"
---
Create a new MVC controller and its views in this ASP.NET Core project.

Requirements:
- Name the class `<Name>Controller`, place it in [Controllers/](../../Controllers/), namespace `WebApplication222.Controllers`.
- Return `IActionResult` from actions; use `async Task<IActionResult>` when touching the database.
- Inject `ApplicationDbContext` and any services via the constructor.
- Create matching views under `Views/<Name>/<Action>.cshtml` using the shared layout and tag helpers.
- Add `[Authorize]` if the feature requires a signed-in user.
- Keep `[ValidateAntiForgeryToken]` on POST actions and the anti-forgery token in forms.
- After creating, build with `dotnet build` and fix any warnings/errors.

Ask me for the controller name and actions if I didn't specify them.
