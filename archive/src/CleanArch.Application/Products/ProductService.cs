using CleanArch.Application.Abstractions;
using CleanArch.Domain.Entities;

namespace CleanArch.Application.Products;

public sealed class ProductService(IProductRepository repository)
{
    public async Task<IReadOnlyList<ProductResponse>> ListAsync(CancellationToken cancellationToken = default)
    {
        var products = await repository.ListAsync(cancellationToken);
        return products.Select(ToResponse).ToList();
    }

    public async Task<ProductResponse?> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var product = await repository.GetAsync(id, cancellationToken);
        return product is null ? null : ToResponse(product);
    }

    public async Task<ProductResponse> CreateAsync(CreateProductRequest request, CancellationToken cancellationToken = default)
    {
        var product = new Product(request.Name, request.Price);
        await repository.AddAsync(product, cancellationToken);
        return ToResponse(product);
    }

    public Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        => repository.RemoveAsync(id, cancellationToken);

    private static ProductResponse ToResponse(Product product)
        => new(product.Id, product.Name, product.Price);
}
