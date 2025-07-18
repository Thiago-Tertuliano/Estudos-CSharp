# Aula 1 - Memory Management e Performance

## Objetivos da Aula
- Entender o Garbage Collector do .NET
- Aprender técnicas de otimização de memória
- Compreender Value Types vs Reference Types
- Praticar profiling e análise de performance

## Conteúdo Teórico

### Garbage Collector (GC)

#### Como Funciona o GC
O Garbage Collector do .NET gerencia automaticamente a memória, liberando objetos não utilizados.

```csharp
// Geração 0 - Objetos de vida curta
var tempObject = new MyClass(); // Vai para Gen 0

// Geração 1 - Objetos que sobreviveram a uma coleta
// Geração 2 - Objetos de vida longa
var longLivedObject = new MyClass(); // Pode ir para Gen 2
```

#### Tipos de Coleta
```csharp
// Coleta de Geração 0 (mais frequente)
// Coleta de Geração 1 (menos frequente)
// Coleta de Geração 2 (mais rara)

// Forçar coleta (não recomendado em produção)
GC.Collect();
GC.WaitForPendingFinalizers();
```

### Value Types vs Reference Types

#### Value Types (Stack)
```csharp
public struct Point
{
    public int X { get; set; }
    public int Y { get; set; }
}

// Alocado na stack
Point p1 = new Point { X = 10, Y = 20 };
Point p2 = p1; // Cópia por valor
p2.X = 30; // Não afeta p1
```

#### Reference Types (Heap)
```csharp
public class Person
{
    public string Name { get; set; }
    public int Age { get; set; }
}

// Alocado no heap
Person person1 = new Person { Name = "João", Age = 25 };
Person person2 = person1; // Referência
person2.Name = "Maria"; // Afeta person1
```

### Boxing e Unboxing

#### Boxing (Performance Impact)
```csharp
// Boxing - Value type para reference type
int number = 42;
object boxed = number; // Boxing ocorre aqui

// Unboxing - Reference type para value type
int unboxed = (int)boxed; // Unboxing ocorre aqui

// Evitar boxing
var numbers = new List<int>(); // Usar List<int> em vez de List<object>
```

#### Evitando Boxing
```csharp
// Ruim - Boxing
ArrayList list = new ArrayList();
list.Add(42); // Boxing

// Bom - Sem boxing
List<int> numbers = new List<int>();
numbers.Add(42); // Sem boxing

// Interface genérica
IEnumerable<int> enumerable = numbers; // Sem boxing
```

### Memory Pools

#### Object Pooling
```csharp
public class ObjectPool<T> where T : class, new()
{
    private readonly ConcurrentQueue<T> _pool = new();
    private readonly int _maxSize;
    
    public ObjectPool(int maxSize = 100)
    {
        _maxSize = maxSize;
    }
    
    public T Get()
    {
        if (_pool.TryDequeue(out T item))
        {
            return item;
        }
        return new T();
    }
    
    public void Return(T item)
    {
        if (_pool.Count < _maxSize)
        {
            _pool.Enqueue(item);
        }
    }
}

// Uso
var pool = new ObjectPool<MyClass>();
var obj = pool.Get();
// Usar obj
pool.Return(obj);
```

#### ArrayPool
```csharp
// Usar ArrayPool para arrays grandes
var pool = ArrayPool<byte>.Shared;
byte[] buffer = pool.Rent(1024);

try
{
    // Usar buffer
    ProcessData(buffer);
}
finally
{
    pool.Return(buffer);
}
```

### Span<T> e Memory<T>

#### Span<T> para Performance
```csharp
public static int SumSpan(ReadOnlySpan<int> numbers)
{
    int sum = 0;
    for (int i = 0; i < numbers.Length; i++)
    {
        sum += numbers[i];
    }
    return sum;
}

// Uso
int[] array = { 1, 2, 3, 4, 5 };
Span<int> span = array;
int result = SumSpan(span);

// Com stackalloc
Span<int> stackSpan = stackalloc int[10];
for (int i = 0; i < stackSpan.Length; i++)
{
    stackSpan[i] = i;
}
```

#### Memory<T> para Async
```csharp
public async Task ProcessDataAsync(Memory<byte> data)
{
    // Processar dados de forma assíncrona
    await Task.Delay(100);
    
    for (int i = 0; i < data.Length; i++)
    {
        data.Span[i] = (byte)(data.Span[i] + 1);
    }
}

// Uso
byte[] buffer = new byte[1024];
Memory<byte> memory = buffer;
await ProcessDataAsync(memory);
```

### Structs Otimizados

#### Structs Pequenos
```csharp
public readonly struct Vector2D
{
    public readonly float X;
    public readonly float Y;
    
    public Vector2D(float x, float y)
    {
        X = x;
        Y = y;
    }
    
    public float Magnitude => MathF.Sqrt(X * X + Y * Y);
    
    public static Vector2D operator +(Vector2D a, Vector2D b)
        => new Vector2D(a.X + b.X, a.Y + b.Y);
}

// Uso - Sem alocação no heap
Vector2D v1 = new Vector2D(1, 2);
Vector2D v2 = new Vector2D(3, 4);
Vector2D result = v1 + v2;
```

#### Structs com Ref Semantics
```csharp
public ref struct RefStruct
{
    public int Value;
    
    public void Increment()
    {
        Value++;
    }
}

// Só pode ser usado em stack
public void ProcessRefStruct()
{
    var refStruct = new RefStruct { Value = 10 };
    refStruct.Increment();
    Console.WriteLine(refStruct.Value); // 11
}
```

### Weak References

