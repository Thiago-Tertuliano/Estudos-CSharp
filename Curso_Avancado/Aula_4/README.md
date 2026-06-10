# Aula 4 - Microservices Architecture

## Objetivos da Aula
- Entender arquitetura de microservices
- Aprender padrões de comunicação entre serviços
- Compreender service discovery e load balancing
- Praticar implementação de microservices

## Conteúdo Teórico

### Conceitos Fundamentais

#### O que são Microservices?
Microservices são uma abordagem arquitetural onde uma aplicação é dividida em serviços menores, independentes e autônomos.

```csharp
// Exemplo de divisão em microservices
// UserService - Gerencia usuários
// OrderService - Gerencia pedidos
// PaymentService - Processa pagamentos
// NotificationService - Envia notificações
```

#### Vantagens dos Microservices
- **Escalabilidade**: Escalar serviços independentemente
- **Flexibilidade**: Tecnologias diferentes por serviço
- **Resiliência**: Falha isolada
- **Manutenibilidade**: Código menor e focado

### Service Communication

#### HTTP/REST Communication
```csharp
public interface IUserServiceClient
{
    Task<User> GetUserAsync(int userId);
    Task<User> CreateUserAsync(User user);
    Task UpdateUserAsync(User user);
    Task DeleteUserAsync(int userId);
}

public class UserServiceClient : IUserServiceClient
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;
    
    public UserServiceClient(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _baseUrl = configuration["UserService:BaseUrl"];
    }
    
    public async Task<User> GetUserAsync(int userId)
    {
        var response = await _httpClient.GetAsync($"{_baseUrl}/users/{userId}");
        response.EnsureSuccessStatusCode();
        
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<User>(content);
    }
    
    public async Task<User> CreateUserAsync(User user)
    {
        var json = JsonSerializer.Serialize(user);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        
        var response = await _httpClient.PostAsync($"{_baseUrl}/users", content);
        response.EnsureSuccessStatusCode();
        
        var responseContent = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<User>(responseContent);
    }
}
```

#### gRPC Communication
```csharp
// Proto file (user.proto)
syntax = "proto3";

package UserService;

service UserService {
    rpc GetUser (GetUserRequest) returns (User);
    rpc CreateUser (CreateUserRequest) returns (User);
    rpc UpdateUser (UpdateUserRequest) returns (User);
    rpc DeleteUser (DeleteUserRequest) returns (DeleteUserResponse);
}

message GetUserRequest {
    int32 user_id = 1;
}

message CreateUserRequest {
    string name = 1;
    string email = 2;
}

message User {
    int32 id = 1;
    string name = 2;
    string email = 3;
}
```

```csharp
// gRPC Client
public class UserServiceGrpcClient : IUserServiceClient
{
    private readonly UserService.UserServiceClient _client;
    
    public UserServiceGrpcClient(UserService.UserServiceClient client)
    {
        _client = client;
    }
    
    public async Task<User> GetUserAsync(int userId)
    {
        var request = new GetUserRequest { UserId = userId };
        var response = await _client.GetUserAsync(request);
        
        return new User
        {
            Id = response.Id,
            Name = response.Name,
            Email = response.Email
        };
    }
}
```

### Message Queues

#### RabbitMQ Implementation
```csharp
public interface IMessageBroker
{
    Task PublishAsync<T>(string queue, T message);
    Task SubscribeAsync<T>(string queue, Func<T, Task> handler);
}

public class RabbitMQBroker : IMessageBroker, IDisposable
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    
    public RabbitMQBroker(IConfiguration configuration)
    {
        var factory = new ConnectionFactory
        {
            HostName = configuration["RabbitMQ:Host"],
            UserName = configuration["RabbitMQ:Username"],
            Password = configuration["RabbitMQ:Password"]
        };
        
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
    }
    
    public async Task PublishAsync<T>(string queue, T message)
    {
        _channel.QueueDeclare(queue, durable: true, exclusive: false, autoDelete: false);
        
        var json = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(json);
        
        _channel.BasicPublish("", queue, null, body);
        await Task.CompletedTask;
    }
    
    public async Task SubscribeAsync<T>(string queue, Func<T, Task> handler)
    {
        _channel.QueueDeclare(queue, durable: true, exclusive: false, autoDelete: false);
        
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var json = Encoding.UTF8.GetString(body);
            var message = JsonSerializer.Deserialize<T>(json);
            
            await handler(message);
            _channel.BasicAck(ea.DeliveryTag, false);
        };
        
        _channel.BasicConsume(queue, false, consumer);
        await Task.CompletedTask;
    }
    
    public void Dispose()
    {
        _channel?.Dispose();
        _connection?.Dispose();
    }
}
```

### Service Discovery

