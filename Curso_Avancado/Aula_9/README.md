# Aula 9 - Advanced Testing Strategies

## Objetivos da Aula
- Entender estratégias avançadas de teste
- Aprender testes de performance e integração
- Compreender testes de mutação e propriedade
- Praticar implementação de testes complexos

## Conteúdo Teórico

### Advanced Unit Testing

#### Test Data Builders
```csharp
public class UserBuilder
{
    private string _name = "John Doe";
    private string _email = "john@example.com";
    private int _age = 30;
    private bool _isActive = true;
    
    public UserBuilder WithName(string name)
    {
        _name = name;
        return this;
    }
    
    public UserBuilder WithEmail(string email)
    {
        _email = email;
        return this;
    }
    
    public UserBuilder WithAge(int age)
    {
        _age = age;
        return this;
    }
    
    public UserBuilder IsActive(bool isActive)
    {
        _isActive = isActive;
        return this;
    }
    
    public User Build()
    {
        return new User
        {
            Name = _name,
            Email = _email,
            Age = _age,
            IsActive = _isActive
        };
    }
}

// Uso em testes
[Test]
public void ShouldValidateUser_WhenValidData()
{
    var user = new UserBuilder()
        .WithName("Jane Doe")
        .WithEmail("jane@example.com")
        .WithAge(25)
        .Build();
    
    var validator = new UserValidator();
    var result = validator.Validate(user);
    
    Assert.That(result.IsValid, Is.True);
}
```

#### Test Doubles (Mocks, Stubs, Fakes)
```csharp
public interface IUserRepository
{
    Task<User> GetByIdAsync(int id);
    Task<IEnumerable<User>> GetAllAsync();
    Task<User> AddAsync(User user);
}

// Stub - implementação simples
public class StubUserRepository : IUserRepository
{
    private readonly List<User> _users = new();
    
    public Task<User> GetByIdAsync(int id)
    {
        var user = _users.FirstOrDefault(u => u.Id == id);
        return Task.FromResult(user);
    }
    
    public Task<IEnumerable<User>> GetAllAsync()
    {
        return Task.FromResult(_users.AsEnumerable());
    }
    
    public Task<User> AddAsync(User user)
    {
        user.Id = _users.Count + 1;
        _users.Add(user);
        return Task.FromResult(user);
    }
}

// Mock com Moq
[Test]
public void ShouldCallRepository_WhenAddingUser()
{
    var mockRepository = new Mock<IUserRepository>();
    var userService = new UserService(mockRepository.Object);
    var user = new UserBuilder().Build();
    
    userService.AddUser(user);
    
    mockRepository.Verify(r => r.AddAsync(user), Times.Once);
}
```

### Property-Based Testing

#### FsCheck Implementation
```csharp
using FsCheck;
using FsCheck.Xunit;

public class UserProperties
{
    [Property]
    public Property UserValidation_ValidUser_ShouldPass()
    {
        return Prop.ForAll<User>(user =>
        {
            var validator = new UserValidator();
            var result = validator.Validate(user);
            
            return result.IsValid == IsValidUser(user);
        });
    }
    
    [Property]
    public Property UserAge_ShouldBePositive()
    {
        return Prop.ForAll<int>(age =>
        {
            var user = new UserBuilder().WithAge(age).Build();
            return user.Age >= 0;
        });
    }
    
    [Property]
    public Property UserEmail_ShouldContainAtSymbol()
    {
        return Prop.ForAll<string>(email =>
        {
            if (string.IsNullOrEmpty(email)) return true;
            
            var user = new UserBuilder().WithEmail(email).Build();
            return user.Email.Contains("@");
        });
    }
}

// Custom generators
public static class UserGenerators
{
    public static Arbitrary<User> UserGenerator()
    {
        return Gen.Choose(1, 100)
            .Zip(Gen.Choose(18, 80))
            .Select(tuple => new User
            {
                Id = tuple.Item1,
                Name = $"User{tuple.Item1}",
                Age = tuple.Item2,
                Email = $"user{tuple.Item1}@example.com"
            })
            .ToArbitrary();
    }
}
```

### Performance Testing

#### BenchmarkDotNet
```csharp
[MemoryDiagnoser]
public class UserServiceBenchmark
{
    private UserService _userService;
    private List<User> _users;
    
    [GlobalSetup]
    public void Setup()
    {
        _userService = new UserService(new StubUserRepository());
        _users = Enumerable.Range(1, 1000)
            .Select(i => new UserBuilder().WithName($"User{i}").Build())
            .ToList();
    }
    
    [Benchmark]
    public void AddUsers()
    {
        foreach (var user in _users)
        {
            _userService.AddUser(user);
        }
    }
    
    [Benchmark]
    public void ValidateUsers()
    {
        var validator = new UserValidator();
        foreach (var user in _users)
        {
            validator.Validate(user);
        }
    }
    
    [Benchmark]
    public void SearchUsers()
    {
        _userService.SearchUsers("User");
    }
}
```

