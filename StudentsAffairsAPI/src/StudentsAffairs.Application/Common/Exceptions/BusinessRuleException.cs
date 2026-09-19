namespace StudentsAffairs.Application.Common.Exceptions;

/// <summary>Thrown when a domain/business invariant is violated (e.g. duplicate email). Mapped to HTTP 409/400 at the edge.</summary>
public class BusinessRuleException : Exception
{
    public BusinessRuleException(string message) : base(message)
    {
    }
}
