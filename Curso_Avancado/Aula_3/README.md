# Aula 3 - Dependency Injection e IoC Containers

## Objetivos da Aula
- Entender Dependency Injection (DI)
- Aprender Inversion of Control (IoC)
- Compreender IoC Containers
- Praticar configuração de DI em aplicações

## Conteúdo Teórico

### Dependency Injection (DI)

#### Princípios do DI
```csharp
// Ruim - Dependência concreta
public class UserService
{
    private readonly SqlUserRepository _repository;
    
    public UserService()
    {
        _repository = new SqlUserRepository(); // Acoplamento forte
    }
}

// Bom - Injeção de dependência
public class UserService
{
    private readonly IUserRepository _repository;
    
    public UserService(IUserRepository repository)
    {
        _repository = repository; // Desacoplamento
    }
}
```

#### Tipos de Injeção
```csharp
// Constructor Injection (Recomendado)
public class UserService
{
    private readonly IUserRepository _repository;
    private readonly IEmailService _emailService;
    
    public UserService(IUserRepository repository, IEmailService emailService)
    {
        _repository = repository;
        _emailService = emailService;
    }
}

// Property Injection
public class UserService
{
    public IUserRepository Repository { get; set; }
    public IEmailService EmailService { get; set; }
}

// Method Injection
public class UserService
{
    public void ProcessUser(int userId, IUserRepository repository)
    {
        // Usar repository
    }
}
```

### IoC Container - Microsoft.Extensions.DependencyInjection

#### Configuração Básica
```csharp
// Program.cs
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();

// Registrar serviços
services.AddScoped<IUserRepository, SqlUserRepository>();
services.AddScoped<IEmailService, EmailService>();
services.AddScoped<UserService>();

// Construir container
var serviceProvider = services.BuildServiceProvider();

// Resolver serviço
var userService = serviceProvider.GetService<UserService>();
```

#### Lifetimes (Ciclos de Vida)
```csharp
// Transient - Nova instância a cada resolução
services.AddTransient<IUserRepository, SqlUserRepository>();

// Scoped - Uma instância por escopo (request em web apps)
services.AddScoped<IUserRepository, SqlUserRepository>();

// Singleton - Uma instância para toda a aplicação
services.AddSingleton<IUserRepository, SqlUserRepository>();

// Singleton com instância específica
var repository = new SqlUserRepository();
services.AddSingleton<IUserRepository>(repository);
```

### Interfaces e Implementações

#### Definição de Interfaces
```csharp
public interface IUserRepository
{
    Task<User> GetByIdAsync(int id);
    Task<IEnumerable<User>> GetAllAsync();
    Task<User> AddAsync(User user);
    Task UpdateAsync(User user);
    Task DeleteAsync(int id);
}

public interface IEmailService
{
    Task SendEmailAsync(string to, string subject, string body);
    Task SendWelcomeEmailAsync(User user);
}

public interface ILogger
{
    void LogInformation(string message);
    void LogWarning(string message);
    void LogError(string message, Exception exception = null);
}
```

#### Implementações
```csharp
public class SqlUserRepository : IUserRepository
{
    private readonly DbContext _context;
    
    public SqlUserRepository(DbContext context)
    {
        _context = context;
    }
    
    public async Task<User> GetByIdAsync(int id)
    {
        return await _context.Users.FindAsync(id);
    }
    
    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _context.Users.ToListAsync();
    }
    
    public async Task<User> AddAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }
    
    public async Task UpdateAsync(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }
    
    public async Task DeleteAsync(int id)
    {
        var user = await GetByIdAsync(id);
        if (user != null)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
    }
}

public class EmailService : IEmailService
{
    private readonly ILogger _logger;
    
    public EmailService(ILogger logger)
    {
        _logger = logger;
    }
    
    public async Task SendEmailAsync(string to, string subject, string body)
    {
        _logger.LogInformation($"Sending email to {to}: {subject}");
        // Implementação do envio de email
        await Task.Delay(100); // Simulação
    }
    
    public async Task SendWelcomeEmailAsync(User user)
    {
        await SendEmailAsync(user.Email, "Bem-vindo!", $"Olá {user.Name}!");
    }
}
```

### Configuração Avançada

#### Factory Pattern com DI
```csharp
public interface IUserRepositoryFactory
{
    IUserRepository Create(string connectionString);
}

public class UserRepositoryFactory : IUserRepositoryFactory
{
    public IUserRepository Create(string connectionString)
    {
        return new SqlUserRepository(connectionString);
    }
}

// Registro
services.AddSingleton<IUserRepositoryFactory, UserRepositoryFactory>();
```

#### Conditional Registration
```csharp
// Registrar baseado em configuração
var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

if (environment == "Development")
{
    services.AddScoped<IUserRepository, InMemoryUserRepository>();
}
else
{
    services.AddScoped<IUserRepository, SqlUserRepository>();
}
```

#### Named Services
```csharp
public interface IMessageService
{
    void Send(string message);
}

public class EmailMessageService : IMessageService
{
    public void Send(string message) => Console.WriteLine($"Email: {message}");
}

public class SmsMessageService : IMessageService
{
    public void Send(string message) => Console.WriteLine($"SMS: {message}");
}

// Registro com nomes
services.AddScoped<IMessageService, EmailMessageService>("email");
services.AddScoped<IMessageService, SmsMessageService>("sms");

// Resolução por nome
var emailService = serviceProvider.GetService<IMessageService>("email");
var smsService = serviceProvider.GetService<IMessageService>("sms");
```

