using System.Collections.Concurrent;
using CleanArch.Application.Abstractions;
using CleanArch.Domain.Entities;

namespace CleanArch.Infrastructure.Persistence;

internal sealed class InMemoryProductRepository : IProductRepository
{
    private readonly ConcurrentDictionary<Guid, Product> _products = new();

    public Task<IReadOnlyList<Product>> ListAsync(CancellationToken cancellationToken = default)
        => Task.FromResult<IReadOnlyList<Product>>(_products.Values.OrderBy(p => p.Name).ToList());

    public Task<Product?> GetAsync(Guid id, CancellationToken cancellationToken = default)
        => Task.FromResult(_products.GetValueOrDefault(id));

    public Task AddAsync(Product product, CancellationToken cancellationToken = default)
    {
        _products[product.Id] = product;
        return Task.CompletedTask;
    }

    public Task<bool> RemoveAsync(Guid id, CancellationToken cancellationToken = default)
        => Task.FromResult(_products.TryRemove(id, out _));
}
