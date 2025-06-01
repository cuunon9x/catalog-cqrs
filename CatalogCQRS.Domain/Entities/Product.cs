using CatalogCQRS.Domain.Common;
using CatalogCQRS.Domain.Exceptions;

namespace CatalogCQRS.Domain.Entities;

public class Product : Entity
{
    public string Name { get; private set; }
    public List<string> Category { get; private set; }
    public string Description { get; private set; }
    public string ImageFile { get; private set; }    public Money Price { get; private set; }

    // For ORM
    private Product() : base() { }

    public Product(
        string name,
        List<string> category,
        string description,
        string imageFile,
        Money price) : base()
    {
        UpdateProduct(name, category, description, imageFile, price);
    }    public void UpdateProduct(
        string name,
        List<string> category,
        string description,
        string imageFile,
        Money price)
    {
        ValidateProduct(name, category, description, price);

        Name = name;
        Category = category;
        Description = description;
        ImageFile = imageFile;
        Price = price;
    }

    private void ValidateProduct(string name, List<string> category, string description, Money price)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Product name cannot be empty", nameof(name));

        if (category == null || !category.Any())
            throw new ArgumentException("Product must have at least one category", nameof(category));

        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Product description cannot be empty", nameof(description));

        if (price == null)
            throw new ArgumentException("Product price cannot be null", nameof(price));
    }
}
