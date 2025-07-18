# Aula 6 - Reactive Programming

## Objetivos da Aula
- Entender Reactive Programming (RP)
- Aprender Rx.NET (Reactive Extensions)
- Compreender Observable/Observer pattern
- Praticar programação reativa

## Conteúdo Teórico

### Reactive Programming Fundamentals

#### Observable/Observer Pattern
```csharp
public interface IObservable<T>
{
    IDisposable Subscribe(IObserver<T> observer);
}

public interface IObserver<T>
{
    void OnNext(T value);
    void OnError(Exception error);
    void OnCompleted();
}

// Implementação básica
public class Observable<T> : IObservable<T>
{
    private readonly List<IObserver<T>> _observers = new();
    
    public IDisposable Subscribe(IObserver<T> observer)
    {
        _observers.Add(observer);
        return new Unsubscriber(_observers, observer);
    }
    
    public void Publish(T value)
    {
        foreach (var observer in _observers.ToList())
        {
            observer.OnNext(value);
        }
    }
    
    public void Complete()
    {
        foreach (var observer in _observers.ToList())
        {
            observer.OnCompleted();
        }
        _observers.Clear();
    }
    
    public void Error(Exception error)
    {
        foreach (var observer in _observers.ToList())
        {
            observer.OnError(error);
        }
        _observers.Clear();
    }
    
    private class Unsubscriber : IDisposable
    {
        private readonly List<IObserver<T>> _observers;
        private readonly IObserver<T> _observer;
        
        public Unsubscriber(List<IObserver<T>> observers, IObserver<T> observer)
        {
            _observers = observers;
            _observer = observer;
        }
        
        public void Dispose()
        {
            if (_observers.Contains(_observer))
            {
                _observers.Remove(_observer);
            }
        }
    }
}
```

### Rx.NET Basics

#### Creating Observables
```csharp
using System.Reactive;
using System.Reactive.Linq;

// Observable.Return - valor único
var singleValue = Observable.Return("Hello");

// Observable.Range - sequência de números
var numbers = Observable.Range(1, 10);

// Observable.Interval - valores em intervalos
var timer = Observable.Interval(TimeSpan.FromSeconds(1));

// Observable.FromEventPattern - eventos
public class Button
{
    public event EventHandler Clicked;
    
    public void Click()
    {
        Clicked?.Invoke(this, EventArgs.Empty);
    }
}

var button = new Button();
var clicks = Observable.FromEventPattern<EventHandler, EventArgs>(
    h => button.Clicked += h,
    h => button.Clicked -= h
);
```

#### Subscribing to Observables
```csharp
// Subscribe básico
var subscription = Observable.Range(1, 5)
    .Subscribe(
        value => Console.WriteLine($"Value: {value}"),
        error => Console.WriteLine($"Error: {error}"),
        () => Console.WriteLine("Completed")
    );

// Subscribe com OnNext apenas
var subscription2 = Observable.Range(1, 5)
    .Subscribe(value => Console.WriteLine($"Value: {value}"));

// Usando Subscribe extension
var subscription3 = Observable.Range(1, 5)
    .Subscribe(
        onNext: value => Console.WriteLine($"Value: {value}"),
        onError: error => Console.WriteLine($"Error: {error}"),
        onCompleted: () => Console.WriteLine("Completed")
    );
```

### Observable Operators

#### Filtering Operators
```csharp
// Where - filtrar valores
var evenNumbers = Observable.Range(1, 10)
    .Where(x => x % 2 == 0)
    .Subscribe(x => Console.WriteLine($"Even: {x}"));

// Take - pegar primeiros N valores
var firstThree = Observable.Range(1, 10)
    .Take(3)
    .Subscribe(x => Console.WriteLine($"First three: {x}"));

// Skip - pular primeiros N valores
var skipFirstThree = Observable.Range(1, 10)
    .Skip(3)
    .Subscribe(x => Console.WriteLine($"After skip: {x}"));

// Distinct - valores únicos
var uniqueValues = Observable.Return(1, 2, 2, 3, 1, 4)
    .Distinct()
    .Subscribe(x => Console.WriteLine($"Unique: {x}"));
```

#### Transformation Operators
```csharp
// Select - transformar valores
var squares = Observable.Range(1, 5)
    .Select(x => x * x)
    .Subscribe(x => Console.WriteLine($"Square: {x}"));

// SelectMany - flatten observables
var nested = Observable.Range(1, 3)
    .SelectMany(x => Observable.Range(x, 3))
    .Subscribe(x => Console.WriteLine($"Flattened: {x}"));

// GroupBy - agrupar valores
var grouped = Observable.Range(1, 10)
    .GroupBy(x => x % 2 == 0 ? "Even" : "Odd")
    .Subscribe(group =>
    {
        group.Subscribe(x => Console.WriteLine($"{group.Key}: {x}"));
    });
```

