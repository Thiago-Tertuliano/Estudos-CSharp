# Aula 2 - Advanced Design Patterns

## Objetivos da Aula
- Entender padrões de design avançados
- Aprender padrões arquiteturais complexos
- Compreender padrões de concorrência
- Praticar implementação de padrões empresariais

## Conteúdo Teórico

### Command Pattern

#### Implementação Básica
```csharp
public interface ICommand
{
    void Execute();
    void Undo();
}

public class Light
{
    public void TurnOn() => Console.WriteLine("Light is ON");
    public void TurnOff() => Console.WriteLine("Light is OFF");
}

public class LightOnCommand : ICommand
{
    private readonly Light _light;
    
    public LightOnCommand(Light light)
    {
        _light = light;
    }
    
    public void Execute() => _light.TurnOn();
    public void Undo() => _light.TurnOff();
}

public class LightOffCommand : ICommand
{
    private readonly Light _light;
    
    public LightOffCommand(Light light)
    {
        _light = light;
    }
    
    public void Execute() => _light.TurnOff();
    public void Undo() => _light.TurnOn();
}

public class RemoteControl
{
    private readonly Stack<ICommand> _commandHistory = new();
    
    public void PressButton(ICommand command)
    {
        command.Execute();
        _commandHistory.Push(command);
    }
    
    public void UndoLastCommand()
    {
        if (_commandHistory.Count > 0)
        {
            var lastCommand = _commandHistory.Pop();
            lastCommand.Undo();
        }
    }
}
```

#### Command com Macro
```csharp
public class MacroCommand : ICommand
{
    private readonly List<ICommand> _commands;
    
    public MacroCommand(List<ICommand> commands)
    {
        _commands = commands;
    }
    
    public void Execute()
    {
        foreach (var command in _commands)
        {
            command.Execute();
        }
    }
    
    public void Undo()
    {
        for (int i = _commands.Count - 1; i >= 0; i--)
        {
            _commands[i].Undo();
        }
    }
}
```

### Chain of Responsibility

#### Implementação Básica
```csharp
public abstract class Handler
{
    protected Handler _nextHandler;
    
    public void SetNext(Handler handler)
    {
        _nextHandler = handler;
    }
    
    public abstract void HandleRequest(int request);
}

public class ConcreteHandlerA : Handler
{
    public override void HandleRequest(int request)
    {
        if (request <= 10)
        {
            Console.WriteLine($"HandlerA handled request {request}");
        }
        else if (_nextHandler != null)
        {
            _nextHandler.HandleRequest(request);
        }
    }
}

public class ConcreteHandlerB : Handler
{
    public override void HandleRequest(int request)
    {
        if (request <= 20)
        {
            Console.WriteLine($"HandlerB handled request {request}");
        }
        else if (_nextHandler != null)
        {
            _nextHandler.HandleRequest(request);
        }
    }
}

public class ConcreteHandlerC : Handler
{
    public override void HandleRequest(int request)
    {
        Console.WriteLine($"HandlerC handled request {request}");
    }
}
```

#### Chain com Middleware
```csharp
public abstract class Middleware
{
    private Middleware _next;
    
    public Middleware LinkWith(Middleware next)
    {
        _next = next;
        return next;
    }
    
    public abstract string Check(string email, string password);
    
    protected string CheckNext(string email, string password)
    {
        if (_next == null)
        {
            return "OK";
        }
        
        return _next.Check(email, password);
    }
}

public class ThrottlingMiddleware : Middleware
{
    private int _requestPerMinute;
    private int _request;
    private long _currentTime;
    
    public ThrottlingMiddleware(int requestPerMinute)
    {
        _requestPerMinute = requestPerMinute;
        _currentTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
    }
    
    public override string Check(string email, string password)
    {
        long currentTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        
        if (currentTime > _currentTime)
        {
            _request = 0;
            _currentTime = currentTime;
        }
        
        _request++;
        
        if (_request > _requestPerMinute)
        {
            return "Request limit exceeded";
        }
        
        return CheckNext(email, password);
    }
}

public class UserExistsMiddleware : Middleware
{
    private readonly List<string> _users = new() { "admin@example.com", "user@example.com" };
    
    public override string Check(string email, string password)
    {
        if (!_users.Contains(email))
        {
            return "User not found";
        }
        
        return CheckNext(email, password);
    }
}
```

