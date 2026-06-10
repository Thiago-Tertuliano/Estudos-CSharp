# Aula 10 - Enterprise Architecture Patterns

## Objetivos da Aula
- Entender padrões arquiteturais empresariais
- Aprender Clean Architecture e DDD
- Compreender Event Sourcing e CQRS
- Praticar implementação de arquiteturas complexas

## Conteúdo Teórico

### Clean Architecture

#### Layers and Dependencies
```csharp
// Domain Layer (Core)
public class User
{
    public int Id { get; private set; }
    public string Name { get; private set; }
    public string Email { get; private set; }
    public bool IsActive { get; private set; }
    
    public User(string name, string email)
    {
        Name = name;
        Email = email;
        IsActive = true;
    }
    
    public void Deactivate()
    {
        IsActive = false;
    }
    
    public void UpdateEmail(string newEmail)
    {
        if (string.IsNullOrEmpty(newEmail))
            throw new ArgumentException("Email cannot be empty");
        
        Email = newEmail;
    }
}

// Application Layer (Use Cases)
public interface IUserRepository
{
    Task<User> GetByIdAsync(int id);
    Task<User> AddAsync(User user);
    Task UpdateAsync(User user);
}

public class CreateUserUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly IUserValidator _validator;
    
    public CreateUserUseCase(IUserRepository userRepository, IUserValidator validator)
    {
        _userRepository = userRepository;
        _validator = validator;
    }
    
    public async Task<User> Execute(CreateUserRequest request)
    {
        var user = new User(request.Name, request.Email);
        
        var validationResult = await _validator.ValidateAsync(user);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);
        
        return await _userRepository.AddAsync(user);
    }
}

// Infrastructure Layer (External Concerns)
public class SqlUserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;
    
    public SqlUserRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<User> GetByIdAsync(int id)
    {
        var userEntity = await _context.Users.FindAsync(id);
        return userEntity?.ToDomain();
    }
    
    public async Task<User> AddAsync(User user)
    {
        var userEntity = user.ToEntity();
        _context.Users.Add(userEntity);
        await _context.SaveChangesAsync();
        return userEntity.ToDomain();
    }
    
    public async Task UpdateAsync(User user)
    {
        var userEntity = user.ToEntity();
        _context.Users.Update(userEntity);
        await _context.SaveChangesAsync();
    }
}

// Presentation Layer (Controllers)
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly CreateUserUseCase _createUserUseCase;
    private readonly GetUserUseCase _getUserUseCase;
    
    public UsersController(CreateUserUseCase createUserUseCase, GetUserUseCase getUserUseCase)
    {
        _createUserUseCase = createUserUseCase;
        _getUserUseCase = getUserUseCase;
    }
    
    [HttpPost]
    public async Task<ActionResult<UserDto>> CreateUser(CreateUserRequest request)
    {
        try
        {
            var user = await _createUserUseCase.Execute(request);
            return Ok(user.ToDto());
        }
        catch (ValidationException ex)
        {
            return BadRequest(ex.Errors);
        }
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> GetUser(int id)
    {
        var user = await _getUserUseCase.Execute(id);
        if (user == null)
            return NotFound();
        
        return Ok(user.ToDto());
    }
}
```

### Domain-Driven Design (DDD)