### Service Locator Pattern

#### Implementação
```csharp
public interface IServiceLocator
{
    T GetService<T>();
    T GetService<T>(string name);
}

public class ServiceLocator : IServiceLocator
{
    private readonly IServiceProvider _serviceProvider;
    
    public ServiceLocator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    public T GetService<T>()
    {
        return _serviceProvider.GetService<T>();
    }
    
    public T GetService<T>(string name)
    {
        // Implementação para serviços nomeados
        return _serviceProvider.GetService<T>();
    }
}

// Registro
services.AddSingleton<IServiceLocator, ServiceLocator>();
```

### Auto-Registration

#### Assembly Scanning
```csharp
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServicesFromAssembly<T>(this IServiceCollection services)
    {
        var assembly = typeof(T).Assembly;
        var types = assembly.GetTypes();
        
        foreach (var type in types)
        {
            var interfaces = type.GetInterfaces();
            
            foreach (var interfaceType in interfaces)
            {
                if (interfaceType.Name.StartsWith("I"))
                {
                    services.AddScoped(interfaceType, type);
                }
            }
        }
        
        return services;
    }
}

// Uso
services.AddServicesFromAssembly<Startup>();
```

### Configuration com Options Pattern

#### Configuration Classes
```csharp
public class EmailSettings
{
    public string SmtpServer { get; set; }
    public int Port { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
}

public class DatabaseSettings
{
    public string ConnectionString { get; set; }
    public int CommandTimeout { get; set; }
}
```

#### Registro com Configuration
```csharp
// appsettings.json
{
  "EmailSettings": {
    "SmtpServer": "smtp.gmail.com",
    "Port": 587,
    "Username": "user@gmail.com",
    "Password": "password"
  },
  "DatabaseSettings": {
    "ConnectionString": "Server=localhost;Database=MyApp;",
    "CommandTimeout": 30
  }
}

// Program.cs
services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
services.Configure<DatabaseSettings>(configuration.GetSection("DatabaseSettings"));

// Uso em serviços
public class EmailService : IEmailService
{
    private readonly EmailSettings _settings;
    
    public EmailService(IOptions<EmailSettings> settings)
    {
        _settings = settings.Value;
    }
}
```

### Validation e Health Checks

#### Service Validation
```csharp
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddValidatedServices(this IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();
        
        foreach (var service in services)
        {
            try
            {
                var instance = serviceProvider.GetService(service.ServiceType);
                if (instance == null)
                {
                    throw new InvalidOperationException($"Service {service.ServiceType.Name} could not be resolved");
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Service validation failed for {service.ServiceType.Name}", ex);
            }
        }
        
        return services;
    }
}
```

#### Health Checks
```csharp
public class DatabaseHealthCheck : IHealthCheck
{
    private readonly IUserRepository _repository;
    
    public DatabaseHealthCheck(IUserRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            await _repository.GetAllAsync();
            return HealthCheckResult.Healthy("Database is accessible");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("Database is not accessible", ex);
        }
    }
}

// Registro
services.AddHealthChecks()
    .AddCheck<DatabaseHealthCheck>("database");
```

### Advanced Scenarios

#### Circular Dependencies
```csharp
// Evitar dependências circulares
public interface IUserService
{
    Task<User> GetUserAsync(int id);
}

public interface IOrderService
{
    Task<Order> GetOrderAsync(int id);
}

public class UserService : IUserService
{
    private readonly IUserRepository _repository;
    private readonly IServiceProvider _serviceProvider; // Lazy resolution
    
    public UserService(IUserRepository repository, IServiceProvider serviceProvider)
    {
        _repository = repository;
        _serviceProvider = serviceProvider;
    }
    
    public async Task<User> GetUserAsync(int id)
    {
        var user = await _repository.GetByIdAsync(id);
        
        // Lazy resolution para evitar circular dependency
        var orderService = _serviceProvider.GetService<IOrderService>();
        // Usar orderService se necessário
        
        return user;
    }
}
```

#### Lazy Loading
```csharp
public class LazyUserService : IUserService
{
    private readonly Lazy<IUserRepository> _repository;
    
    public LazyUserService(Lazy<IUserRepository> repository)
    {
        _repository = repository;
    }
    
    public async Task<User> GetUserAsync(int id)
    {
        return await _repository.Value.GetByIdAsync(id);
    }
}

// Registro
services.AddScoped<IUserRepository, SqlUserRepository>();
services.AddScoped<IUserService, LazyUserService>();
```

## Exercícios

### Exercício 1 - Configuração Básica
Configure um container IoC para uma aplicação de e-commerce.

### Exercício 2 - Factory Pattern
Implemente um sistema de factories com DI para diferentes tipos de repositórios.

### Exercício 3 - Advanced Configuration
Crie um sistema de configuração avançada com validação e health checks.

## Dicas
- Use Constructor Injection sempre que possível
- Evite Service Locator Pattern
- Configure lifetimes adequadamente
- Valide configurações de DI
- Use Options Pattern para configurações
- Implemente health checks para serviços críticos
- Documente dependências complexas
- Teste resolução de serviços 