#### Combining Operators
```csharp
// Merge - combinar múltiplos observables
var observable1 = Observable.Range(1, 3);
var observable2 = Observable.Range(4, 3);

observable1.Merge(observable2)
    .Subscribe(x => Console.WriteLine($"Merged: {x}"));

// Concat - concatenar observables
observable1.Concat(observable2)
    .Subscribe(x => Console.WriteLine($"Concatenated: {x}"));

// Zip - combinar valores de múltiplos observables
Observable.Zip(observable1, observable2)
    .Subscribe(tuple => Console.WriteLine($"Zipped: {tuple.First}, {tuple.Second}"));
```

### Error Handling

#### Error Handling Patterns
```csharp
// Catch - capturar exceções
var errorObservable = Observable.Create<int>(observer =>
{
    observer.OnNext(1);
    observer.OnNext(2);
    observer.OnError(new Exception("Something went wrong"));
    return Disposable.Empty;
});

errorObservable.Catch<int, Exception>(ex =>
{
    Console.WriteLine($"Caught error: {ex.Message}");
    return Observable.Return(-1);
})
.Subscribe(
    value => Console.WriteLine($"Value: {value}"),
    error => Console.WriteLine($"Error: {error}"),
    () => Console.WriteLine("Completed")
);

// Retry - tentar novamente
Observable.Create<int>(observer =>
{
    observer.OnNext(1);
    observer.OnError(new Exception("Temporary error"));
    return Disposable.Empty;
})
.Retry(3)
.Subscribe(
    value => Console.WriteLine($"Value: {value}"),
    error => Console.WriteLine($"Final error: {error}")
);
```

### Schedulers

#### Different Schedulers
```csharp
// CurrentThread - thread atual
Observable.Range(1, 5)
    .ObserveOn(Scheduler.CurrentThread)
    .Subscribe(x => Console.WriteLine($"Current thread: {x}"));

// ThreadPool - thread pool
Observable.Range(1, 5)
    .ObserveOn(Scheduler.ThreadPool)
    .Subscribe(x => Console.WriteLine($"Thread pool: {x}"));

// TaskPool - task scheduler
Observable.Range(1, 5)
    .ObserveOn(Scheduler.TaskPool)
    .Subscribe(x => Console.WriteLine($"Task pool: {x}"));

// UI thread (WPF/WinForms)
Observable.Range(1, 5)
    .ObserveOn(DispatcherScheduler.Current)
    .Subscribe(x => UpdateUI(x));
```

### Subject Types

#### Different Subject Types
```csharp
// Subject - broadcast para múltiplos observers
var subject = new Subject<int>();
subject.Subscribe(x => Console.WriteLine($"Observer 1: {x}"));
subject.Subscribe(x => Console.WriteLine($"Observer 2: {x}"));

subject.OnNext(1);
subject.OnNext(2);
subject.OnCompleted();

// BehaviorSubject - mantém último valor
var behaviorSubject = new BehaviorSubject<int>(0);
behaviorSubject.Subscribe(x => Console.WriteLine($"Behavior: {x}"));

behaviorSubject.OnNext(1);
behaviorSubject.OnNext(2);

// ReplaySubject - replay valores para novos observers
var replaySubject = new ReplaySubject<int>();
replaySubject.OnNext(1);
replaySubject.OnNext(2);

replaySubject.Subscribe(x => Console.WriteLine($"Replay: {x}"));

// AsyncSubject - apenas último valor quando completado
var asyncSubject = new AsyncSubject<int>();
asyncSubject.Subscribe(x => Console.WriteLine($"Async: {x}"));

asyncSubject.OnNext(1);
asyncSubject.OnNext(2);
asyncSubject.OnCompleted(); // Agora emite 2
```

### Real-World Examples

#### Event Stream Processing
```csharp
public class UserActivityTracker
{
    private readonly Subject<UserActivity> _activityStream = new();
    
    public IObservable<UserActivity> ActivityStream => _activityStream.AsObservable();
    
    public void TrackActivity(UserActivity activity)
    {
        _activityStream.OnNext(activity);
    }
    
    public void Complete()
    {
        _activityStream.OnCompleted();
    }
}

public class UserActivity
{
    public int UserId { get; set; }
    public string Action { get; set; }
    public DateTime Timestamp { get; set; }
}

// Uso
var tracker = new UserActivityTracker();

// Processar atividades em tempo real
tracker.ActivityStream
    .Where(activity => activity.Action == "login")
    .GroupBy(activity => activity.UserId)
    .Subscribe(group =>
    {
        group.Buffer(TimeSpan.FromMinutes(5))
            .Where(logins => logins.Count > 3)
            .Subscribe(logins => Console.WriteLine($"User {group.Key} logged in {logins.Count} times"));
    });

// Simular atividades
tracker.TrackActivity(new UserActivity { UserId = 1, Action = "login", Timestamp = DateTime.Now });
tracker.TrackActivity(new UserActivity { UserId = 1, Action = "logout", Timestamp = DateTime.Now });
```

