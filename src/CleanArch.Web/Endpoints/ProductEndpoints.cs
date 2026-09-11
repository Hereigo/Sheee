using CleanArch.Application.Products;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CleanArch.Web.Endpoints;

public static class ProductEndpoints
{
    public static IEndpointRouteBuilder MapProductEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/products").WithTags("Products");

        group.MapGet("/", async (ProductService service, CancellationToken ct)
            => TypedResults.Ok(await service.ListAsync(ct)));

        group.MapGet("/{id:guid}", async Task<Results<Ok<ProductResponse>, NotFound>> (
            Guid id, ProductService service, CancellationToken ct) =>
        {
            var product = await service.GetAsync(id, ct);
            return product is null ? TypedResults.NotFound() : TypedResults.Ok(product);
        });

        group.MapPost("/", async Task<Results<Created<ProductResponse>, ValidationProblem>> (
            CreateProductRequest request, ProductService service, CancellationToken ct) =>
        {
            if (string.IsNullOrWhiteSpace(request.Name) || request.Price < 0)
            {
                return TypedResults.ValidationProblem(new Dictionary<string, string[]>
                {
                    [nameof(request.Name)] = ["Name is required and price must be non-negative."]
                });
            }

            var created = await service.CreateAsync(request, ct);
            return TypedResults.Created($"/products/{created.Id}", created);
        });

        group.MapDelete("/{id:guid}", async Task<Results<NoContent, NotFound>> (
            Guid id, ProductService service, CancellationToken ct)
            => await service.DeleteAsync(id, ct) ? TypedResults.NoContent() : TypedResults.NotFound());

        return app;
    }
}
