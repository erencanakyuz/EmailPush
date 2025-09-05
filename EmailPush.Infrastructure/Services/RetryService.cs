using Microsoft.Extensions.Logging;

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
        var retryDelay = delay ?? TimeSpan.FromSeconds(1);
        
        for (int attempt = 0; attempt <= maxRetries; attempt++)
        {
            try
            {
                return await operation();
            }
            catch (Exception ex) when (attempt < maxRetries)
            {
                _logger.LogWarning(ex, "Operation failed on attempt {Attempt} of {MaxRetries}. Retrying in {Delay}ms...", 
                    attempt + 1, maxRetries, retryDelay.TotalMilliseconds);
                
                await Task.Delay(retryDelay);
                retryDelay = TimeSpan.FromMilliseconds(retryDelay.TotalMilliseconds * 2); // Exponential backoff
            }
        }
        
        // Last attempt, let it throw
        return await operation();
    }

    public async Task ExecuteWithRetryAsync(Func<Task> operation, int maxRetries = 3, TimeSpan? delay = null)
    {
        var retryDelay = delay ?? TimeSpan.FromSeconds(1);
        
        for (int attempt = 0; attempt <= maxRetries; attempt++)
        {
            try
            {
                await operation();
                return;
            }
            catch (Exception ex) when (attempt < maxRetries)
            {
                _logger.LogWarning(ex, "Operation failed on attempt {Attempt} of {MaxRetries}. Retrying in {Delay}ms...", 
                    attempt + 1, maxRetries, retryDelay.TotalMilliseconds);
                
                await Task.Delay(retryDelay);
                retryDelay = TimeSpan.FromMilliseconds(retryDelay.TotalMilliseconds * 2); // Exponential backoff
            }
        }
        
        // Last attempt, let it throw
        await operation();
    }
}