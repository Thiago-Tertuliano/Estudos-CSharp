# Aula 5 - Delegates e Events

## Objetivos da Aula
- Entender o conceito de delegates
- Aprender a usar delegates e eventos
- Compreender padrões de callback
- Praticar programação orientada a eventos

## Conteúdo Teórico

### O que são Delegates?
Delegates são tipos que representam referências para métodos com uma assinatura específica. Permitem tratar métodos como dados, passando-os como parâmetros ou armazenando-os em variáveis.

### Vantagens dos Delegates
- **Flexibilidade**: Métodos podem ser passados como parâmetros
- **Callbacks**: Notificação de eventos
- **Extensibilidade**: Comportamento configurável
- **Desacoplamento**: Separação entre quem chama e quem executa

### Tipos de Delegates

#### Delegate Simples
```csharp
// Declaração do delegate
public delegate int OperacaoMatematica(int a, int b);

// Métodos que correspondem à assinatura
public static int Soma(int a, int b) => a + b;
public static int Multiplicacao(int a, int b) => a * b;

// Uso do delegate
OperacaoMatematica operacao = Soma;
int resultado = operacao(10, 20); // 30

operacao = Multiplicacao;
resultado = operacao(10, 20); // 200
```

#### Delegate como Parâmetro
```csharp
public static int ExecutarOperacao(int a, int b, OperacaoMatematica operacao)
{
    return operacao(a, b);
}

// Uso
int resultado = ExecutarOperacao(10, 20, Soma);
int resultado2 = ExecutarOperacao(10, 20, Multiplicacao);
```

### Delegates Built-in

#### Action
```csharp
// Action sem retorno
Action<string> imprimir = (mensagem) => Console.WriteLine(mensagem);
imprimir("Olá, mundo!");

// Action com múltiplos parâmetros
Action<string, int> imprimirRepetido = (mensagem, vezes) =>
{
    for (int i = 0; i < vezes; i++)
    {
        Console.WriteLine(mensagem);
    }
};
```

#### Func
```csharp
// Func com retorno
Func<int, int> quadrado = (x) => x * x;
int resultado = quadrado(5); // 25

// Func com múltiplos parâmetros
Func<int, int, int> soma = (a, b) => a + b;
int resultado2 = soma(10, 20); // 30
```

#### Predicate
```csharp
// Predicate (retorna bool)
Predicate<int> ehPar = (numero) => numero % 2 == 0;
bool resultado = ehPar(10); // true
```

### Multicast Delegates

#### Múltiplos Métodos
```csharp
public delegate void Notificacao(string mensagem);

public static void EnviarEmail(string mensagem)
{
    Console.WriteLine($"Email: {mensagem}");
}

public static void EnviarSMS(string mensagem)
{
    Console.WriteLine($"SMS: {mensagem}");
}

// Multicast delegate
Notificacao notificacao = EnviarEmail;
notificacao += EnviarSMS; // Adiciona mais um método

notificacao("Alerta importante!"); // Executa ambos os métodos
```

#### Removendo Métodos
```csharp
notificacao -= EnviarEmail; // Remove o método
notificacao("Teste"); // Executa apenas EnviarSMS
```

### Events

#### Declaração de Event
```csharp
public class Processador
{
    // Declaração do evento
    public event Action<string> ProcessamentoIniciado;
    public event Action<string> ProcessamentoConcluido;
    public event Action<string, Exception> ErroOcorrido;
    
    public void Processar(string dados)
    {
        try
        {
            // Dispara o evento de início
            ProcessamentoIniciado?.Invoke(dados);
            
            // Simula processamento
            Thread.Sleep(1000);
            
            // Dispara o evento de conclusão
            ProcessamentoConcluido?.Invoke("Processamento concluído");
        }
        catch (Exception ex)
        {
            // Dispara o evento de erro
            ErroOcorrido?.Invoke(dados, ex);
        }
    }
}
```

#### Assinando Eventos
```csharp
var processador = new Processador();

// Assinando eventos
processador.ProcessamentoIniciado += (dados) => 
    Console.WriteLine($"Iniciando processamento de: {dados}");

processador.ProcessamentoConcluido += (mensagem) => 
    Console.WriteLine($"Status: {mensagem}");

processador.ErroOcorrido += (dados, ex) => 
    Console.WriteLine($"Erro ao processar {dados}: {ex.Message}");

// Executando
processador.Processar("Dados de teste");
```

