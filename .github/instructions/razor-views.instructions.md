---
description: "Use when creating or editing Razor views (.cshtml), layouts, or partials for this MVC app."
applyTo: "**/*.cshtml"
---
# Razor View Conventions

- Views are strongly typed — declare `@model` and use tag helpers (`asp-for`, `asp-action`, `asp-controller`, `asp-route-*`).
- Reuse the shared layout in [Views/Shared/_Layout.cshtml](../../Views/Shared/_Layout.cshtml); place a view under `Views/<Controller>/<Action>.cshtml`.
- Keep POST forms protected — do not remove the auto-generated anti-forgery token.
- Use Bootstrap 5 classes for layout/styling; client libraries live in `wwwroot/lib/`.
- Put validation UI with `asp-validation-for` and include the validation partial `_ValidationScriptsPartial` where forms need client-side validation.
- Avoid inline C# business logic in views — prepare data in the controller/view model instead.
- HTML-encode by default (Razor does this); only use `@Html.Raw` for content you fully trust.
