namespace CatalogCQRS.Domain.Exceptions;

public class DomainException : Exception
{
    public DomainException()
    { }

    public DomainException(string message)
        : base(message)
    { }

    public DomainException(string message, Exception innerException)
        : base(message, innerException)
    { }
}

public class ProductNotFoundException : DomainException
{
    public ProductNotFoundException(Guid id)
        : base($"Product with ID {id} was not found.")
    { }
}

public class InvalidProductException : DomainException
{
    public InvalidProductException(string message)
        : base(message)
    { }
}

public class ValidationException : DomainException
{
    public ValidationException(IEnumerable<string> errors)
        : base("One or more validation failures have occurred.")
    {
        Errors = errors;
    }

    public IEnumerable<string> Errors { get; }
}