### Template Method Pattern

#### Implementação Básica
```csharp
public abstract class DataMiner
{
    public string Mine(string path)
    {
        string rawData = ExtractData(path);
        string data = ParseData(rawData);
        string analysis = AnalyzeData(data);
        string report = SendReport(analysis);
        return report;
    }
    
    protected abstract string ExtractData(string path);
    protected abstract string ParseData(string rawData);
    protected abstract string AnalyzeData(string data);
    protected abstract string SendReport(string analysis);
}

public class PDFDataMiner : DataMiner
{
    protected override string ExtractData(string path)
    {
        return "PDF data extracted";
    }
    
    protected override string ParseData(string rawData)
    {
        return "PDF data parsed";
    }
    
    protected override string AnalyzeData(string data)
    {
        return "PDF data analyzed";
    }
    
    protected override string SendReport(string analysis)
    {
        return "PDF report sent";
    }
}

public class CSVDataMiner : DataMiner
{
    protected override string ExtractData(string path)
    {
        return "CSV data extracted";
    }
    
    protected override string ParseData(string rawData)
    {
        return "CSV data parsed";
    }
    
    protected override string AnalyzeData(string data)
    {
        return "CSV data analyzed";
    }
    
    protected override string SendReport(string analysis)
    {
        return "CSV report sent";
    }
}
```

### State Pattern

#### Implementação Básica
```csharp
public interface IState
{
    void InsertQuarter();
    void EjectQuarter();
    void TurnCrank();
    void Dispense();
}

public class GumballMachine
{
    private IState _state;
    private int _count;
    
    public GumballMachine(int count)
    {
        _count = count;
        _state = _count > 0 ? new NoQuarterState(this) : new SoldOutState(this);
    }
    
    public void InsertQuarter()
    {
        _state.InsertQuarter();
    }
    
    public void EjectQuarter()
    {
        _state.EjectQuarter();
    }
    
    public void TurnCrank()
    {
        _state.TurnCrank();
        _state.Dispense();
    }
    
    public void SetState(IState state)
    {
        _state = state;
    }
    
    public void ReleaseBall()
    {
        Console.WriteLine("A gumball comes rolling out the slot...");
        if (_count > 0)
        {
            _count--;
        }
    }
    
    public int GetCount() => _count;
}

public class NoQuarterState : IState
{
    private readonly GumballMachine _machine;
    
    public NoQuarterState(GumballMachine machine)
    {
        _machine = machine;
    }
    
    public void InsertQuarter()
    {
        Console.WriteLine("You inserted a quarter");
        _machine.SetState(new HasQuarterState(_machine));
    }
    
    public void EjectQuarter()
    {
        Console.WriteLine("You haven't inserted a quarter");
    }
    
    public void TurnCrank()
    {
        Console.WriteLine("You turned, but there's no quarter");
    }
    
    public void Dispense()
    {
        Console.WriteLine("You need to pay first");
    }
}

public class HasQuarterState : IState
{
    private readonly GumballMachine _machine;
    
    public HasQuarterState(GumballMachine machine)
    {
        _machine = machine;
    }
    
    public void InsertQuarter()
    {
        Console.WriteLine("You can't insert another quarter");
    }
    
    public void EjectQuarter()
    {
        Console.WriteLine("Quarter returned");
        _machine.SetState(new NoQuarterState(_machine));
    }
    
    public void TurnCrank()
    {
        Console.WriteLine("You turned...");
        _machine.SetState(new SoldState(_machine));
    }
    
    public void Dispense()
    {
        Console.WriteLine("No gumball dispensed");
    }
}
```

### Memento Pattern

