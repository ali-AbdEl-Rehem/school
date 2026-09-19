namespace StudentsAffairs.Application.Common.Exceptions;

/// <summary>Thrown when a requested aggregate does not exist. Mapped to HTTP 404 at the edge.</summary>
public class NotFoundException : Exception
{
    public NotFoundException(string name, object key)
        : base($"\"{name}\" ({key}) was not found.")
    {
    }

    public NotFoundException(string message) : base(message)
    {
    }
}
