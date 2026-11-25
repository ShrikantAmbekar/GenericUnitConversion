using System.Text.Json;
using UnitConversion.Domain.Exceptions;

namespace UnitConversion.Api.Middleware;

public sealed class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            await HandleExceptionAsync(context, exception);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var (statusCode, message) = exception switch
        {
            UnitNotFoundException unitEx => (StatusCodes.Status404NotFound, unitEx.Message),
            CrossCategoryConversionException crossEx => (StatusCodes.Status400BadRequest, crossEx.Message),
            InvalidConversionValueException invalidEx => (StatusCodes.Status400BadRequest, invalidEx.Message),
            DomainException domainEx => (StatusCodes.Status400BadRequest, domainEx.Message),
            _ => (StatusCodes.Status500InternalServerError, "An unexpected error occurred.")
        };

        if (statusCode == StatusCodes.Status500InternalServerError)
        {
            _logger.LogError(exception, "Unhandled exception processing request.");
        }
        else
        {
            _logger.LogWarning(exception, "Request failed with {StatusCode}", statusCode);
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        var payload = JsonSerializer.Serialize(new
        {
            status = statusCode,
            message
        });

        await context.Response.WriteAsync(payload);
    }
}