#### Data Stream Processing
```csharp
public class StockPriceStream
{
    private readonly Subject<StockPrice> _priceStream = new();
    
    public IObservable<StockPrice> PriceStream => _priceStream.AsObservable();
    
    public void UpdatePrice(StockPrice price)
    {
        _priceStream.OnNext(price);
    }
}

public class StockPrice
{
    public string Symbol { get; set; }
    public decimal Price { get; set; }
    public DateTime Timestamp { get; set; }
}

// Análise de preços em tempo real
var stockStream = new StockPriceStream();

stockStream.PriceStream
    .GroupBy(price => price.Symbol)
    .Subscribe(group =>
    {
        group.Buffer(TimeSpan.FromSeconds(10))
            .Where(prices => prices.Any())
            .Subscribe(prices =>
            {
                var avg = prices.Average(p => p.Price);
                var max = prices.Max(p => p.Price);
                var min = prices.Min(p => p.Price);
                
                Console.WriteLine($"{group.Key}: Avg={avg:F2}, Max={max:F2}, Min={min:F2}");
            });
    });
```

#### UI Event Handling
```csharp
public class SearchBox
{
    public event EventHandler<string> TextChanged;
    
    public void OnTextChanged(string text)
    {
        TextChanged?.Invoke(this, text);
    }
}

// Implementar search com debounce
var searchBox = new SearchBox();

Observable.FromEventPattern<EventHandler<string>, string>(
    h => searchBox.TextChanged += h,
    h => searchBox.TextChanged -= h
)
.Select(e => e.EventArgs)
.Debounce(TimeSpan.FromMilliseconds(300))
.Where(text => text.Length >= 3)
.DistinctUntilChanged()
.Subscribe(async text =>
{
    var results = await SearchAsync(text);
    UpdateSearchResults(results);
});
```

### Advanced Patterns

#### Hot vs Cold Observables
```csharp
// Cold Observable - cada subscription cria nova sequência
var coldObservable = Observable.Create<int>(observer =>
{
    Console.WriteLine("Creating new sequence");
    observer.OnNext(1);
    observer.OnNext(2);
    observer.OnNext(3);
    observer.OnCompleted();
    return Disposable.Empty;
});

coldObservable.Subscribe(x => Console.WriteLine($"Cold 1: {x}"));
coldObservable.Subscribe(x => Console.WriteLine($"Cold 2: {x}"));

// Hot Observable - compartilhado entre subscriptions
var hotObservable = coldObservable.Publish();
hotObservable.Subscribe(x => Console.WriteLine($"Hot 1: {x}"));
hotObservable.Subscribe(x => Console.WriteLine($"Hot 2: {x}"));
hotObservable.Connect(); // Inicia a sequência
```

#### Backpressure Handling
```csharp
// Buffer - acumular valores
Observable.Interval(TimeSpan.FromMilliseconds(100))
    .Buffer(TimeSpan.FromSeconds(1))
    .Subscribe(batch => Console.WriteLine($"Batch: {string.Join(", ", batch)}"));

// Sample - pegar último valor em intervalos
Observable.Interval(TimeSpan.FromMilliseconds(100))
    .Sample(TimeSpan.FromSeconds(1))
    .Subscribe(value => Console.WriteLine($"Sampled: {value}"));

// Throttle - pegar valor após período de inatividade
Observable.Interval(TimeSpan.FromMilliseconds(100))
    .Throttle(TimeSpan.FromSeconds(1))
    .Subscribe(value => Console.WriteLine($"Throttled: {value}"));
```

#### Custom Operators
```csharp
public static class ObservableExtensions
{
    public static IObservable<T> RetryWithDelay<T>(
        this IObservable<T> source,
        int retryCount,
        TimeSpan delay)
    {
        return source.Catch<T, Exception>(ex =>
        {
            if (retryCount > 0)
            {
                return Observable.Timer(delay)
                    .SelectMany(_ => source.RetryWithDelay(retryCount - 1, delay));
            }
            return Observable.Throw<T>(ex);
        });
    }
    
    public static IObservable<T> WithTimeout<T>(
        this IObservable<T> source,
        TimeSpan timeout)
    {
        return source.Timeout(timeout);
    }
}

// Uso
Observable.Create<int>(observer =>
{
    observer.OnNext(1);
    observer.OnError(new Exception("Error"));
    return Disposable.Empty;
})
.RetryWithDelay(3, TimeSpan.FromSeconds(1))
.Subscribe(
    value => Console.WriteLine($"Value: {value}"),
    error => Console.WriteLine($"Error: {error}")
);
```

## Exercícios

### Exercício 1 - Event Stream Processing
Implemente um sistema de processamento de eventos usando Rx.NET.

### Exercício 2 - Real-time Data Analysis
Crie um sistema de análise de dados em tempo real.

### Exercício 3 - Custom Operators
Implemente operadores customizados para Rx.NET.

## Dicas
- Use schedulers apropriados para diferentes contextos
- Implemente error handling robusto
- Considere backpressure em streams de alta velocidade
- Use subjects com cuidado (podem causar memory leaks)
- Prefira cold observables quando possível
- Implemente timeout para operações críticas
- Use buffer/sample para controlar fluxo de dados
- Monitore performance de streams complexos 