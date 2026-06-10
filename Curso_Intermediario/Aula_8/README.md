# Aula 8 - Design Patterns Básicos

## Objetivos da Aula
- Entender o conceito de Design Patterns
- Aprender padrões fundamentais
- Compreender quando usar cada padrão
- Praticar implementação de padrões

## Conteúdo Teórico

### O que são Design Patterns?
Design Patterns são soluções reutilizáveis para problemas comuns de design de software. São templates que descrevem como resolver problemas recorrentes.

### Vantagens dos Design Patterns
- **Reutilização**: Soluções testadas e comprovadas
- **Comunicação**: Linguagem comum entre desenvolvedores
- **Manutenibilidade**: Código mais organizado e legível
- **Flexibilidade**: Fácil modificação e extensão

### Padrões Criacionais

#### Singleton
```csharp
public class DatabaseConnection
{
    private static DatabaseConnection _instance;
    private static readonly object _lock = new object();
    
    private DatabaseConnection() { }
    
    public static DatabaseConnection Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new DatabaseConnection();
                    }
                }
            }
            return _instance;
        }
    }
    
    public void Connect()
    {
        Console.WriteLine("Conectando ao banco...");
    }
}

// Uso
var connection1 = DatabaseConnection.Instance;
var connection2 = DatabaseConnection.Instance;
// connection1 e connection2 são a mesma instância
```

#### Factory Method
```csharp
public abstract class Animal
{
    public abstract string FazerSom();
}

public class Cachorro : Animal
{
    public override string FazerSom() => "Au au!";
}

public class Gato : Animal
{
    public override string FazerSom() => "Miau!";
}

public abstract class AnimalFactory
{
    public abstract Animal CriarAnimal();
}

public class CachorroFactory : AnimalFactory
{
    public override Animal CriarAnimal() => new Cachorro();
}

public class GatoFactory : AnimalFactory
{
    public override Animal CriarAnimal() => new Gato();
}

// Uso
AnimalFactory factory = new CachorroFactory();
Animal animal = factory.CriarAnimal();
Console.WriteLine(animal.FazerSom()); // "Au au!"
```

### Padrões Estruturais

#### Adapter
```csharp
// Interface antiga
public interface ILegacyPayment
{
    void ProcessPayment(double amount);
}

// Nova interface
public interface INewPayment
{
    void Pay(decimal amount);
}

// Adapter
public class PaymentAdapter : ILegacyPayment
{
    private readonly INewPayment _newPayment;
    
    public PaymentAdapter(INewPayment newPayment)
    {
        _newPayment = newPayment;
    }
    
    public void ProcessPayment(double amount)
    {
        _newPayment.Pay((decimal)amount);
    }
}

// Implementação da nova interface
public class NewPaymentSystem : INewPayment
{
    public void Pay(decimal amount)
    {
        Console.WriteLine($"Pagamento processado: {amount:C}");
    }
}

// Uso
INewPayment newPayment = new NewPaymentSystem();
ILegacyPayment legacyPayment = new PaymentAdapter(newPayment);
legacyPayment.ProcessPayment(100.50);
```

#### Decorator
```csharp
public abstract class Coffee
{
    public abstract string GetDescription();
    public abstract decimal GetCost();
}

public class SimpleCoffee : Coffee
{
    public override string GetDescription() => "Café simples";
    public override decimal GetCost() => 5.00m;
}

public abstract class CoffeeDecorator : Coffee
{
    protected Coffee _coffee;
    
    public CoffeeDecorator(Coffee coffee)
    {
        _coffee = coffee;
    }
    
    public override string GetDescription() => _coffee.GetDescription();
    public override decimal GetCost() => _coffee.GetCost();
}

public class MilkDecorator : CoffeeDecorator
{
    public MilkDecorator(Coffee coffee) : base(coffee) { }
    
    public override string GetDescription() => _coffee.GetDescription() + ", Leite";
    public override decimal GetCost() => _coffee.GetCost() + 1.00m;
}

public class SugarDecorator : CoffeeDecorator
{
    public SugarDecorator(Coffee coffee) : base(coffee) { }
    
    public override string GetDescription() => _coffee.GetDescription() + ", Açúcar";
    public override decimal GetCost() => _coffee.GetCost() + 0.50m;
}

// Uso
Coffee coffee = new SimpleCoffee();
coffee = new MilkDecorator(coffee);
coffee = new SugarDecorator(coffee);

Console.WriteLine($"{coffee.GetDescription()}: {coffee.GetCost():C}");
// "Café simples, Leite, Açúcar: R$ 6,50"
```

### Padrões Comportamentais

#### Observer
```csharp
public interface IObserver
{
    void Update(string message);
}

public interface ISubject
{
    void Attach(IObserver observer);
    void Detach(IObserver observer);
    void Notify(string message);
}

public class NewsAgency : ISubject
{
    private List<IObserver> _observers = new List<IObserver>();
    
    public void Attach(IObserver observer)
    {
        _observers.Add(observer);
    }
    
    public void Detach(IObserver observer)
    {
        _observers.Remove(observer);
    }
    
    public void Notify(string message)
    {
        foreach (var observer in _observers)
        {
            observer.Update(message);
        }
    }
    
    public void PublishNews(string news)
    {
        Console.WriteLine($"Agência publica: {news}");
        Notify(news);
    }
}

public class NewsChannel : IObserver
{
    public string Name { get; set; }
    
    public NewsChannel(string name)
    {
        Name = name;
    }
    
    public void Update(string message)
    {
        Console.WriteLine($"{Name} recebeu: {message}");
    }
}

// Uso
var agency = new NewsAgency();
var channel1 = new NewsChannel("CNN");
var channel2 = new NewsChannel("BBC");

agency.Attach(channel1);
agency.Attach(channel2);

agency.PublishNews("Notícia importante!");
```

