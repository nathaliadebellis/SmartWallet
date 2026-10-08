using System.Text.Json;
using SmartWallet.Domain.Exceptions;

namespace SmartWallet.Web.Middleware;

/// <summary>
/// Translates domain exceptions into 400/404 responses. Any other exception is
/// rethrown so the framework error page handles it without leaking details.
/// </summary>
public class ExceptionHandlingMiddleware
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
        catch (Exception ex) when (ex is NotFoundException or DomainException && !context.Response.HasStarted)
        {
            _logger.LogWarning(ex, "Exceção de domínio capturada pelo middleware");

            context.Response.Clear();
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = ex is NotFoundException
                ? StatusCodes.Status404NotFound
                : StatusCodes.Status400BadRequest;

            await context.Response.WriteAsync(JsonSerializer.Serialize(new { message = ex.Message }));
        }
    }
}