#### WeakReference para Cache
```csharp
public class Cache<T> where T : class
{
    private readonly Dictionary<string, WeakReference<T>> _cache = new();
    
    public T GetOrCreate(string key, Func<T> factory)
    {
        if (_cache.TryGetValue(key, out var weakRef) && 
            weakRef.TryGetTarget(out T value))
        {
            return value;
        }
        
        value = factory();
        _cache[key] = new WeakReference<T>(value);
        return value;
    }
    
    public void Cleanup()
    {
        var keysToRemove = new List<string>();
        
        foreach (var kvp in _cache)
        {
            if (!kvp.Value.TryGetTarget(out _))
            {
                keysToRemove.Add(kvp.Key);
            }
        }
        
        foreach (var key in keysToRemove)
        {
            _cache.Remove(key);
        }
    }
}
```

### Memory Profiling

#### Usando dotMemory
```csharp
// Código para profiling
public class MemoryIntensiveClass
{
    private List<string> _data = new();
    
    public void AddData(string item)
    {
        _data.Add(item);
    }
    
    public void ClearData()
    {
        _data.Clear();
    }
}

// Profiling com BenchmarkDotNet
[MemoryDiagnoser]
public class MemoryBenchmark
{
    [Benchmark]
    public void AllocateObjects()
    {
        var list = new List<object>();
        for (int i = 0; i < 1000; i++)
        {
            list.Add(new object());
        }
    }
    
    [Benchmark]
    public void AllocateStructs()
    {
        var list = new List<Vector2D>();
        for (int i = 0; i < 1000; i++)
        {
            list.Add(new Vector2D(i, i));
        }
    }
}
```

### Performance Tips

#### Evitando Alocações
```csharp
// Ruim - Alocações desnecessárias
public string ProcessData(string input)
{
    var result = "";
    for (int i = 0; i < input.Length; i++)
    {
        result += input[i]; // Alocação a cada concatenação
    }
    return result;
}

// Bom - StringBuilder
public string ProcessDataOptimized(string input)
{
    var sb = new StringBuilder();
    for (int i = 0; i < input.Length; i++)
    {
        sb.Append(input[i]);
    }
    return sb.ToString();
}

// Melhor - Span<char>
public string ProcessDataWithSpan(ReadOnlySpan<char> input)
{
    var result = new char[input.Length];
    for (int i = 0; i < input.Length; i++)
    {
        result[i] = input[i];
    }
    return new string(result);
}
```

#### Structs vs Classes
```csharp
// Use structs para dados pequenos e imutáveis
public readonly struct Color
{
    public readonly byte R, G, B;
    
    public Color(byte r, byte g, byte b)
    {
        R = r;
        G = g;
        B = b;
    }
}

// Use classes para objetos complexos
public class Image
{
    public int Width { get; set; }
    public int Height { get; set; }
    public Color[,] Pixels { get; set; }
    
    public Image(int width, int height)
    {
        Width = width;
        Height = height;
        Pixels = new Color[width, height];
    }
}
```

### Memory Leaks

#### Detecção de Memory Leaks
```csharp
public class MemoryLeakExample
{
    private static List<object> _staticList = new();
    
    // Memory leak - objetos nunca são liberados
    public void AddToStaticList(object item)
    {
        _staticList.Add(item);
    }
    
    // Solução - usar WeakReference
    private static List<WeakReference> _weakList = new();
    
    public void AddToWeakList(object item)
    {
        _weakList.Add(new WeakReference(item));
    }
}
```

#### Event Handlers
```csharp
public class EventHandlerLeak
{
    public event EventHandler MyEvent;
    
    public void Subscribe(EventHandler handler)
    {
        MyEvent += handler; // Pode causar memory leak
    }
    
    public void Unsubscribe(EventHandler handler)
    {
        MyEvent -= handler; // Sempre desinscrever
    }
}

// Solução - usar WeakEventManager
public class WeakEventHandler
{
    private WeakReference<EventHandler> _handler;
    
    public void Subscribe(EventHandler handler)
    {
        _handler = new WeakReference<EventHandler>(handler);
    }
    
    public void RaiseEvent()
    {
        if (_handler.TryGetTarget(out var handler))
        {
            handler?.Invoke(this, EventArgs.Empty);
        }
    }
}
```

### Garbage Collection Tuning

#### Configuração do GC
```csharp
// Configurar GC para servidor
GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;

// Verificar se é servidor GC
Console.WriteLine($"Server GC: {GCSettings.IsServerGC}");

// Configurar para workstation
GCSettings.LatencyMode = GCLatencyMode.Interactive;
```

#### Finalizers e Dispose
```csharp
public class ResourceManager : IDisposable
{
    private bool _disposed = false;
    private IntPtr _handle;
    
    public ResourceManager()
    {
        _handle = Marshal.AllocHGlobal(1024);
    }
    
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                // Liberar recursos gerenciados
            }
            
            // Liberar recursos não gerenciados
            if (_handle != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(_handle);
                _handle = IntPtr.Zero;
            }
            
            _disposed = true;
        }
    }
    
    ~ResourceManager()
    {
        Dispose(false);
    }
}
```

## Exercícios

### Exercício 1 - Memory Profiling
Crie um programa que demonstre diferentes padrões de uso de memória e use ferramentas de profiling.

### Exercício 2 - Object Pooling
Implemente um sistema de object pooling para objetos custosos de criar.

### Exercício 3 - Performance Optimization
Otimize um algoritmo existente usando técnicas de memory management.

## Dicas
- Use structs para dados pequenos e imutáveis
- Evite boxing/unboxing desnecessário
- Implemente IDisposable para recursos não gerenciados
- Use Span<T> e Memory<T> para operações de alto desempenho
- Monitore o uso de memória em aplicações críticas
- Configure o GC adequadamente para seu cenário
- Use object pooling para objetos custosos
- Sempre desinscreva de eventos para evitar memory leaks 