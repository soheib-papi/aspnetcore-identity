namespace aspnetcore_identity.Middlewares;

public class GlobalExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

    public GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger)
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
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred.");

            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = exception switch
        {
            ArgumentException => StatusCodes.Status400BadRequest, // خطاهای ولیدیشن
            UnauthorizedAccessException => StatusCodes.Status401Unauthorized, // عدم احراز هویت
            KeyNotFoundException => StatusCodes.Status404NotFound, // داده‌ای پیدا نشد
            _ => StatusCodes.Status500InternalServerError // خطای عمومی سرور
        };

        var errorResponse = new
        {
            StatusCode = context.Response.StatusCode,
            Message = exception.Message,
            Detail = exception.InnerException?.Message
        };

        return context.Response.WriteAsJsonAsync(errorResponse);
    }
}
