# Aula 7 - Advanced Generics e Constraints

## Objetivos da Aula
- Entender generics avançados
- Aprender constraints complexos
- Compreender covariant e contravariant
- Praticar implementação de tipos genéricos

## Conteúdo Teórico

### Advanced Generic Constraints

#### Multiple Constraints
```csharp
// Múltiplas constraints
public class Repository<T> where T : class, IEntity, new()
{
    public T Create()
    {
        return new T();
    }
    
    public void Save(T entity)
    {
        entity.Id = GenerateId();
        // Salvar entidade
    }
}

public interface IEntity
{
    int Id { get; set; }
}

public class User : IEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
}
```

#### Constructor Constraints
```csharp
// Constraint de construtor
public class Factory<T> where T : class, new()
{
    public T CreateInstance()
    {
        return new T();
    }
}

// Sem constraint de construtor
public class FactoryWithoutConstraint<T> where T : class
{
    public T CreateInstance()
    {
        // Não pode usar new T() sem constraint
        return Activator.CreateInstance<T>();
    }
}
```

#### Struct Constraints
```csharp
// Struct constraint
public class ValueContainer<T> where T : struct
{
    private T _value;
    
    public T Value
    {
        get => _value;
        set => _value = value;
    }
    
    public bool HasValue => !_value.Equals(default(T));
}

// Nullable struct constraint
public class NullableContainer<T> where T : struct
{
    private T? _value;
    
    public T? Value
    {
        get => _value;
        set => _value = value;
    }
    
    public bool HasValue => _value.HasValue;
}
```

### Variance (Covariance e Contravariance)

#### Covariance (out)
```csharp
// Covariant interface
public interface IProducer<out T>
{
    T Produce();
}

public class StringProducer : IProducer<string>
{
    public string Produce() => "Hello";
}

public class ObjectProducer : IProducer<object>
{
    public object Produce() => new object();
}

// Uso de covariança
IProducer<string> stringProducer = new StringProducer();
IProducer<object> objectProducer = stringProducer; // Covariant assignment
```

#### Contravariance (in)
```csharp
// Contravariant interface
public interface IConsumer<in T>
{
    void Consume(T item);
}

public class ObjectConsumer : IConsumer<object>
{
    public void Consume(object item) => Console.WriteLine(item);
}

public class StringConsumer : IConsumer<string>
{
    public void Consume(string item) => Console.WriteLine(item);
}

// Uso de contravariância
IConsumer<object> objectConsumer = new ObjectConsumer();
IConsumer<string> stringConsumer = objectConsumer; // Contravariant assignment
```

#### Variance in Delegates
```csharp
// Covariant delegate
public delegate T CovariantDelegate<out T>();

// Contravariant delegate
public delegate void ContravariantDelegate<in T>(T item);

// Uso
CovariantDelegate<string> stringDelegate = () => "Hello";
CovariantDelegate<object> objectDelegate = stringDelegate; // Covariant

ContravariantDelegate<object> objectConsumer = (obj) => Console.WriteLine(obj);
ContravariantDelegate<string> stringConsumer = objectConsumer; // Contravariant
```

### Generic Type Inference

#### Type Inference Patterns
```csharp
public class TypeInferenceExample
{
    // Método genérico com inferência
    public static T Create<T>() where T : class, new()
    {
        return new T();
    }
    
    // Método genérico com parâmetros
    public static T Max<T>(T a, T b) where T : IComparable<T>
    {
        return a.CompareTo(b) > 0 ? a : b;
    }
    
    // Método genérico com múltiplos tipos
    public static TResult Convert<TInput, TResult>(TInput input, Func<TInput, TResult> converter)
    {
        return converter(input);
    }
}

// Uso com inferência
var user = TypeInferenceExample.Create<User>(); // Inferência explícita
var max = TypeInferenceExample.Max(10, 20); // Inferência automática
var result = TypeInferenceExample.Convert("123", int.Parse); // Inferência automática
```

### Generic Collections and Data Structures

#### Generic Stack Implementation
```csharp
public class GenericStack<T>
{
    private readonly List<T> _items = new();
    
    public void Push(T item)
    {
        _items.Add(item);
    }
    
    public T Pop()
    {
        if (_items.Count == 0)
            throw new InvalidOperationException("Stack is empty");
        
        var item = _items[_items.Count - 1];
        _items.RemoveAt(_items.Count - 1);
        return item;
    }
    
    public T Peek()
    {
        if (_items.Count == 0)
            throw new InvalidOperationException("Stack is empty");
        
        return _items[_items.Count - 1];
    }
    
    public int Count => _items.Count;
    public bool IsEmpty => _items.Count == 0;
}
```