#### Custom Performance Tests
```csharp
public class PerformanceTestBase
{
    protected void MeasurePerformance(Action action, string testName, int iterations = 1000)
    {
        var stopwatch = Stopwatch.StartNew();
        
        for (int i = 0; i < iterations; i++)
        {
            action();
        }
        
        stopwatch.Stop();
        var averageTime = stopwatch.ElapsedMilliseconds / (double)iterations;
        
        Console.WriteLine($"{testName}: {averageTime:F2}ms average");
    }
}

[TestFixture]
public class UserServicePerformanceTests : PerformanceTestBase
{
    [Test]
    public void AddUser_ShouldCompleteWithinTimeLimit()
    {
        var userService = new UserService(new StubUserRepository());
        var user = new UserBuilder().Build();
        
        MeasurePerformance(
            () => userService.AddUser(user),
            "AddUser",
            10000
        );
    }
}
```

### Integration Testing

#### Test Containers
```csharp
public class DatabaseIntegrationTests : IAsyncDisposable
{
    private TestcontainersContainer _container;
    private string _connectionString;
    
    [OneTimeSetUp]
    public async Task Setup()
    {
        _container = new TestcontainersBuilder<TestcontainersContainer>()
            .WithImage("mcr.microsoft.com/mssql/server:2019-latest")
            .WithEnvironment("ACCEPT_EULA", "Y")
            .WithEnvironment("SA_PASSWORD", "YourStrong@Passw0rd")
            .WithPortBinding(1433, 1433)
            .Build();
        
        await _container.StartAsync();
        
        var host = _container.Hostname;
        var port = _container.GetMappedPublicPort(1433);
        _connectionString = $"Server={host},{port};Database=TestDb;User Id=sa;Password=YourStrong@Passw0rd;TrustServerCertificate=true";
    }
    
    [Test]
    public async Task ShouldSaveUser_WhenValidUser()
    {
        var context = new ApplicationDbContext(_connectionString);
        var repository = new UserRepository(context);
        var user = new UserBuilder().Build();
        
        var savedUser = await repository.AddAsync(user);
        
        Assert.That(savedUser.Id, Is.GreaterThan(0));
        Assert.That(savedUser.Name, Is.EqualTo(user.Name));
    }
    
    public async ValueTask DisposeAsync()
    {
        await _container?.DisposeAsync();
    }
}
```

#### API Integration Tests
```csharp
[TestFixture]
public class UserApiIntegrationTests
{
    private WebApplicationFactory<Program> _factory;
    private HttpClient _client;
    
    [OneTimeSetUp]
    public void Setup()
    {
        _factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    // Substituir serviços por mocks se necessário
                    services.AddScoped<IUserRepository, StubUserRepository>();
                });
            });
        
        _client = _factory.CreateClient();
    }
    
    [Test]
    public async Task GetUsers_ShouldReturnUsers()
    {
        var response = await _client.GetAsync("/api/users");
        
        Assert.That(response.IsSuccessStatusCode, Is.True);
        
        var content = await response.Content.ReadAsStringAsync();
        var users = JsonSerializer.Deserialize<List<User>>(content);
        
        Assert.That(users, Is.Not.Null);
    }
    
    [Test]
    public async Task CreateUser_ShouldReturnCreatedUser()
    {
        var user = new UserBuilder().Build();
        var json = JsonSerializer.Serialize(user);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        
        var response = await _client.PostAsync("/api/users", content);
        
        Assert.That(response.IsSuccessStatusCode, Is.True);
        
        var responseContent = await response.Content.ReadAsStringAsync();
        var createdUser = JsonSerializer.Deserialize<User>(responseContent);
        
        Assert.That(createdUser.Id, Is.GreaterThan(0));
    }
}
```

### Mutation Testing

#### Stryker.NET
```csharp
// Código original
public class Calculator
{
    public int Add(int a, int b)
    {
        return a + b;
    }
    
    public int Subtract(int a, int b)
    {
        return a - b;
    }
}

// Testes
[TestFixture]
public class CalculatorTests
{
    [Test]
    public void Add_ShouldReturnSum()
    {
        var calculator = new Calculator();
        var result = calculator.Add(2, 3);
        Assert.That(result, Is.EqualTo(5));
    }
    
    [Test]
    public void Subtract_ShouldReturnDifference()
    {
        var calculator = new Calculator();
        var result = calculator.Subtract(5, 3);
        Assert.That(result, Is.EqualTo(2));
    }
}

// Mutantes que Stryker pode gerar:
// return a + b; -> return a - b;
// return a - b; -> return a + b;
// return a + b; -> return a;
// return a - b; -> return a;
```