#### Entities and Value Objects
```csharp
// Entity
public class Order : Entity<int>
{
    public OrderNumber OrderNumber { get; private set; }
    public CustomerId CustomerId { get; private set; }
    public OrderStatus Status { get; private set; }
    public Money TotalAmount { get; private set; }
    private readonly List<OrderItem> _items = new();
    public IReadOnlyList<OrderItem> Items => _items.AsReadOnly();
    
    public Order(OrderNumber orderNumber, CustomerId customerId)
    {
        OrderNumber = orderNumber;
        CustomerId = customerId;
        Status = OrderStatus.Created;
        TotalAmount = Money.Zero;
    }
    
    public void AddItem(ProductId productId, Quantity quantity, Money unitPrice)
    {
        var item = new OrderItem(productId, quantity, unitPrice);
        _items.Add(item);
        RecalculateTotal();
    }
    
    public void Confirm()
    {
        if (Status != OrderStatus.Created)
            throw new InvalidOperationException("Order can only be confirmed when in Created status");
        
        Status = OrderStatus.Confirmed;
    }
    
    public void Cancel()
    {
        if (Status == OrderStatus.Shipped)
            throw new InvalidOperationException("Cannot cancel shipped order");
        
        Status = OrderStatus.Cancelled;
    }
    
    private void RecalculateTotal()
    {
        TotalAmount = _items.Sum(item => item.TotalPrice);
    }
}

// Value Objects
public class OrderNumber : ValueObject<OrderNumber>
{
    public string Value { get; }
    
    public OrderNumber(string value)
    {
        if (string.IsNullOrEmpty(value))
            throw new ArgumentException("Order number cannot be empty");
        
        Value = value;
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
    
    public static OrderNumber Generate()
    {
        return new OrderNumber($"ORD-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid().ToString("N")[..8]}");
    }
}

public class Money : ValueObject<Money>
{
    public decimal Amount { get; }
    public string Currency { get; }
    
    public Money(decimal amount, string currency = "USD")
    {
        Amount = amount;
        Currency = currency;
    }
    
    public static Money Zero => new(0);
    
    public Money Add(Money other)
    {
        if (Currency != other.Currency)
            throw new InvalidOperationException("Cannot add money with different currencies");
        
        return new Money(Amount + other.Amount, Currency);
    }
    
    public Money Multiply(decimal factor)
    {
        return new Money(Amount * factor, Currency);
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Amount;
        yield return Currency;
    }
}
```

#### Aggregates and Repositories
```csharp
// Aggregate Root
public class Customer : AggregateRoot<CustomerId>
{
    public CustomerName Name { get; private set; }
    public Email Email { get; private set; }
    public CustomerStatus Status { get; private set; }
    private readonly List<Order> _orders = new();
    public IReadOnlyList<Order> Orders => _orders.AsReadOnly();
    
    public Customer(CustomerId id, CustomerName name, Email email)
    {
        Id = id;
        Name = name;
        Email = email;
        Status = CustomerStatus.Active;
    }
    
    public Order CreateOrder()
    {
        if (Status != CustomerStatus.Active)
            throw new InvalidOperationException("Inactive customer cannot create orders");
        
        var orderNumber = OrderNumber.Generate();
        var order = new Order(orderNumber, Id);
        _orders.Add(order);
        
        AddDomainEvent(new OrderCreatedEvent(order.Id, Id));
        
        return order;
    }
    
    public void Deactivate()
    {
        Status = CustomerStatus.Inactive;
        AddDomainEvent(new CustomerDeactivatedEvent(Id));
    }
}

// Repository Interface
public interface ICustomerRepository : IRepository<Customer, CustomerId>
{
    Task<Customer> GetByEmailAsync(Email email);
    Task<IEnumerable<Customer>> GetActiveCustomersAsync();
}

// Repository Implementation
public class SqlCustomerRepository : ICustomerRepository
{
    private readonly ApplicationDbContext _context;
    
    public SqlCustomerRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<Customer> GetByIdAsync(CustomerId id)
    {
        var customerEntity = await _context.Customers
            .Include(c => c.Orders)
            .FirstOrDefaultAsync(c => c.Id == id.Value);
        
        return customerEntity?.ToDomain();
    }
    
    public async Task<Customer> GetByEmailAsync(Email email)
    {
        var customerEntity = await _context.Customers
            .Include(c => c.Orders)
            .FirstOrDefaultAsync(c => c.Email == email.Value);
        
        return customerEntity?.ToDomain();
    }
    
    public async Task<IEnumerable<Customer>> GetActiveCustomersAsync()
    {
        var customerEntities = await _context.Customers
            .Include(c => c.Orders)
            .Where(c => c.Status == CustomerStatus.Active)
            .ToListAsync();
        
        return customerEntities.Select(e => e.ToDomain());
    }
    
    public async Task AddAsync(Customer customer)
    {
        var customerEntity = customer.ToEntity();
        _context.Customers.Add(customerEntity);
        await _context.SaveChangesAsync();
    }
    
    public async Task UpdateAsync(Customer customer)
    {
        var customerEntity = customer.ToEntity();
        _context.Customers.Update(customerEntity);
        await _context.SaveChangesAsync();
    }
}
```

