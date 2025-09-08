using Microsoft.Extensions.Logging;
using Polly;

namespace EmailPush.Infrastructure.Services;

public interface IRetryService
{
    Task<T> ExecuteWithRetryAsync<T>(Func<Task<T>> operation, int maxRetries = 3, TimeSpan? delay = null);
    Task ExecuteWithRetryAsync(Func<Task> operation, int maxRetries = 3, TimeSpan? delay = null);
}

public class RetryService : IRetryService
{
    private readonly ILogger<RetryService> _logger;

    public RetryService(ILogger<RetryService> logger)
    {
        _logger = logger;
    }

    public async Task<T> ExecuteWithRetryAsync<T>(Func<Task<T>> operation, int maxRetries = 3, TimeSpan? delay = null)
    {
        var baseDelay = delay ?? TimeSpan.FromSeconds(1);
        
        var retryPolicy = Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(
                maxRetries,
                retryAttempt => TimeSpan.FromMilliseconds(baseDelay.TotalMilliseconds * Math.Pow(2, retryAttempt - 1)),
                onRetry: (outcome, timespan, retryCount, context) =>
                {
                    _logger.LogWarning(outcome, "Operation failed on attempt {Attempt} of {MaxRetries}. Retrying in {Delay}ms...", 
                        retryCount, maxRetries, timespan.TotalMilliseconds);
                });

        return await retryPolicy.ExecuteAsync(operation);
    }

    public async Task ExecuteWithRetryAsync(Func<Task> operation, int maxRetries = 3, TimeSpan? delay = null)
    {
        var baseDelay = delay ?? TimeSpan.FromSeconds(1);
        
        var retryPolicy = Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(
                maxRetries,
                retryAttempt => TimeSpan.FromMilliseconds(baseDelay.TotalMilliseconds * Math.Pow(2, retryAttempt - 1)),
                onRetry: (outcome, timespan, retryCount, context) =>
                {
                    _logger.LogWarning(outcome, "Operation failed on attempt {Attempt} of {MaxRetries}. Retrying in {Delay}ms...", 
                        retryCount, maxRetries, timespan.TotalMilliseconds);
                });

        await retryPolicy.ExecuteAsync(operation);
    }
}