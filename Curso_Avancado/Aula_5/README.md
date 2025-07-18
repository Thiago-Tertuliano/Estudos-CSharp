# Aula 5 - Advanced Async Patterns

## Objetivos da Aula
- Entender padrões assíncronos avançados
- Aprender TAP (Task-based Asynchronous Pattern)
- Compreender async/await patterns complexos
- Praticar implementação de operações assíncronas

## Conteúdo Teórico

### Task-based Asynchronous Pattern (TAP)

#### Princípios do TAP
```csharp
// Bom - TAP Pattern
public async Task<User> GetUserAsync(int userId)
{
    // Operação assíncrona
    var user = await _repository.GetByIdAsync(userId);
    return user;
}

// Ruim - APM Pattern (legacy)
public IAsyncResult BeginGetUser(int userId, AsyncCallback callback, object state)
{
    // Não usar este padrão
}

// Ruim - EAP Pattern (legacy)
public void GetUserAsync(int userId)
{
    // Não usar este padrão
}
```

#### Task Creation Patterns
```csharp
// Task.FromResult - Para valores já calculados
public async Task<int> GetCachedValueAsync()
{
    var cachedValue = await _cache.GetAsync("key");
    if (cachedValue != null)
    {
        return Task.FromResult(cachedValue.Value);
    }
    
    var newValue = await CalculateValueAsync();
    await _cache.SetAsync("key", newValue);
    return newValue;
}

// Task.CompletedTask - Para operações void
public async Task LogAsync(string message)
{
    if (string.IsNullOrEmpty(message))
    {
        return Task.CompletedTask;
    }
    
    await _logger.LogAsync(message);
}

// Task.FromException - Para exceções
public async Task<User> GetUserAsync(int userId)
{
    if (userId <= 0)
    {
        return await Task.FromException<User>(new ArgumentException("Invalid user ID"));
    }
    
    return await _repository.GetByIdAsync(userId);
}
```

### Cancellation Support

#### CancellationToken Usage
```csharp
public async Task<User> GetUserAsync(int userId, CancellationToken cancellationToken = default)
{
    // Verificar cancelamento
    cancellationToken.ThrowIfCancellationRequested();
    
    var user = await _repository.GetByIdAsync(userId, cancellationToken);
    
    // Verificar novamente após operação longa
    cancellationToken.ThrowIfCancellationRequested();
    
    return user;
}

// Uso com timeout
public async Task<User> GetUserWithTimeoutAsync(int userId, TimeSpan timeout)
{
    using var cts = new CancellationTokenSource(timeout);
    return await GetUserAsync(userId, cts.Token);
}
```

#### CancellationTokenSource Patterns
```csharp
public class AsyncOperationManager
{
    private CancellationTokenSource _cts;
    
    public async Task StartOperationAsync()
    {
        _cts = new CancellationTokenSource();
        
        try
        {
            await LongRunningOperationAsync(_cts.Token);
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("Operation was cancelled");
        }
    }
    
    public void CancelOperation()
    {
        _cts?.Cancel();
    }
    
    private async Task LongRunningOperationAsync(CancellationToken cancellationToken)
    {
        for (int i = 0; i < 100; i++)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await Task.Delay(100, cancellationToken);
        }
    }
}
```

### Async Streams (C# 8.0+)

#### IAsyncEnumerable Pattern
```csharp
public async IAsyncEnumerable<User> GetUsersAsync([System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken = default)
{
    var page = 1;
    const int pageSize = 100;
    
    while (true)
    {
        var users = await _repository.GetUsersPageAsync(page, pageSize, cancellationToken);
        
        if (!users.Any())
            break;
        
        foreach (var user in users)
        {
            yield return user;
        }
        
        page++;
    }
}

// Consumindo async streams
public async Task ProcessAllUsersAsync()
{
    await foreach (var user in GetUsersAsync())
    {
        await ProcessUserAsync(user);
    }
}
```

#### Async Streams com LINQ
```csharp
public static class AsyncEnumerableExtensions
{
    public static async IAsyncEnumerable<T> WhereAsync<T>(
        this IAsyncEnumerable<T> source,
        Func<T, Task<bool>> predicate)
    {
        await foreach (var item in source)
        {
            if (await predicate(item))
            {
                yield return item;
            }
        }
    }
    
    public static async IAsyncEnumerable<TResult> SelectAsync<T, TResult>(
        this IAsyncEnumerable<T> source,
        Func<T, Task<TResult>> selector)
    {
        await foreach (var item in source)
        {
            yield return await selector(item);
        }
    }
}

// Uso
public async Task ProcessActiveUsersAsync()
{
    var activeUsers = GetUsersAsync()
        .WhereAsync(async user => await _service.IsActiveAsync(user.Id))
        .SelectAsync(async user => await _service.EnrichUserAsync(user));
    
    await foreach (var user in activeUsers)
    {
        await ProcessUserAsync(user);
    }
}
```