### CQRS (Command Query Responsibility Segregation)

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

public class CreateCustomerCommand : ICommand
{
    public string Name { get; set; }
    public string Email { get; set; }
}

public class CreateCustomerCommandHandler : ICommandHandler<CreateCustomerCommand>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IUnitOfWork _unitOfWork;
    
    public CreateCustomerCommandHandler(ICustomerRepository customerRepository, IUnitOfWork unitOfWork)
    {
        _customerRepository = customerRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task HandleAsync(CreateCustomerCommand command)
    {
        var customerId = new CustomerId(Guid.NewGuid());
        var customerName = new CustomerName(command.Name);
        var email = new Email(command.Email);
        
        var customer = new Customer(customerId, customerName, email);
        
        await _customerRepository.AddAsync(customer);
        await _unitOfWork.SaveChangesAsync();
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

public class GetCustomerQuery : IQuery<CustomerDto>
{
    public Guid CustomerId { get; set; }
}

public class GetCustomerQueryHandler : IQueryHandler<GetCustomerQuery, CustomerDto>
{
    private readonly ICustomerReadRepository _customerReadRepository;
    
    public GetCustomerQueryHandler(ICustomerReadRepository customerReadRepository)
    {
        _customerReadRepository = customerReadRepository;
    }
    
    public async Task<CustomerDto> HandleAsync(GetCustomerQuery query)
    {
        var customerId = new CustomerId(query.CustomerId);
        var customer = await _customerReadRepository.GetByIdAsync(customerId);
        
        return customer?.ToDto();
    }
}

// Mediator
public class Mediator : IMediator
{
    private readonly IServiceProvider _serviceProvider;
    
    public Mediator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    public async Task SendAsync<TCommand>(TCommand command) where TCommand : ICommand
    {
        var handler = _serviceProvider.GetService<ICommandHandler<TCommand>>();
        await handler.HandleAsync(command);
    }
    
    public async Task<TResult> SendAsync<TQuery, TResult>(TQuery query) where TQuery : IQuery<TResult>
    {
        var handler = _serviceProvider.GetService<IQueryHandler<TQuery, TResult>>();
        return await handler.HandleAsync(query);
    }
}
```

### Event Sourcing

#### Event Store and Event Handling
```csharp
// Events
public abstract class DomainEvent
{
    public Guid Id { get; set; }
    public DateTime OccurredOn { get; set; }
    public string AggregateType { get; set; }
    public string AggregateId { get; set; }
    public long Version { get; set; }
}

public class CustomerCreatedEvent : DomainEvent
{
    public string Name { get; set; }
    public string Email { get; set; }
}

public class CustomerDeactivatedEvent : DomainEvent
{
    public string Reason { get; set; }
}

// Event Store
public interface IEventStore
{
    Task SaveEventsAsync(string aggregateId, IEnumerable<DomainEvent> events, long expectedVersion);
    Task<IEnumerable<DomainEvent>> GetEventsAsync(string aggregateId);
}

public class EventStore : IEventStore
{
    private readonly ApplicationDbContext _context;
    
    public EventStore(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task SaveEventsAsync(string aggregateId, IEnumerable<DomainEvent> events, long expectedVersion)
    {
        var eventList = events.ToList();
        var version = expectedVersion;
        
        foreach (var @event in eventList)
        {
            version++;
            @event.Version = version;
            @event.AggregateId = aggregateId;
            @event.OccurredOn = DateTime.UtcNow;
            
            var eventEntity = new EventEntity
            {
                Id = @event.Id,
                AggregateId = aggregateId,
                Version = version,
                EventType = @event.GetType().Name,
                EventData = JsonSerializer.Serialize(@event),
                OccurredOn = @event.OccurredOn
            };
            
            _context.Events.Add(eventEntity);
        }
        
        await _context.SaveChangesAsync();
    }
    
    public async Task<IEnumerable<DomainEvent>> GetEventsAsync(string aggregateId)
    {
        var eventEntities = await _context.Events
            .Where(e => e.AggregateId == aggregateId)
            .OrderBy(e => e.Version)
            .ToListAsync();
        
        var events = new List<DomainEvent>();
        
        foreach (var eventEntity in eventEntities)
        {
            var eventType = Type.GetType(eventEntity.EventType);
            var @event = JsonSerializer.Deserialize(eventEntity.EventData, eventType) as DomainEvent;
            events.Add(@event);
        }
        
        return events;
    }
}

// Event Sourced Aggregate
public abstract class EventSourcedAggregate : AggregateRoot
{
    private readonly List<DomainEvent> _uncommittedEvents = new();
    
    protected void Apply(DomainEvent @event)
    {
        When(@event);
        _uncommittedEvents.Add(@event);
    }
    
    protected abstract void When(DomainEvent @event);
    
    public IEnumerable<DomainEvent> GetUncommittedEvents()
    {
        return _uncommittedEvents.AsReadOnly();
    }
    
    public void MarkEventsAsCommitted()
    {
        _uncommittedEvents.Clear();
    }
}

public class EventSourcedCustomer : EventSourcedAggregate
{
    public CustomerId Id { get; private set; }
    public CustomerName Name { get; private set; }
    public Email Email { get; private set; }
    public CustomerStatus Status { get; private set; }
    
    public EventSourcedCustomer(CustomerId id, CustomerName name, Email email)
    {
        Apply(new CustomerCreatedEvent
        {
            Id = Guid.NewGuid(),
            AggregateId = id.Value.ToString(),
            AggregateType = nameof(EventSourcedCustomer),
            Name = name.Value,
            Email = email.Value
        });
    }
    
    public void Deactivate(string reason)
    {
        Apply(new CustomerDeactivatedEvent
        {
            Id = Guid.NewGuid(),
            AggregateId = Id.Value.ToString(),
            AggregateType = nameof(EventSourcedCustomer),
            Reason = reason
        });
    }
    
    protected override void When(DomainEvent @event)
    {
        switch (@event)
        {
            case CustomerCreatedEvent e:
                Id = new CustomerId(Guid.Parse(e.AggregateId));
                Name = new CustomerName(e.Name);
                Email = new Email(e.Email);
                Status = CustomerStatus.Active;
                break;
            case CustomerDeactivatedEvent e:
                Status = CustomerStatus.Inactive;
                break;
        }
    }
}
```

### Hexagonal Architecture (Ports and Adapters)

#### Ports and Adapters Implementation
```csharp
// Domain (Core)
public interface IUserRepository
{
    Task<User> GetByIdAsync(int id);
    Task<User> SaveAsync(User user);
}

public interface IEmailService
{
    Task SendWelcomeEmailAsync(User user);
}

// Application (Use Cases)
public class CreateUserUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly IEmailService _emailService;
    
    public CreateUserUseCase(IUserRepository userRepository, IEmailService emailService)
    {
        _userRepository = userRepository;
        _emailService = emailService;
    }
    
    public async Task<User> Execute(CreateUserRequest request)
    {
        var user = new User(request.Name, request.Email);
        
        var savedUser = await _userRepository.SaveAsync(user);
        await _emailService.SendWelcomeEmailAsync(savedUser);
        
        return savedUser;
    }
}

// Adapters (Infrastructure)
public class SqlUserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;
    
    public SqlUserRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<User> GetByIdAsync(int id)
    {
        var userEntity = await _context.Users.FindAsync(id);
        return userEntity?.ToDomain();
    }
    
    public async Task<User> SaveAsync(User user)
    {
        var userEntity = user.ToEntity();
        _context.Users.Add(userEntity);
        await _context.SaveChangesAsync();
        return userEntity.ToDomain();
    }
}

public class SmtpEmailService : IEmailService
{
    private readonly SmtpClient _smtpClient;
    
    public SmtpEmailService(SmtpClient smtpClient)
    {
        _smtpClient = smtpClient;
    }
    
    public async Task SendWelcomeEmailAsync(User user)
    {
        var message = new MailMessage
        {
            Subject = "Welcome!",
            Body = $"Welcome {user.Name}!",
            To = { user.Email }
        };
        
        await _smtpClient.SendMailAsync(message);
    }
}

// Controllers (Primary Adapters)
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly CreateUserUseCase _createUserUseCase;
    
    public UsersController(CreateUserUseCase createUserUseCase)
    {
        _createUserUseCase = createUserUseCase;
    }
    
    [HttpPost]
    public async Task<ActionResult<UserDto>> CreateUser(CreateUserRequest request)
    {
        var user = await _createUserUseCase.Execute(request);
        return Ok(user.ToDto());
    }
}
```

### Microservices Architecture

#### Service Communication
```csharp
// Service Interface
public interface IUserService
{
    Task<UserDto> GetUserAsync(int id);
    Task<UserDto> CreateUserAsync(CreateUserRequest request);
    Task UpdateUserAsync(int id, UpdateUserRequest request);
    Task DeleteUserAsync(int id);
}