#### Generic Binary Tree
```csharp
public class BinaryTreeNode<T> where T : IComparable<T>
{
    public T Value { get; set; }
    public BinaryTreeNode<T> Left { get; set; }
    public BinaryTreeNode<T> Right { get; set; }
    
    public BinaryTreeNode(T value)
    {
        Value = value;
    }
}

public class BinaryTree<T> where T : IComparable<T>
{
    private BinaryTreeNode<T> _root;
    
    public void Insert(T value)
    {
        _root = InsertRecursive(_root, value);
    }
    
    private BinaryTreeNode<T> InsertRecursive(BinaryTreeNode<T> node, T value)
    {
        if (node == null)
            return new BinaryTreeNode<T>(value);
        
        if (value.CompareTo(node.Value) < 0)
            node.Left = InsertRecursive(node.Left, value);
        else if (value.CompareTo(node.Value) > 0)
            node.Right = InsertRecursive(node.Right, value);
        
        return node;
    }
    
    public bool Contains(T value)
    {
        return ContainsRecursive(_root, value);
    }
    
    private bool ContainsRecursive(BinaryTreeNode<T> node, T value)
    {
        if (node == null)
            return false;
        
        if (value.CompareTo(node.Value) == 0)
            return true;
        
        if (value.CompareTo(node.Value) < 0)
            return ContainsRecursive(node.Left, value);
        
        return ContainsRecursive(node.Right, value);
    }
}
```

### Generic Algorithms

#### Generic Sorting
```csharp
public static class GenericAlgorithms
{
    public static void BubbleSort<T>(T[] array) where T : IComparable<T>
    {
        for (int i = 0; i < array.Length - 1; i++)
        {
            for (int j = 0; j < array.Length - i - 1; j++)
            {
                if (array[j].CompareTo(array[j + 1]) > 0)
                {
                    var temp = array[j];
                    array[j] = array[j + 1];
                    array[j + 1] = temp;
                }
            }
        }
    }
    
    public static void QuickSort<T>(T[] array) where T : IComparable<T>
    {
        QuickSortRecursive(array, 0, array.Length - 1);
    }
    
    private static void QuickSortRecursive<T>(T[] array, int low, int high) where T : IComparable<T>
    {
        if (low < high)
        {
            int pi = Partition(array, low, high);
            QuickSortRecursive(array, low, pi - 1);
            QuickSortRecursive(array, pi + 1, high);
        }
    }
    
    private static int Partition<T>(T[] array, int low, int high) where T : IComparable<T>
    {
        T pivot = array[high];
        int i = low - 1;
        
        for (int j = low; j < high; j++)
        {
            if (array[j].CompareTo(pivot) <= 0)
            {
                i++;
                var temp = array[i];
                array[i] = array[j];
                array[j] = temp;
            }
        }
        
        var temp2 = array[i + 1];
        array[i + 1] = array[high];
        array[high] = temp2;
        
        return i + 1;
    }
}
```

### Generic Design Patterns

#### Generic Factory Pattern
```csharp
public interface IProduct
{
    void DoSomething();
}

public class ProductA : IProduct
{
    public void DoSomething() => Console.WriteLine("ProductA");
}

public class ProductB : IProduct
{
    public void DoSomething() => Console.WriteLine("ProductB");
}

public class GenericFactory<T> where T : IProduct, new()
{
    public T CreateProduct()
    {
        return new T();
    }
}

// Factory com registro
public class ProductFactory
{
    private readonly Dictionary<Type, Func<IProduct>> _factories = new();
    
    public void Register<T>(Func<T> factory) where T : IProduct
    {
        _factories[typeof(T)] = () => factory();
    }
    
    public T Create<T>() where T : IProduct
    {
        if (_factories.TryGetValue(typeof(T), out var factory))
        {
            return (T)factory();
        }
        
        throw new InvalidOperationException($"No factory registered for {typeof(T)}");
    }
}
```

