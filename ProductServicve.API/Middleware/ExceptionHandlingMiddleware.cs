
namespace ProductServicve.API.Middleware;

// You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext httpContext)
    {
        try
        {
            // Call the next delegate/middleware in the pipeline
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            _logger.LogError("{ExceptionType} : {Message}", ex.GetType(), ex.Message);

            if (ex.InnerException is not null)
            {
                _logger.LogError("Inner Exception: {InnerExceptionType} : {InnerMessage}", ex.InnerException.GetType(), ex.InnerException.Message);
            }

            httpContext.Response.StatusCode = 500;
            // Do not attempt to serialize System.Type; return the type name instead
            await httpContext.Response.WriteAsJsonAsync(new { Message = ex.Message, Type = ex.GetType().FullName });
        }
    }
}

// Extension method used to add the middleware to the HTTP request pipeline.
public static class ExceptionHandlingMiddlewareExtensions
{
    public static IApplicationBuilder UseExceptionHandlingMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}
