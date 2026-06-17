namespace Notifica.Domain.Interfaces;
public interface IMessageBusService
{
    Task PublishAsync<T>(string queue, T message, CancellationToken ct = default);
    Task SubscribeAsync<T>(string queue, Func<T, Task> handler, CancellationToken ct = default);
}