#### Strategy
```csharp
public interface IPaymentStrategy
{
    void Pay(decimal amount);
}

public class CreditCardPayment : IPaymentStrategy
{
    public void Pay(decimal amount)
    {
        Console.WriteLine($"Pagamento com cartão de crédito: {amount:C}");
    }
}

public class PayPalPayment : IPaymentStrategy
{
    public void Pay(decimal amount)
    {
        Console.WriteLine($"Pagamento com PayPal: {amount:C}");
    }
}

public class BankTransferPayment : IPaymentStrategy
{
    public void Pay(decimal amount)
    {
        Console.WriteLine($"Pagamento com transferência bancária: {amount:C}");
    }
}

public class ShoppingCart
{
    private IPaymentStrategy _paymentStrategy;
    
    public void SetPaymentStrategy(IPaymentStrategy strategy)
    {
        _paymentStrategy = strategy;
    }
    
    public void Checkout(decimal amount)
    {
        _paymentStrategy.Pay(amount);
    }
}

// Uso
var cart = new ShoppingCart();
cart.SetPaymentStrategy(new CreditCardPayment());
cart.Checkout(100.00m);

cart.SetPaymentStrategy(new PayPalPayment());
cart.Checkout(50.00m);
```

### Padrões de Arquitetura

#### Repository Pattern
```csharp
public interface IRepository<T>
{
    T GetById(int id);
    IEnumerable<T> GetAll();
    void Add(T entity);
    void Update(T entity);
    void Delete(int id);
}

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
}

public class UserRepository : IRepository<User>
{
    private List<User> _users = new List<User>();
    private int _nextId = 1;
    
    public User GetById(int id)
    {
        return _users.FirstOrDefault(u => u.Id == id);
    }
    
    public IEnumerable<User> GetAll()
    {
        return _users;
    }
    
    public void Add(User entity)
    {
        entity.Id = _nextId++;
        _users.Add(entity);
    }
    
    public void Update(User entity)
    {
        var existingUser = GetById(entity.Id);
        if (existingUser != null)
        {
            existingUser.Name = entity.Name;
            existingUser.Email = entity.Email;
        }
    }
    
    public void Delete(int id)
    {
        var user = GetById(id);
        if (user != null)
        {
            _users.Remove(user);
        }
    }
}
```

#### Unit of Work Pattern
```csharp
public interface IUnitOfWork : IDisposable
{
    IRepository<User> Users { get; }
    IRepository<Product> Products { get; }
    void SaveChanges();
}

public class UnitOfWork : IUnitOfWork
{
    private readonly IRepository<User> _users;
    private readonly IRepository<Product> _products;
    private bool _disposed = false;
    
    public UnitOfWork()
    {
        _users = new UserRepository();
        _products = new ProductRepository();
    }
    
    public IRepository<User> Users => _users;
    public IRepository<Product> Products => _products;
    
    public void SaveChanges()
    {
        // Implementar lógica de persistência
        Console.WriteLine("Mudanças salvas!");
    }
    
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed && disposing)
        {
            // Limpar recursos
            _disposed = true;
        }
    }
}
```

### Padrões de Injeção de Dependência

#### Constructor Injection
```csharp
public interface IEmailService
{
    void SendEmail(string to, string subject, string body);
}

public class EmailService : IEmailService
{
    public void SendEmail(string to, string subject, string body)
    {
        Console.WriteLine($"Email enviado para {to}: {subject}");
    }
}

public class UserService
{
    private readonly IEmailService _emailService;
    
    public UserService(IEmailService emailService)
    {
        _emailService = emailService;
    }
    
    public void RegisterUser(string email, string name)
    {
        // Lógica de registro
        _emailService.SendEmail(email, "Bem-vindo!", $"Olá {name}!");
    }
}

// Uso
IEmailService emailService = new EmailService();
var userService = new UserService(emailService);
userService.RegisterUser("user@example.com", "João");
```

### Boas Práticas

#### Escolhendo o Padrão Correto
```csharp
// Use Singleton para recursos únicos
// Use Factory para criação complexa de objetos
// Use Adapter para compatibilidade
// Use Decorator para funcionalidade adicional
// Use Observer para notificações
// Use Strategy para algoritmos intercambiáveis
```

#### Implementação Limpa
```csharp
// Mantenha as implementações simples
// Use interfaces para desacoplamento
// Documente o propósito do padrão
// Teste as implementações
```

## Exercícios

### Exercício 1 - Singleton e Factory
Implemente um sistema de logging usando Singleton e Factory.

### Exercício 2 - Observer e Strategy
Crie um sistema de notificações com diferentes estratégias.

### Exercício 3 - Repository Pattern
Implemente um CRUD completo usando Repository Pattern.

## Dicas
- Use padrões para resolver problemas específicos
- Não force padrões onde não são necessários
- Mantenha implementações simples e legíveis
- Documente o uso de padrões no código
- Teste as implementações de padrões
- Considere a manutenibilidade ao escolher padrões
- Use interfaces para desacoplamento 