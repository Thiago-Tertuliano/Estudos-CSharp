# Aula 9 - SOLID Principles

## Objetivos da Aula
- Entender os princípios SOLID
- Aprender a aplicar cada princípio
- Compreender benefícios dos princípios
- Praticar refatoração seguindo SOLID

## Conteúdo Teórico

### O que são os Princípios SOLID?
SOLID é um acrônimo para cinco princípios de design de software que visam criar código mais limpo, manutenível e extensível.

### Vantagens dos Princípios SOLID
- **Manutenibilidade**: Código mais fácil de manter
- **Extensibilidade**: Fácil adição de novas funcionalidades
- **Testabilidade**: Código mais fácil de testar
- **Reutilização**: Componentes reutilizáveis
- **Flexibilidade**: Mudanças sem quebrar código existente

### S - Single Responsibility Principle (SRP)

#### Princípio
Uma classe deve ter apenas uma razão para mudar, ou seja, uma única responsabilidade.

#### Exemplo Ruim
```csharp
public class Usuario
{
    public string Nome { get; set; }
    public string Email { get; set; }
    
    public void Salvar()
    {
        // Lógica de persistência
        Console.WriteLine("Salvando usuário...");
    }
    
    public void EnviarEmail()
    {
        // Lógica de envio de email
        Console.WriteLine("Enviando email...");
    }
    
    public void Validar()
    {
        // Lógica de validação
        Console.WriteLine("Validando usuário...");
    }
}
```

#### Exemplo Bom
```csharp
public class Usuario
{
    public string Nome { get; set; }
    public string Email { get; set; }
}

public class UsuarioRepository
{
    public void Salvar(Usuario usuario)
    {
        Console.WriteLine("Salvando usuário...");
    }
}

public class EmailService
{
    public void EnviarEmail(string email, string mensagem)
    {
        Console.WriteLine("Enviando email...");
    }
}

public class UsuarioValidator
{
    public bool Validar(Usuario usuario)
    {
        Console.WriteLine("Validando usuário...");
        return !string.IsNullOrEmpty(usuario.Nome) && 
               !string.IsNullOrEmpty(usuario.Email);
    }
}
```

### O - Open/Closed Principle (OCP)

#### Princípio
Entidades de software devem estar abertas para extensão, mas fechadas para modificação.

#### Exemplo Ruim
```csharp
public class CalculadoraDesconto
{
    public decimal CalcularDesconto(string tipoCliente, decimal valor)
    {
        if (tipoCliente == "VIP")
        {
            return valor * 0.1m;
        }
        else if (tipoCliente == "Premium")
        {
            return valor * 0.15m;
        }
        else
        {
            return valor * 0.05m;
        }
    }
}
```

#### Exemplo Bom
```csharp
public abstract class DescontoStrategy
{
    public abstract decimal CalcularDesconto(decimal valor);
}

public class VipDesconto : DescontoStrategy
{
    public override decimal CalcularDesconto(decimal valor)
    {
        return valor * 0.1m;
    }
}

public class PremiumDesconto : DescontoStrategy
{
    public override decimal CalcularDesconto(decimal valor)
    {
        return valor * 0.15m;
    }
}

public class PadraoDesconto : DescontoStrategy
{
    public override decimal CalcularDesconto(decimal valor)
    {
        return valor * 0.05m;
    }
}

public class CalculadoraDesconto
{
    private readonly DescontoStrategy _strategy;
    
    public CalculadoraDesconto(DescontoStrategy strategy)
    {
        _strategy = strategy;
    }
    
    public decimal CalcularDesconto(decimal valor)
    {
        return _strategy.CalcularDesconto(valor);
    }
}
```

### L - Liskov Substitution Principle (LSP)

#### Princípio
Objetos de uma superclasse devem poder ser substituídos por objetos de uma subclasse sem quebrar a funcionalidade.

#### Exemplo Ruim
```csharp
public class Retangulo
{
    public virtual int Largura { get; set; }
    public virtual int Altura { get; set; }
    
    public int CalcularArea()
    {
        return Largura * Altura;
    }
}

public class Quadrado : Retangulo
{
    public override int Largura
    {
        get => base.Largura;
        set
        {
            base.Largura = value;
            base.Altura = value; // Quebra o comportamento esperado
        }
    }
    
    public override int Altura
    {
        get => base.Altura;
        set
        {
            base.Altura = value;
            base.Largura = value; // Quebra o comportamento esperado
        }
    }
}
```

