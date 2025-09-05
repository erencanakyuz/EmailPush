using System.Diagnostics;

namespace EmailPush.Api.Middleware;

public class MonitoringMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<MonitoringMiddleware> _logger;

    public MonitoringMiddleware(RequestDelegate next, ILogger<MonitoringMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();
        
        // Log request start
        _logger.LogInformation("Request started: {Method} {Path}", context.Request.Method, context.Request.Path);

        try
        {
            await _next(context);
        }
        finally
        {
            stopwatch.Stop();
            
            // Log request completion with duration
            _logger.LogInformation(
                "Request completed: {Method} {Path} - Status: {StatusCode} - Duration: {Duration}ms",
                context.Request.Method,
                context.Request.Path,
                context.Response.StatusCode,
                stopwatch.ElapsedMilliseconds);
        }
    }
}