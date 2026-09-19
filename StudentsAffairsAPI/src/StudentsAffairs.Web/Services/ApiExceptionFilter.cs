using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using StudentsAffairs.Application.Common.Exceptions;

namespace StudentsAffairs.Web.Services;

/// <summary>
/// Global MVC exception filter: turns Application-layer exceptions raised by the API
/// controllers into RFC 7807 ProblemDetails responses. Blazor requests never hit this
/// (they are not MVC actions) and keep using the <c>/Error</c> page.
/// </summary>
public sealed class ApiExceptionFilter : IExceptionFilter
{
    private readonly ILogger<ApiExceptionFilter> _logger;

    public ApiExceptionFilter(ILogger<ApiExceptionFilter> logger) => _logger = logger;

    public void OnException(ExceptionContext context)
    {
        var (status, title) = context.Exception switch
        {
            NotFoundException => (StatusCodes.Status404NotFound, "Resource not found"),
            BusinessRuleException => (StatusCodes.Status409Conflict, "Business rule violated"),
            _ => (StatusCodes.Status500InternalServerError, "An unexpected error occurred")
        };

        if (status == StatusCodes.Status500InternalServerError)
        {
            _logger.LogError(context.Exception, "Unhandled exception in API action");
            return; // let the framework produce the 500 (and dev exception page)
        }

        _logger.LogWarning("{Title}: {Message}", title, context.Exception.Message);

        var problem = new ProblemDetails
        {
            Status = status,
            Title = title,
            Detail = context.Exception.Message,
            Type = $"https://httpstatuses.io/{status}",
            Instance = context.HttpContext.Request.Path
        };
        problem.Extensions["traceId"] = context.HttpContext.TraceIdentifier;

        context.Result = new ObjectResult(problem) { StatusCode = status };
        context.ExceptionHandled = true;
    }
}
