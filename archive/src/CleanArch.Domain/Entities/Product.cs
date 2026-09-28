namespace CleanArch.Domain.Entities;

public sealed class Product
{
    public Product(string name, decimal price)
    {
        Rename(name);
        Reprice(price);
    }

    public Guid Id { get; } = Guid.NewGuid();

    public string Name { get; private set; } = null!;

    public decimal Price { get; private set; }

    public void Rename(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name is required.", nameof(name));
        }

        Name = name.Trim();
    }

    public void Reprice(decimal price)
    {
        if (price < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(price), "Price cannot be negative.");
        }

        Price = price;
    }
}
