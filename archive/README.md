# Clean Architecture — ASP.NET Core (.NET 9)

### Minimal four-project clean architecture solution.

```
CleanArch.sln
└── src
    ├── CleanArch.Domain          # Entities and business rules. No dependencies.
    ├── CleanArch.Application     # Use cases, DTOs, port interfaces. -> Domain
    ├── CleanArch.Infrastructure  # Adapters (persistence). -> Application
    └── CleanArch.Web             # Minimal API host + composition root. -> Application, Infrastructure
```

Dependencies point inward only: the Domain knows nothing about the outside, and the Web host
is the only project that wires concrete implementations to the Application's interfaces.

## Run

```powershell
dotnet run --project src/CleanArch.Web
```

OpenAPI document (Development only): `/openapi/v1.json`

## Endpoints

| Method | Route              | Description       |
| ------ | ------------------ | ----------------- |
| GET    | `/products`        | List products     |
| GET    | `/products/{id}`   | Get product by id |
| POST   | `/products`        | Create product    |
| DELETE | `/products/{id}`   | Delete product    |

Persistence is an in-memory adapter; swap `InMemoryProductRepository` for an EF Core
implementation without touching the Domain, Application, or Web layers.

---
## **Structure** (dependencies point inward only):

| Project | References | Contents |
| --- | --- | --- |
| CleanArch.Domain | none | `Product` entity with invariants enforced in the constructor/methods |
| CleanArch.Application | Domain | IProductRepository port, DTOs, ProductService |
| CleanArch.Infrastructure | Application | InMemoryProductRepository adapter (`internal`, exposed only via DI) |
| CleanArch.Web | Application, Infrastructure | Composition root + minimal API endpoints |

**Notes:**
- EF Core was skipped — `dotnet add package` resolved v10.0.x which isn't compatible with `net9.0`. The repository interface means you can drop in an EF Core adapter later by changing only the Infrastructure project.
- Endpoints use `TypedResults` with `Results<T1,T2>` unions, so the OpenAPI document gets accurate response types without attributes.
- `AddProblemDetails()` + `UseExceptionHandler()` ensure domain exceptions surface as RFC 9457 responses rather than stack traces.

Run with .

Made changes.