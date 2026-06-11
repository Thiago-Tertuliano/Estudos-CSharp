using MediatR;

namespace ReservEasy.Api.Common.Behaviors;

public class LoggingBehavior<TRequest, TResponse>(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
    {
        logger.LogInformation("Handling {Request}", typeof(TRequest).Name);
        var start = DateTime.UtcNow;
        var response = await next(ct);
        var elapsed = DateTime.UtcNow - start;
        logger.LogInformation("Handled {Request} in {Elapsed}ms", typeof(TRequest).Name, elapsed.TotalMilliseconds);
        return response;
    }
}