#### Implementação Básica
```csharp
public class Memento
{
    public string State { get; }
    
    public Memento(string state)
    {
        State = state;
    }
}

public class Originator
{
    private string _state;
    
    public string State
    {
        get => _state;
        set
        {
            _state = value;
            Console.WriteLine($"State = {_state}");
        }
    }
    
    public Memento CreateMemento()
    {
        return new Memento(_state);
    }
    
    public void SetMemento(Memento memento)
    {
        _state = memento.State;
        Console.WriteLine($"Restored state = {_state}");
    }
}

public class Caretaker
{
    private readonly List<Memento> _mementos = new();
    
    public void AddMemento(Memento memento)
    {
        _mementos.Add(memento);
    }
    
    public Memento GetMemento(int index)
    {
        return _mementos[index];
    }
}
```

### Interpreter Pattern

#### Implementação Básica
```csharp
public interface IExpression
{
    int Interpret(Dictionary<string, int> context);
}

public class NumberExpression : IExpression
{
    private readonly int _number;
    
    public NumberExpression(int number)
    {
        _number = number;
    }
    
    public int Interpret(Dictionary<string, int> context)
    {
        return _number;
    }
}

public class VariableExpression : IExpression
{
    private readonly string _variable;
    
    public VariableExpression(string variable)
    {
        _variable = variable;
    }
    
    public int Interpret(Dictionary<string, int> context)
    {
        return context[_variable];
    }
}

public class AddExpression : IExpression
{
    private readonly IExpression _left;
    private readonly IExpression _right;
    
    public AddExpression(IExpression left, IExpression right)
    {
        _left = left;
        _right = right;
    }
    
    public int Interpret(Dictionary<string, int> context)
    {
        return _left.Interpret(context) + _right.Interpret(context);
    }
}

public class SubtractExpression : IExpression
{
    private readonly IExpression _left;
    private readonly IExpression _right;
    
    public SubtractExpression(IExpression left, IExpression right)
    {
        _left = left;
        _right = right;
    }
    
    public int Interpret(Dictionary<string, int> context)
    {
        return _left.Interpret(context) - _right.Interpret(context);
    }
}
```

### Mediator Pattern

#### Implementação Básica
```csharp
public interface IMediator
{
    void Notify(object sender, string ev);
}

public class ConcreteMediator : IMediator
{
    private Component1 _component1;
    private Component2 _component2;
    
    public ConcreteMediator(Component1 component1, Component2 component2)
    {
        _component1 = component1;
        _component1.SetMediator(this);
        _component2 = component2;
        _component2.SetMediator(this);
    }
    
    public void Notify(object sender, string ev)
    {
        if (ev == "A")
        {
            Console.WriteLine("Mediator reacts on A and triggers following operations:");
            _component2.DoC();
        }
        if (ev == "D")
        {
            Console.WriteLine("Mediator reacts on D and triggers following operations:");
            _component1.DoB();
            _component2.DoC();
        }
    }
}

public abstract class BaseComponent
{
    protected IMediator _mediator;
    
    public void SetMediator(IMediator mediator)
    {
        _mediator = mediator;
    }
}

public class Component1 : BaseComponent
{
    public void DoA()
    {
        Console.WriteLine("Component 1 does A.");
        _mediator.Notify(this, "A");
    }
    
    public void DoB()
    {
        Console.WriteLine("Component 1 does B.");
        _mediator.Notify(this, "B");
    }
}

public class Component2 : BaseComponent
{
    public void DoC()
    {
        Console.WriteLine("Component 2 does C.");
        _mediator.Notify(this, "C");
    }
    
    public void DoD()
    {
        Console.WriteLine("Component 2 does D.");
        _mediator.Notify(this, "D");
    }
}
```

### Visitor Pattern