#### Exemplo Bom
```csharp
public abstract class Forma
{
    public abstract int CalcularArea();
}

public class Retangulo : Forma
{
    public int Largura { get; set; }
    public int Altura { get; set; }
    
    public override int CalcularArea()
    {
        return Largura * Altura;
    }
}

public class Quadrado : Forma
{
    public int Lado { get; set; }
    
    public override int CalcularArea()
    {
        return Lado * Lado;
    }
}
```

### I - Interface Segregation Principle (ISP)

#### Princípio
É melhor ter várias interfaces específicas do que uma interface geral.

#### Exemplo Ruim
```csharp
public interface IWorker
{
    void Work();
    void Eat();
    void Sleep();
}

public class Human : IWorker
{
    public void Work() => Console.WriteLine("Human working");
    public void Eat() => Console.WriteLine("Human eating");
    public void Sleep() => Console.WriteLine("Human sleeping");
}

public class Robot : IWorker
{
    public void Work() => Console.WriteLine("Robot working");
    public void Eat() => throw new NotImplementedException(); // Robot não come
    public void Sleep() => throw new NotImplementedException(); // Robot não dorme
}
```

#### Exemplo Bom
```csharp
public interface IWorkable
{
    void Work();
}

public interface IEatable
{
    void Eat();
}

public interface ISleepable
{
    void Sleep();
}

public class Human : IWorkable, IEatable, ISleepable
{
    public void Work() => Console.WriteLine("Human working");
    public void Eat() => Console.WriteLine("Human eating");
    public void Sleep() => Console.WriteLine("Human sleeping");
}

public class Robot : IWorkable
{
    public void Work() => Console.WriteLine("Robot working");
}
```

### D - Dependency Inversion Principle (DIP)

#### Princípio
Módulos de alto nível não devem depender de módulos de baixo nível. Ambos devem depender de abstrações.

#### Exemplo Ruim
```csharp
public class EmailService
{
    public void EnviarEmail(string to, string message)
    {
        Console.WriteLine($"Email enviado para {to}: {message}");
    }
}

public class UsuarioService
{
    private readonly EmailService _emailService;
    
    public UsuarioService()
    {
        _emailService = new EmailService(); // Dependência concreta
    }
    
    public void RegistrarUsuario(string email, string nome)
    {
        // Lógica de registro
        _emailService.EnviarEmail(email, $"Bem-vindo {nome}!");
    }
}
```

#### Exemplo Bom
```csharp
public interface INotificationService
{
    void EnviarNotificacao(string destinatario, string mensagem);
}

public class EmailService : INotificationService
{
    public void EnviarNotificacao(string destinatario, string mensagem)
    {
        Console.WriteLine($"Email enviado para {destinatario}: {mensagem}");
    }
}

public class SMSService : INotificationService
{
    public void EnviarNotificacao(string destinatario, string mensagem)
    {
        Console.WriteLine($"SMS enviado para {destinatario}: {mensagem}");
    }
}

public class UsuarioService
{
    private readonly INotificationService _notificationService;
    
    public UsuarioService(INotificationService notificationService)
    {
        _notificationService = notificationService; // Dependência de abstração
    }
    
    public void RegistrarUsuario(string email, string nome)
    {
        // Lógica de registro
        _notificationService.EnviarNotificacao(email, $"Bem-vindo {nome}!");
    }
}
```

### Aplicando SOLID em Projetos Reais

#### Exemplo de Sistema de Pagamento
```csharp
// Interfaces (DIP)
public interface IPaymentProcessor
{
    bool ProcessPayment(decimal amount);
}

public interface IPaymentValidator
{
    bool ValidatePayment(decimal amount);
}

public interface IPaymentLogger
{
    void LogPayment(decimal amount, bool success);
}

// Implementações
public class CreditCardProcessor : IPaymentProcessor
{
    public bool ProcessPayment(decimal amount)
    {
        Console.WriteLine($"Processando pagamento com cartão: {amount:C}");
        return true;
    }
}

public class PayPalProcessor : IPaymentProcessor
{
    public bool ProcessPayment(decimal amount)
    {
        Console.WriteLine($"Processando pagamento com PayPal: {amount:C}");
        return true;
    }
}

public class PaymentValidator : IPaymentValidator
{
    public bool ValidatePayment(decimal amount)
    {
        return amount > 0 && amount <= 10000;
    }
}

public class PaymentLogger : IPaymentLogger
{
    public void LogPayment(decimal amount, bool success)
    {
        Console.WriteLine($"Log: Pagamento de {amount:C} - {(success ? "Sucesso" : "Falha")}");
    }
}

// Serviço principal (SRP)
public class PaymentService
{
    private readonly IPaymentProcessor _processor;
    private readonly IPaymentValidator _validator;
    private readonly IPaymentLogger _logger;
    
    public PaymentService(IPaymentProcessor processor, IPaymentValidator validator, IPaymentLogger logger)
    {
        _processor = processor;
        _validator = validator;
        _logger = logger;
    }
    
    public bool ProcessPayment(decimal amount)
    {
        if (!_validator.ValidatePayment(amount))
        {
            _logger.LogPayment(amount, false);
            return false;
        }
        
        var success = _processor.ProcessPayment(amount);
        _logger.LogPayment(amount, success);
        return success;
    }
}
```