### ValueTask for Performance

#### Quando usar ValueTask
```csharp
// Use ValueTask para operações que frequentemente são síncronas
public async ValueTask<int> GetCachedValueAsync(string key)
{
    var cached = _cache.Get(key);
    if (cached != null)
    {
        return cached.Value; // Retorno síncrono
    }
    
    var value = await CalculateValueAsync(key);
    _cache.Set(key, value);
    return value;
}

// Use Task para operações sempre assíncronas
public async Task<User> GetUserFromDatabaseAsync(int userId)
{
    return await _database.GetUserAsync(userId); // Sempre assíncrono
}
```

#### ValueTask Best Practices
```csharp
public class AsyncCache<T>
{
    private readonly Dictionary<string, T> _cache = new();
    private readonly SemaphoreSlim _semaphore = new(1, 1);
    
    public async ValueTask<T> GetOrAddAsync(string key, Func<Task<T>> factory)
    {
        if (_cache.TryGetValue(key, out var value))
        {
            return value; // Retorno síncrono
        }
        
        await _semaphore.WaitAsync();
        try
        {
            // Verificar novamente após adquirir lock
            if (_cache.TryGetValue(key, out value))
            {
                return value;
            }
            
            value = await factory();
            _cache[key] = value;
            return value;
        }
        finally
        {
            _semaphore.Release();
        }
    }
}
```

### ConfigureAwait Pattern

#### ConfigureAwait Usage
```csharp
public async Task ProcessDataAsync()
{
    // Em aplicações desktop/WPF - manter contexto de sincronização
    var data = await GetDataAsync(); // Mantém contexto
    
    // Em aplicações web/console - não manter contexto
    var processedData = await ProcessDataAsync().ConfigureAwait(false);
    
    // Em bibliotecas - sempre usar ConfigureAwait(false)
    await SaveDataAsync(processedData).ConfigureAwait(false);
}

// Em bibliotecas
public static class AsyncExtensions
{
    public static async Task<T> WithTimeoutAsync<T>(this Task<T> task, TimeSpan timeout)
    {
        using var cts = new CancellationTokenSource(timeout);
        var timeoutTask = Task.Delay(timeout, cts.Token);
        
        var completedTask = await Task.WhenAny(task, timeoutTask).ConfigureAwait(false);
        
        if (completedTask == timeoutTask)
        {
            throw new TimeoutException();
        }
        
        return await task.ConfigureAwait(false);
    }
}
```

### Async Patterns

#### Async Factory Pattern
```csharp
public class DatabaseConnection
{
    private DatabaseConnection() { }
    
    public static async Task<DatabaseConnection> CreateAsync(string connectionString)
    {
        var connection = new DatabaseConnection();
        await connection.InitializeAsync(connectionString);
        return connection;
    }
    
    private async Task InitializeAsync(string connectionString)
    {
        // Inicialização assíncrona
        await Task.Delay(100);
    }
}

// Uso
public async Task ProcessWithDatabaseAsync()
{
    using var connection = await DatabaseConnection.CreateAsync("connectionString");
    // Usar connection
}
```

#### Async Lazy Pattern
```csharp
public class AsyncLazy<T>
{
    private readonly Lazy<Task<T>> _lazy;
    
    public AsyncLazy(Func<Task<T>> factory)
    {
        _lazy = new Lazy<Task<T>>(factory);
    }
    
    public Task<T> Value => _lazy.Value;
}

// Uso
public class UserService
{
    private readonly AsyncLazy<IUserRepository> _repository;
    
    public UserService()
    {
        _repository = new AsyncLazy<IUserRepository>(async () =>
        {
            var connection = await DatabaseConnection.CreateAsync("connectionString");
            return new UserRepository(connection);
        });
    }
    
    public async Task<User> GetUserAsync(int id)
    {
        var repo = await _repository.Value;
        return await repo.GetByIdAsync(id);
    }
}
```

### Task Combinators

#### Task.WhenAll Pattern
```csharp
public async Task<UserProfile> GetUserProfileAsync(int userId)
{
    var userTask = _userService.GetUserAsync(userId);
    var ordersTask = _orderService.GetUserOrdersAsync(userId);
    var preferencesTask = _preferenceService.GetUserPreferencesAsync(userId);
    
    // Executar todas as tarefas em paralelo
    await Task.WhenAll(userTask, ordersTask, preferencesTask);
    
    return new UserProfile
    {
        User = await userTask,
        Orders = await ordersTask,
        Preferences = await preferencesTask
    };
}
```