#### Implementação Básica
```csharp
public interface IVisitor
{
    void VisitConcreteElementA(ConcreteElementA element);
    void VisitConcreteElementB(ConcreteElementB element);
}

public interface IElement
{
    void Accept(IVisitor visitor);
}

public class ConcreteElementA : IElement
{
    public void Accept(IVisitor visitor)
    {
        visitor.VisitConcreteElementA(this);
    }
    
    public string ExclusiveMethodOfConcreteElementA()
    {
        return "A";
    }
}

public class ConcreteElementB : IElement
{
    public void Accept(IVisitor visitor)
    {
        visitor.VisitConcreteElementB(this);
    }
    
    public string SpecialMethodOfConcreteElementB()
    {
        return "B";
    }
}

public class ConcreteVisitor1 : IVisitor
{
    public void VisitConcreteElementA(ConcreteElementA element)
    {
        Console.WriteLine($"ConcreteVisitor1: {element.ExclusiveMethodOfConcreteElementA()}");
    }
    
    public void VisitConcreteElementB(ConcreteElementB element)
    {
        Console.WriteLine($"ConcreteVisitor1: {element.SpecialMethodOfConcreteElementB()}");
    }
}

public class ConcreteVisitor2 : IVisitor
{
    public void VisitConcreteElementA(ConcreteElementA element)
    {
        Console.WriteLine($"ConcreteVisitor2: {element.ExclusiveMethodOfConcreteElementA()}");
    }
    
    public void VisitConcreteElementB(ConcreteElementB element)
    {
        Console.WriteLine($"ConcreteVisitor2: {element.SpecialMethodOfConcreteElementB()}");
    }
}
```

### Flyweight Pattern

#### Implementação Básica
```csharp
public class Flyweight
{
    private readonly string _intrinsicState;
    
    public Flyweight(string intrinsicState)
    {
        _intrinsicState = intrinsicState;
    }
    
    public void Operation(string extrinsicState)
    {
        Console.WriteLine($"Flyweight: Intrinsic state = {_intrinsicState}, Extrinsic state = {extrinsicState}");
    }
}

public class FlyweightFactory
{
    private readonly Dictionary<string, Flyweight> _flyweights = new();
    
    public Flyweight GetFlyweight(string key)
    {
        if (!_flyweights.ContainsKey(key))
        {
            _flyweights[key] = new Flyweight(key);
        }
        
        return _flyweights[key];
    }
    
    public int GetFlyweightCount()
    {
        return _flyweights.Count;
    }
}
```

### Bridge Pattern

#### Implementação Básica
```csharp
public interface IImplementation
{
    string OperationImplementation();
}

public class ConcreteImplementationA : IImplementation
{
    public string OperationImplementation()
    {
        return "ConcreteImplementationA: The result in platform A.";
    }
}

public class ConcreteImplementationB : IImplementation
{
    public string OperationImplementation()
    {
        return "ConcreteImplementationB: The result in platform B.";
    }
}

public abstract class Abstraction
{
    protected IImplementation _implementation;
    
    public Abstraction(IImplementation implementation)
    {
        _implementation = implementation;
    }
    
    public virtual string Operation()
    {
        return $"Abstraction: Base operation with:\n{_implementation.OperationImplementation()}";
    }
}

public class ExtendedAbstraction : Abstraction
{
    public ExtendedAbstraction(IImplementation implementation) : base(implementation)
    {
    }
    
    public override string Operation()
    {
        return $"ExtendedAbstraction: Extended operation with:\n{_implementation.OperationImplementation()}";
    }
}
```

## Exercícios

### Exercício 1 - Command Pattern
Implemente um sistema de comandos para um editor de texto com undo/redo.

### Exercício 2 - State Pattern
Crie um sistema de estados para um jogo com diferentes níveis de dificuldade.

### Exercício 3 - Visitor Pattern
Implemente um sistema de visitantes para processar diferentes tipos de documentos.

## Dicas
- Use Command Pattern para operações que podem ser desfeitas
- Use State Pattern para objetos com comportamento que muda com o estado
- Use Visitor Pattern para operações que dependem do tipo de objeto
- Use Mediator Pattern para reduzir acoplamento entre componentes
- Use Flyweight Pattern para economizar memória com objetos similares
- Use Bridge Pattern para separar abstração da implementação
- Considere a complexidade antes de aplicar padrões
- Documente o uso de padrões no código 