// Service Implementation
public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IEventBus _eventBus;
    
    public UserService(IUserRepository userRepository, IEventBus eventBus)
    {
        _userRepository = userRepository;
        _eventBus = eventBus;
    }
    
    public async Task<UserDto> GetUserAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
            throw new UserNotFoundException(id);
        
        return user.ToDto();
    }
    
    public async Task<UserDto> CreateUserAsync(CreateUserRequest request)
    {
        var user = new User(request.Name, request.Email);
        var savedUser = await _userRepository.AddAsync(user);
        
        await _eventBus.PublishAsync(new UserCreatedEvent
        {
            UserId = savedUser.Id,
            Name = savedUser.Name,
            Email = savedUser.Email
        });
        
        return savedUser.ToDto();
    }
    
    public async Task UpdateUserAsync(int id, UpdateUserRequest request)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
            throw new UserNotFoundException(id);
        
        user.UpdateName(request.Name);
        user.UpdateEmail(request.Email);
        
        await _userRepository.UpdateAsync(user);
        
        await _eventBus.PublishAsync(new UserUpdatedEvent
        {
            UserId = user.Id,
            Name = user.Name,
            Email = user.Email
        });
    }
    
    public async Task DeleteUserAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
            throw new UserNotFoundException(id);
        
        await _userRepository.DeleteAsync(id);
        
        await _eventBus.PublishAsync(new UserDeletedEvent
        {
            UserId = id
        });
    }
}