#### Task.WhenAny Pattern
```csharp
public async Task<string> GetDataFromMultipleSourcesAsync()
{
    var tasks = new[]
    {
        _cache.GetAsync("key"),
        _database.GetAsync("key"),
        _api.GetAsync("key")
    };
    
    // Retornar o primeiro que completar
    var completedTask = await Task.WhenAny(tasks);
    return await completedTask;
}
```

#### Task.WhenAny with Timeout
```csharp
public async Task<T> GetWithTimeoutAsync<T>(Task<T> task, TimeSpan timeout)
{
    var timeoutTask = Task.Delay(timeout).ContinueWith(_ => default(T));
    var completedTask = await Task.WhenAny(task, timeoutTask);
    
    if (completedTask == timeoutTask)
    {
        throw new TimeoutException();
    }
    
    return await task;
}
```

### Async Exception Handling

#### Exception Handling Patterns
```csharp
public async Task<User> GetUserSafelyAsync(int userId)
{
    try
    {
        return await _repository.GetByIdAsync(userId);
    }
    catch (Exception ex) when (ex is not OperationCanceledException)
    {
        _logger.LogError(ex, "Error getting user {UserId}", userId);
        return await _fallbackRepository.GetByIdAsync(userId);
    }
}

// Aggregate Exception Handling
public async Task ProcessUsersAsync(IEnumerable<int> userIds)
{
    var tasks = userIds.Select(id => GetUserSafelyAsync(id));
    var results = await Task.WhenAll(tasks);
    
    // Ou capturar exceções individuais
    var tasksWithResults = userIds.Select(async id =>
    {
        try
        {
            return await GetUserSafelyAsync(id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing user {UserId}", id);
            return null;
        }
    });
    
    var results2 = await Task.WhenAll(tasksWithResults);
}
```

### Async Performance Patterns

#### Async Batching
```csharp
public class AsyncBatchProcessor<T>
{
    private readonly SemaphoreSlim _semaphore;
    private readonly int _batchSize;
    private readonly TimeSpan _batchTimeout;
    
    public AsyncBatchProcessor(int maxConcurrency, int batchSize, TimeSpan batchTimeout)
    {
        _semaphore = new SemaphoreSlim(maxConcurrency);
        _batchSize = batchSize;
        _batchTimeout = batchTimeout;
    }
    
    public async Task ProcessBatchAsync(IEnumerable<T> items, Func<T, Task> processor)
    {
        var batches = items.Chunk(_batchSize);
        
        foreach (var batch in batches)
        {
            var tasks = batch.Select(async item =>
            {
                await _semaphore.WaitAsync();
                try
                {
                    await processor(item);
                }
                finally
                {
                    _semaphore.Release();
                }
            });
            
            await Task.WhenAll(tasks);
        }
    }
}
```

#### Async Throttling
```csharp
public class AsyncThrottler
{
    private readonly SemaphoreSlim _semaphore;
    
    public AsyncThrottler(int maxConcurrency)
    {
        _semaphore = new SemaphoreSlim(maxConcurrency);
    }
    
    public async Task<T> ExecuteAsync<T>(Func<Task<T>> operation)
    {
        await _semaphore.WaitAsync();
        try
        {
            return await operation();
        }
        finally
        {
            _semaphore.Release();
        }
    }
}

// Uso
public async Task ProcessUsersWithThrottlingAsync(IEnumerable<int> userIds)
{
    var throttler = new AsyncThrottler(maxConcurrency: 5);
    var tasks = userIds.Select(id => 
        throttler.ExecuteAsync(async () => await ProcessUserAsync(id)));
    
    await Task.WhenAll(tasks);
}
```

## Exercícios

### Exercício 1 - Async Streams
Implemente um sistema de processamento de dados usando async streams.

### Exercício 2 - Async Patterns
Crie um sistema de cache assíncrono com ValueTask.

### Exercício 3 - Performance Optimization
Otimize operações assíncronas usando batching e throttling.

## Dicas
- Use ValueTask para operações frequentemente síncronas
- Sempre suporte CancellationToken em operações assíncronas
- Use ConfigureAwait(false) em bibliotecas
- Implemente timeout para operações críticas
- Use async streams para processamento de grandes volumes
- Implemente retry logic para operações que podem falhar
- Monitore performance de operações assíncronas
- Use Task.WhenAll para operações independentes 