### Padrão Observer

#### Implementação Básica
```csharp
public interface IObserver
{
    void Atualizar(string mensagem);
}

public class Observador : IObserver
{
    public string Nome { get; set; }
    
    public Observador(string nome)
    {
        Nome = nome;
    }
    
    public void Atualizar(string mensagem)
    {
        Console.WriteLine($"{Nome} recebeu: {mensagem}");
    }
}

public class Sujeito
{
    private List<IObserver> observadores = new List<IObserver>();
    
    public void AdicionarObservador(IObserver observador)
    {
        observadores.Add(observador);
    }
    
    public void RemoverObservador(IObserver observador)
    {
        observadores.Remove(observador);
    }
    
    public void NotificarObservadores(string mensagem)
    {
        foreach (var observador in observadores)
        {
            observador.Atualizar(mensagem);
        }
    }
}
```

### EventHandler Padrão

#### Usando EventHandler
```csharp
public class DadosEventArgs : EventArgs
{
    public string Mensagem { get; set; }
    public DateTime Timestamp { get; set; }
}

public class ProcessadorAvancado
{
    public event EventHandler<DadosEventArgs> DadosProcessados;
    
    public void ProcessarDados(string dados)
    {
        // Processamento...
        
        // Dispara o evento
        DadosProcessados?.Invoke(this, new DadosEventArgs
        {
            Mensagem = dados,
            Timestamp = DateTime.Now
        });
    }
}
```

### Delegates com Lambda

#### Expressões Lambda
```csharp
// Lambda simples
Action<string> acao = (mensagem) => Console.WriteLine(mensagem);

// Lambda com múltiplas linhas
Action<string> acaoComplexa = (mensagem) =>
{
    Console.WriteLine($"Início: {mensagem}");
    Console.WriteLine($"Fim: {mensagem}");
};

// Lambda com retorno
Func<int, int> dobro = (numero) => numero * 2;
```

### Callbacks e Callbacks Assíncronos

#### Callback Síncrono
```csharp
public static void ProcessarComCallback(string dados, Action<string> callback)
{
    // Processamento...
    string resultado = $"Processado: {dados}";
    
    // Chama o callback
    callback(resultado);
}

// Uso
ProcessarComCallback("teste", (resultado) => Console.WriteLine(resultado));
```

#### Callback Assíncrono
```csharp
public static async Task ProcessarComCallbackAsync(string dados, Func<string, Task> callback)
{
    // Processamento assíncrono...
    await Task.Delay(1000);
    string resultado = $"Processado: {dados}";
    
    // Chama o callback assíncrono
    await callback(resultado);
}
```

### Boas Práticas

#### Nomenclatura de Eventos
```csharp
// Use verbos no passado para eventos
public event EventHandler ProcessamentoIniciado;
public event EventHandler ProcessamentoConcluido;
public event EventHandler<ErroEventArgs> ErroOcorrido;
```

#### Verificação de Null
```csharp
public void DispararEvento()
{
    // Sempre verifique se há assinantes
    MeuEvento?.Invoke(this, EventArgs.Empty);
}
```

#### Desinscrição de Eventos
```csharp
public class Cliente : IDisposable
{
    private Processador processador;
    
    public Cliente(Processador processador)
    {
        this.processador = processador;
        this.processador.ProcessamentoConcluido += OnProcessamentoConcluido;
    }
    
    private void OnProcessamentoConcluido(string mensagem)
    {
        Console.WriteLine($"Cliente recebeu: {mensagem}");
    }
    
    public void Dispose()
    {
        // Sempre desinscreva dos eventos
        processador.ProcessamentoConcluido -= OnProcessamentoConcluido;
    }
}
```

## Exercícios

### Exercício 1 - Delegates Básicos
Crie delegates para diferentes tipos de operações.

### Exercício 2 - Sistema de Eventos
Implemente um sistema de notificações usando eventos.

### Exercício 3 - Padrão Observer
Crie uma implementação do padrão Observer.

## Dicas
- Use Action para métodos sem retorno
- Use Func para métodos com retorno
- Use Predicate para métodos que retornam bool
- Sempre verifique null antes de disparar eventos
- Desinscreva de eventos para evitar memory leaks
- Use EventHandler para eventos padrão
- Prefira lambda expressions para callbacks simples 