### Refatoração Seguindo SOLID

#### Antes da Refatoração
```csharp
public class OrderProcessor
{
    public void ProcessOrder(Order order)
    {
        // Validação
        if (order.Total <= 0)
            throw new ArgumentException("Total inválido");
        
        // Persistência
        SaveOrder(order);
        
        // Notificação
        SendEmail(order.CustomerEmail, "Pedido processado");
        
        // Log
        LogOrder(order);
    }
    
    private void SaveOrder(Order order) { }
    private void SendEmail(string email, string message) { }
    private void LogOrder(Order order) { }
}
```

#### Após a Refatoração
```csharp
public interface IOrderValidator
{
    bool Validate(Order order);
}

public interface IOrderRepository
{
    void Save(Order order);
}

public interface INotificationService
{
    void SendNotification(string recipient, string message);
}

public interface ILogger
{
    void Log(string message);
}

public class OrderProcessor
{
    private readonly IOrderValidator _validator;
    private readonly IOrderRepository _repository;
    private readonly INotificationService _notification;
    private readonly ILogger _logger;
    
    public OrderProcessor(
        IOrderValidator validator,
        IOrderRepository repository,
        INotificationService notification,
        ILogger logger)
    {
        _validator = validator;
        _repository = repository;
        _notification = notification;
        _logger = logger;
    }
    
    public void ProcessOrder(Order order)
    {
        if (!_validator.Validate(order))
            throw new ArgumentException("Pedido inválido");
        
        _repository.Save(order);
        _notification.SendNotification(order.CustomerEmail, "Pedido processado");
        _logger.Log($"Pedido {order.Id} processado");
    }
}
```

### Boas Práticas

#### Identificando Violações
```csharp
// Violação do SRP
public class UserManager
{
    public void CreateUser() { } // Responsabilidade 1
    public void SendEmail() { }  // Responsabilidade 2
    public void ValidateData() { } // Responsabilidade 3
}

// Violação do OCP
public class DiscountCalculator
{
    public decimal CalculateDiscount(string customerType)
    {
        if (customerType == "VIP") return 0.1m;
        if (customerType == "Premium") return 0.15m;
        // Precisa modificar para adicionar novos tipos
        return 0.05m;
    }
}

// Violação do LSP
public class Bird
{
    public virtual void Fly() { }
}

public class Penguin : Bird // Penguin não voa!
{
    public override void Fly() => throw new NotImplementedException();
}
```

#### Aplicando SOLID
```csharp
// SRP - Cada classe tem uma responsabilidade
public class UserValidator { }
public class UserRepository { }
public class EmailService { }

// OCP - Aberto para extensão
public interface IDiscountStrategy
{
    decimal CalculateDiscount();
}

// LSP - Substituição válida
public interface IFlyable
{
    void Fly();
}

public class Sparrow : IFlyable
{
    public void Fly() { }
}

// ISP - Interfaces específicas
public interface IReadable { }
public interface IWritable { }
public interface IDeletable { }

// DIP - Dependência de abstrações
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
```

## Exercícios

### Exercício 1 - Aplicando SRP
Refatore uma classe com múltiplas responsabilidades.

### Exercício 2 - Implementando OCP
Crie um sistema extensível usando abstrações.

### Exercício 3 - Refatoração SOLID
Refatore um sistema completo seguindo todos os princípios SOLID.

## Dicas
- Identifique responsabilidades únicas
- Use abstrações para extensibilidade
- Garanta substituição válida
- Crie interfaces específicas
- Dependa de abstrações, não implementações
- Teste suas refatorações
- Documente as decisões de design 