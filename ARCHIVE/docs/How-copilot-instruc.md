# AI - Copilot Instructions for Modern ASP.NET Application  

## 🎯 Purpose
This document provides guidelines for using GitHub Copilot effectively in our ASP.NET solution. It ensures Copilot-generated code aligns with our architecture, coding standards, and security practices.
---

## Solution Overview
- **Framework**: ASP.NET Core 8.0
- **Pattern**: Clean Architecture (Presentation → Application → Domain → Infrastructure)
- **UI**: Razor Pages + minimal APIs
- **Database**: EF Core with SQL Server
- **Authentication**: ASP.NET Identity + JWT
- **Deployment**: Docker + Kubernetes

---

## ⚙️ Copilot Usage Guidelines

### ✅ Appropriate Use
- Scaffolding boilerplate code (controllers, DTOs, EF Core configurations).
- Common patterns (dependency injection, middleware setup).
- Unit test scaffolding.
- Generating LINQ queries and EF Core mappings.
- Suggesting Razor syntax for views.
- Drafting middleware and dependency injection setup.

### ❌ Restricted Use
- Security-sensitive code (authentication, encryption, cryptography).
- Business-critical algorithms without review.
- Configuration secrets (connection strings, API keys).
- Copy-pasting large unverified blocks.

---

## 📐 Coding Standards
- **Naming**: PascalCase for classes/methods, camelCase for variables.
- **Async**: Use `async/await` with `Task` return types.
- **Dependency Injection**: Register services in `Program.cs`.
- **Logging**: Use `ILogger<T>` with structured logging.
- **Validation**: Use FluentValidation for request models.

---

## 🔒 Security Practices
- Never hardcode secrets; use environment variables or Key Vault.
- Always validate user input (FluentValidation + DataAnnotations).
- Use HTTPS redirection and HSTS middleware.
- Apply `[Authorize]` attributes for protected endpoints.
- Sanitize output in Razor views.

---

## 🧪 Testing Strategy
- **Unit Tests**: xUnit + Moq for services and repositories.
- **Integration Tests**: EF Core InMemory + TestServer.
- **API Tests**: Postman/Newman collections.
- **CI/CD**: GitHub Actions with automated test runs.

---

## 🚀 Example Copilot Prompts

### Controller or Minimal API Endpoint
```csharp
// Prompt: Create API endpoint to fetch products by category
app.MapGet("/products/{category}", async (string category, IProductService service) =>
{
    var products = await service.GetByCategoryAsync(category);
    return Results.Ok(products);
});
```

### EF Core Entity
```csharp
// Prompt: Generate EF Core entity with validation
public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Category { get; set; } = string.Empty;
}
```

### Unit Test
```csharp
// Prompt: Write xUnit test for ProductService GetByCategoryAsync
[Fact]
public async Task GetByCategoryAsync_ReturnsProducts()
{
    var mockRepo = new Mock<IProductRepository>();
    mockRepo.Setup(r => r.GetByCategoryAsync("Books"))
            .ReturnsAsync(new List<Product> { new Product { Name = "C# in Depth" } });

    var service = new ProductService(mockRepo.Object);
    var result = await service.GetByCategoryAsync("Books");

    Assert.Single(result);
    Assert.Equal("C# in Depth", result.First().Name);
}
```

---

## 📋 Review Process
- All Copilot-generated code must undergo **peer review**.
- Ensure adherence to **SOLID principles**.
- Run **static analysis** (SonarQube, Roslyn analyzers).
- Validate against **OWASP Top 10** security risks.

---

## 🛠️ Productivity Tips
- Use Copilot for **scaffolding repetitive code**.
- Refine prompts with **specific context** (e.g., "Generate EF Core migration for Product entity").
- Pair Copilot with **IntelliSense** for better suggestions.
- Regularly update Copilot to leverage new features.

---

👉 This file is designed as a **template**: you can adjust the architecture section, swap out testing frameworks, or add your own company-specific rules.