#### Generic Repository Pattern
```csharp
public interface IRepository<T> where T : class, IEntity
{
    Task<T> GetByIdAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(int id);
}

public class GenericRepository<T> : IRepository<T> where T : class, IEntity
{
    private readonly DbContext _context;
    private readonly DbSet<T> _dbSet;
    
    public GenericRepository(DbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }
    
    public async Task<T> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }
    
    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }
    
    public async Task<T> AddAsync(T entity)
    {
        _dbSet.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }
    
    public async Task UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        await _context.SaveChangesAsync();
    }
    
    public async Task DeleteAsync(int id)
    {
        var entity = await GetByIdAsync(id);
        if (entity != null)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
```

### Generic Constraints with Reflection

#### Generic Type Validation
```csharp
public static class GenericTypeValidator
{
    public static bool ValidateConstraints<T>()
    {
        var type = typeof(T);
        
        // Verificar se implementa interface
        if (!typeof(IEntity).IsAssignableFrom(type))
            return false;
        
        // Verificar se tem construtor sem parâmetros
        if (type.GetConstructor(Type.EmptyTypes) == null)
            return false;
        
        // Verificar se é classe
        if (!type.IsClass)
            return false;
        
        return true;
    }
    
    public static void ValidateTypeAtRuntime<T>() where T : class, IEntity, new()
    {
        if (!ValidateConstraints<T>())
        {
            throw new InvalidOperationException($"Type {typeof(T)} does not meet constraints");
        }
    }
}
```

### Advanced Generic Patterns

#### Generic Builder Pattern
```csharp
public class GenericBuilder<T> where T : class, new()
{
    private readonly T _instance;
    private readonly Dictionary<string, object> _properties = new();
    
    public GenericBuilder()
    {
        _instance = new T();
    }
    
    public GenericBuilder<T> WithProperty<TProperty>(string propertyName, TProperty value)
    {
        _properties[propertyName] = value;
        return this;
    }
    
    public T Build()
    {
        var type = typeof(T);
        
        foreach (var property in _properties)
        {
            var propInfo = type.GetProperty(property.Key);
            if (propInfo != null && propInfo.CanWrite)
            {
                propInfo.SetValue(_instance, property.Value);
            }
        }
        
        return _instance;
    }
}

// Uso
var user = new GenericBuilder<User>()
    .WithProperty("Name", "John")
    .WithProperty("Email", "john@example.com")
    .Build();
```

#### Generic Strategy Pattern
```csharp
public interface IStrategy<T>
{
    void Execute(T data);
}

public class StrategyA<T> : IStrategy<T>
{
    public void Execute(T data) => Console.WriteLine($"StrategyA: {data}");
}

public class StrategyB<T> : IStrategy<T>
{
    public void Execute(T data) => Console.WriteLine($"StrategyB: {data}");
}

public class GenericContext<T>
{
    private IStrategy<T> _strategy;
    
    public void SetStrategy(IStrategy<T> strategy)
    {
        _strategy = strategy;
    }
    
    public void ExecuteStrategy(T data)
    {
        _strategy?.Execute(data);
    }
}
```

### Generic Performance Optimization

#### Generic Object Pool
```csharp
public class GenericObjectPool<T> where T : class, new()
{
    private readonly ConcurrentQueue<T> _pool = new();
    private readonly int _maxSize;
    private int _currentSize;
    
    public GenericObjectPool(int maxSize = 100)
    {
        _maxSize = maxSize;
    }
    
    public T Get()
    {
        if (_pool.TryDequeue(out T item))
        {
            return item;
        }
        
        if (_currentSize < _maxSize)
        {
            Interlocked.Increment(ref _currentSize);
            return new T();
        }
        
        // Aguardar até que um item seja retornado
        while (!_pool.TryDequeue(out item))
        {
            Thread.Sleep(1);
        }
        
        return item;
    }
    
    public void Return(T item)
    {
        if (item != null && _pool.Count < _maxSize)
        {
            _pool.Enqueue(item);
        }
    }
}
```

## Exercícios

### Exercício 1 - Generic Data Structures
Implemente uma árvore AVL genérica com constraints apropriados.

### Exercício 2 - Generic Algorithms
Crie algoritmos de ordenação genéricos com diferentes constraints.

### Exercício 3 - Generic Design Patterns
Implemente padrões de design usando generics avançados.

## Dicas
- Use constraints apropriados para cada cenário
- Considere variance para interfaces genéricas
- Implemente validação de tipos em runtime quando necessário
- Use reflection com cuidado em código genérico
- Considere performance ao usar generics complexos
- Documente constraints e suas implicações
- Teste tipos genéricos com diferentes tipos de dados
- Use generic algorithms para reutilização de código 