// Event Bus
public interface IEventBus
{
    Task PublishAsync<T>(T @event) where T : class;
    Task SubscribeAsync<T>(Func<T, Task> handler) where T : class;
}

public class RabbitMqEventBus : IEventBus
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    
    public RabbitMqEventBus(IConnection connection)
    {
        _connection = connection;
        _channel = connection.CreateModel();
    }
    
    public async Task PublishAsync<T>(T @event) where T : class
    {
        var queueName = typeof(T).Name;
        _channel.QueueDeclare(queueName, durable: true, exclusive: false, autoDelete: false);
        
        var json = JsonSerializer.Serialize(@event);
        var body = Encoding.UTF8.GetBytes(json);
        
        _channel.BasicPublish("", queueName, null, body);
        await Task.CompletedTask;
    }
    
    public async Task SubscribeAsync<T>(Func<T, Task> handler) where T : class
    {
        var queueName = typeof(T).Name;
        _channel.QueueDeclare(queueName, durable: true, exclusive: false, autoDelete: false);
        
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var json = Encoding.UTF8.GetString(body);
            var @event = JsonSerializer.Deserialize<T>(json);
            
            await handler(@event);
            _channel.BasicAck(ea.DeliveryTag, false);
        };
        
        _channel.BasicConsume(queueName, false, consumer);
        await Task.CompletedTask;
    }
}
```

## Exercícios

### Exercício 1 - Clean Architecture
Implemente uma aplicação usando Clean Architecture com todas as camadas.

### Exercício 2 - Event Sourcing
Crie um sistema de Event Sourcing para gerenciamento de pedidos.

### Exercício 3 - Microservices
Implemente comunicação entre microservices usando eventos.

## Dicas
- Mantenha o domínio independente de frameworks
- Use value objects para encapsular regras de negócio
- Implemente event sourcing para auditoria completa
- Use CQRS para separar operações de leitura e escrita
- Implemente hexagonal architecture para testabilidade
- Use eventos para comunicação entre serviços
- Mantenha bounded contexts bem definidos
- Documente decisões arquiteturais 