#### Consul Service Discovery
```csharp
public interface IServiceDiscovery
{
    Task<string> GetServiceUrlAsync(string serviceName);
    Task RegisterServiceAsync(string serviceName, string serviceUrl);
    Task DeregisterServiceAsync(string serviceName);
}

public class ConsulServiceDiscovery : IServiceDiscovery
{
    private readonly IConsulClient _consulClient;
    
    public ConsulServiceDiscovery(IConsulClient consulClient)
    {
        _consulClient = consulClient;
    }
    
    public async Task<string> GetServiceUrlAsync(string serviceName)
    {
        var response = await _consulClient.Health.Service(serviceName);
        
        if (response.Response.Any())
        {
            var service = response.Response.First();
            return $"http://{service.Service.Address}:{service.Service.Port}";
        }
        
        throw new ServiceNotFoundException($"Service {serviceName} not found");
    }
    
    public async Task RegisterServiceAsync(string serviceName, string serviceUrl)
    {
        var registration = new AgentServiceRegistration
        {
            ID = serviceName,
            Name = serviceName,
            Address = new Uri(serviceUrl).Host,
            Port = new Uri(serviceUrl).Port,
            Check = new AgentServiceCheck
            {
                HTTP = $"{serviceUrl}/health",
                Interval = TimeSpan.FromSeconds(10)
            }
        };
        
        await _consulClient.Agent.ServiceRegister(registration);
    }
}
```

### API Gateway

#### Ocelot Implementation
```csharp
// ocelot.json
{
  "Routes": [
    {
      "DownstreamPathTemplate": "/api/users/{everything}",
      "DownstreamScheme": "http",
      "DownstreamHostAndPorts": [
        {
          "Host": "localhost",
          "Port": 5001
        }
      ],
      "UpstreamPathTemplate": "/users/{everything}",
      "UpstreamHttpMethod": [ "GET", "POST", "PUT", "DELETE" ]
    },
    {
      "DownstreamPathTemplate": "/api/orders/{everything}",
      "DownstreamScheme": "http",
      "DownstreamHostAndPorts": [
        {
          "Host": "localhost",
          "Port": 5002
        }
      ],
      "UpstreamPathTemplate": "/orders/{everything}",
      "UpstreamHttpMethod": [ "GET", "POST", "PUT", "DELETE" ]
    }
  ],
  "GlobalConfiguration": {
    "BaseUrl": "http://localhost:5000"
  }
}
```

```csharp
// Program.cs
var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);

builder.Services.AddOcelot(builder.Configuration);

var app = builder.Build();

await app.UseOcelot();

app.Run();
```

### Circuit Breaker Pattern

#### Polly Implementation
```csharp
public interface IResilientHttpClient
{
    Task<T> GetAsync<T>(string url);
    Task<T> PostAsync<T>(string url, object data);
}

public class ResilientHttpClient : IResilientHttpClient
{
    private readonly HttpClient _httpClient;
    private readonly IAsyncPolicy<HttpResponseMessage> _circuitBreakerPolicy;
    
    public ResilientHttpClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
        
        _circuitBreakerPolicy = Policy<HttpResponseMessage>
            .Handle<HttpRequestException>()
            .Or<TimeoutException>()
            .CircuitBreakerAsync(
                exceptionsAllowedBeforeBreaking: 3,
                durationOfBreak: TimeSpan.FromSeconds(30)
            );
    }
    
    public async Task<T> GetAsync<T>(string url)
    {
        var response = await _circuitBreakerPolicy.ExecuteAsync(async () =>
        {
            return await _httpClient.GetAsync(url);
        });
        
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(content);
    }
    
    public async Task<T> PostAsync<T>(string url, object data)
    {
        var json = JsonSerializer.Serialize(data);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        
        var response = await _circuitBreakerPolicy.ExecuteAsync(async () =>
        {
            return await _httpClient.PostAsync(url, content);
        });
        
        response.EnsureSuccessStatusCode();
        var responseContent = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(responseContent);
    }
}
```

### Distributed Tracing

#### OpenTelemetry Implementation
```csharp
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDistributedTracing(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOpenTelemetry()
            .WithTracing(builder =>
            {
                builder
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddSqlClientInstrumentation()
                    .AddJaegerExporter(options =>
                    {
                        options.AgentHost = configuration["Jaeger:Host"];
                        options.AgentPort = int.Parse(configuration["Jaeger:Port"]);
                    });
            });
        
        return services;
    }
}
```

### Event Sourcing