### Contract Testing

#### Pact.NET
```csharp
public class UserServicePactTests
{
    private IPactBuilderV3 _pactBuilder;
    
    [SetUp]
    public void Setup()
    {
        _pactBuilder = Pact.V3("UserService", "UserClient", new PactConfig
        {
            PactDir = @"..\..\..\pacts",
            DefaultJsonSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            }
        });
    }
    
    [Test]
    public async Task GetUser_ShouldReturnUser()
    {
        _pactBuilder
            .UponReceiving("A request for a user")
            .Given("A user exists")
            .WithRequest(HttpMethod.Get, "/api/users/1")
            .WillRespond()
            .WithStatus(200)
            .WithHeader("Content-Type", "application/json; charset=utf-8")
            .WithJsonBody(new
            {
                id = 1,
                name = "John Doe",
                email = "john@example.com"
            });
        
        await _pactBuilder.VerifyAsync(async ctx =>
        {
            var client = new HttpClient { BaseAddress = new Uri(ctx.MockServerUri) };
            var response = await client.GetAsync("/api/users/1");
            
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        });
    }
}
```

### Test Data Management

#### Test Data Factories
```csharp
public static class TestDataFactory
{
    public static User CreateValidUser()
    {
        return new UserBuilder()
            .WithName("Test User")
            .WithEmail("test@example.com")
            .WithAge(25)
            .Build();
    }
    
    public static User CreateInvalidUser()
    {
        return new UserBuilder()
            .WithName("")
            .WithEmail("invalid-email")
            .WithAge(-1)
            .Build();
    }
    
    public static List<User> CreateUserList(int count = 10)
    {
        return Enumerable.Range(1, count)
            .Select(i => new UserBuilder()
                .WithName($"User {i}")
                .WithEmail($"user{i}@example.com")
                .WithAge(20 + i)
                .Build())
            .ToList();
    }
    
    public static User CreateUserWithSpecificAge(int age)
    {
        return new UserBuilder()
            .WithAge(age)
            .Build();
    }
}
```

#### Test Data Cleanup
```csharp
[TestFixture]
public class DatabaseTests
{
    private ApplicationDbContext _context;
    
    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        
        _context = new ApplicationDbContext(options);
    }
    
    [TearDown]
    public void Cleanup()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
    
    [Test]
    public async Task ShouldSaveAndRetrieveUser()
    {
        var user = TestDataFactory.CreateValidUser();
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        
        var retrievedUser = await _context.Users.FirstAsync(u => u.Id == user.Id);
        
        Assert.That(retrievedUser.Name, Is.EqualTo(user.Name));
    }
}
```

### Advanced Test Patterns

#### Test Categories
```csharp
[TestFixture]
public class UserServiceTests
{
    [Test]
    [Category("Unit")]
    public void ValidateUser_ValidUser_ShouldReturnTrue()
    {
        // Teste unitário
    }
    
    [Test]
    [Category("Integration")]
    public async Task SaveUser_ShouldPersistToDatabase()
    {
        // Teste de integração
    }
    
    [Test]
    [Category("Performance")]
    public void ProcessUsers_ShouldCompleteWithinTimeLimit()
    {
        // Teste de performance
    }
    
    [Test]
    [Category("Security")]
    public void ValidateUser_InvalidEmail_ShouldThrowException()
    {
        // Teste de segurança
    }
}
```

#### Test Parallelization
```csharp
[TestFixture]
[Parallelizable(ParallelScope.Fixtures)]
public class ParallelTests
{
    [Test]
    [Parallelizable(ParallelScope.Self)]
    public void Test1()
    {
        Thread.Sleep(1000);
        Assert.Pass();
    }
    
    [Test]
    [Parallelizable(ParallelScope.Self)]
    public void Test2()
    {
        Thread.Sleep(1000);
        Assert.Pass();
    }
}
```

## Exercícios

### Exercício 1 - Property-Based Testing
Implemente testes baseados em propriedades para uma calculadora.

### Exercício 2 - Performance Testing
Crie testes de performance para operações de banco de dados.

### Exercício 3 - Integration Testing
Implemente testes de integração para uma API REST.

## Dicas
- Use builders para criar dados de teste complexos
- Implemente testes de propriedade para validação de regras de negócio
- Monitore performance de testes críticos
- Use containers para testes de integração
- Implemente mutation testing para validar cobertura
- Use contract testing para APIs
- Organize testes por categoria
- Implemente cleanup adequado em testes de integração 