#### Event Store Implementation
```csharp
public interface IEventStore
{
    Task SaveEventsAsync(string aggregateId, IEnumerable<IEvent> events, int expectedVersion);
    Task<IEnumerable<IEvent>> GetEventsAsync(string aggregateId);
}

public class EventStore : IEventStore
{
    private readonly IEventPublisher _eventPublisher;
    private readonly Dictionary<string, List<IEvent>> _events = new();
    
    public EventStore(IEventPublisher eventPublisher)
    {
        _eventPublisher = eventPublisher;
    }
    
    public async Task SaveEventsAsync(string aggregateId, IEnumerable<IEvent> events, int expectedVersion)
    {
        if (!_events.ContainsKey(aggregateId))
        {
            _events[aggregateId] = new List<IEvent>();
        }
        
        var eventList = _events[aggregateId];
        
        if (eventList.Count != expectedVersion)
        {
            throw new ConcurrencyException();
        }
        
        foreach (var @event in events)
        {
            @event.Version = expectedVersion++;
            eventList.Add(@event);
            
            await _eventPublisher.PublishAsync(@event);
        }
    }
    
    public async Task<IEnumerable<IEvent>> GetEventsAsync(string aggregateId)
    {
        if (_events.ContainsKey(aggregateId))
        {
            return _events[aggregateId];
        }
        
        return new List<IEvent>();
    }
}
```

### CQRS Pattern

#### Command and Query Separation
```csharp
// Commands
public interface ICommand
{
}

public interface ICommandHandler<TCommand> where TCommand : ICommand
{
    Task HandleAsync(TCommand command);
}

public class CreateUserCommand : ICommand
{
    public string Name { get; set; }
    public string Email { get; set; }
}

public class CreateUserCommandHandler : ICommandHandler<CreateUserCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly IEventStore _eventStore;
    
    public CreateUserCommandHandler(IUserRepository userRepository, IEventStore eventStore)
    {
        _userRepository = userRepository;
        _eventStore = eventStore;
    }
    
    public async Task HandleAsync(CreateUserCommand command)
    {
        var user = new User
        {
            Name = command.Name,
            Email = command.Email
        };
        
        await _userRepository.AddAsync(user);
        
        var @event = new UserCreatedEvent
        {
            UserId = user.Id,
            Name = user.Name,
            Email = user.Email
        };
        
        await _eventStore.SaveEventsAsync(user.Id.ToString(), new[] { @event }, 0);
    }
}

// Queries
public interface IQuery<TResult>
{
}

public interface IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
{
    Task<TResult> HandleAsync(TQuery query);
}

public class GetUserQuery : IQuery<User>
{
    public int UserId { get; set; }
}

public class GetUserQueryHandler : IQueryHandler<GetUserQuery, User>
{
    private readonly IUserRepository _userRepository;
    
    public GetUserQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    public async Task<User> HandleAsync(GetUserQuery query)
    {
        return await _userRepository.GetByIdAsync(query.UserId);
    }
}
```

### Saga Pattern

#### Orchestration-based Saga
```csharp
public interface ISaga
{
    Task ExecuteAsync();
    Task CompensateAsync();
}

public class OrderSaga : ISaga
{
    private readonly IOrderService _orderService;
    private readonly IUserService _userService;
    private readonly IPaymentService _paymentService;
    private readonly INotificationService _notificationService;
    
    public OrderSaga(
        IOrderService orderService,
        IUserService userService,
        IPaymentService paymentService,
        INotificationService notificationService)
    {
        _orderService = orderService;
        _userService = userService;
        _paymentService = paymentService;
        _notificationService = notificationService;
    }
    
    public async Task ExecuteAsync()
    {
        try
        {
            // Step 1: Validate user
            await _userService.ValidateUserAsync(userId);
            
            // Step 2: Create order
            var order = await _orderService.CreateOrderAsync(orderData);
            
            // Step 3: Process payment
            await _paymentService.ProcessPaymentAsync(order.PaymentInfo);
            
            // Step 4: Send notification
            await _notificationService.SendOrderConfirmationAsync(order);
        }
        catch (Exception)
        {
            await CompensateAsync();
            throw;
        }
    }
    
    public async Task CompensateAsync()
    {
        // Compensate in reverse order
        await _notificationService.CancelNotificationAsync();
        await _paymentService.RefundPaymentAsync();
        await _orderService.CancelOrderAsync();
    }
}
```

## Exercícios

### Exercício 1 - Service Communication
Implemente comunicação entre dois microservices usando HTTP/REST.

### Exercício 2 - Event-Driven Architecture
Crie um sistema de eventos usando message queues.

### Exercício 3 - Saga Pattern
Implemente uma saga para processamento de pedidos.

## Dicas
- Use circuit breakers para resiliência
- Implemente distributed tracing
- Use API gateways para roteamento
- Implemente service discovery
- Use event sourcing para auditoria
- Separe commands e queries (CQRS)
- Implemente sagas para transações distribuídas
- Monitore e observe seus serviços 