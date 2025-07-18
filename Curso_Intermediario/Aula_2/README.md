# Aula 2 - Programação Assíncrona (async/await)

## Objetivos da Aula
- Entender o conceito de programação assíncrona
- Aprender a usar async/await
- Compreender Task e Task<T>
- Praticar operações assíncronas

## Conteúdo Teórico

### O que é Programação Assíncrona?
Programação assíncrona permite que o programa execute operações que podem demorar (como I/O, rede, banco de dados) sem bloquear a thread principal, melhorando a responsividade da aplicação.

### Por que usar async/await?
- **Responsividade**: Interface não trava durante operações longas
- **Escalabilidade**: Melhor uso de recursos do sistema
- **Simplicidade**: Código mais legível que callbacks
- **Performance**: Operações I/O não bloqueiam threads

### Conceitos Básicos

#### Task
Representa uma operação assíncrona que pode ou não retornar um valor.

```csharp
// Task sem retorno
Task task = Task.Run(() => Console.WriteLine("Operação assíncrona"));

// Task com retorno
Task<int> taskComRetorno = Task.Run(() => 42);
```

#### async/await
- **async**: Marca um método como assíncrono
- **await**: Aguarda a conclusão de uma operação assíncrona

```csharp
public async Task<string> ObterDadosAsync()
{
    await Task.Delay(1000); // Simula operação longa
    return "Dados obtidos";
}
```

### Padrões de Uso

#### Método Assíncrono Básico
```csharp
public async Task<string> ObterNomeAsync()
{
    // Simula operação de I/O
    await Task.Delay(2000);
    return "João Silva";
}

// Chamada
string nome = await ObterNomeAsync();
```

#### Método com Retorno
```csharp
public async Task<int> CalcularAsync(int a, int b)
{
    await Task.Delay(1000); // Simula processamento
    return a + b;
}

// Chamada
int resultado = await CalcularAsync(10, 20);
```

#### Método void (não recomendado)
```csharp
public async void ProcessarAsync()
{
    await Task.Delay(1000);
    Console.WriteLine("Processado");
}
```

### Operações Assíncronas Comuns

#### Leitura de Arquivo
```csharp
public async Task<string> LerArquivoAsync(string caminho)
{
    using var reader = new StreamReader(caminho);
    return await reader.ReadToEndAsync();
}
```

#### Requisição HTTP
```csharp
public async Task<string> ObterDadosWebAsync(string url)
{
    using var client = new HttpClient();
    return await client.GetStringAsync(url);
}
```

#### Múltiplas Operações

#### Execução Paralela
```csharp
public async Task<string[]> ObterMultiplosDadosAsync()
{
    var task1 = ObterDadosAsync("url1");
    var task2 = ObterDadosAsync("url2");
    var task3 = ObterDadosAsync("url3");
    
    // Executa todas em paralelo
    var resultados = await Task.WhenAll(task1, task2, task3);
    return resultados;
}
```

#### Execução Sequencial
```csharp
public async Task<string[]> ObterDadosSequencialAsync()
{
    var resultado1 = await ObterDadosAsync("url1");
    var resultado2 = await ObterDadosAsync("url2");
    var resultado3 = await ObterDadosAsync("url3");
    
    return new[] { resultado1, resultado2, resultado3 };
}
```

### Tratamento de Exceções

#### Try-catch em Métodos Assíncronos
```csharp
public async Task<string> ObterDadosComTratamentoAsync()
{
    try
    {
        return await ObterDadosAsync();
    }
    catch (HttpRequestException ex)
    {
        Console.WriteLine($"Erro de rede: {ex.Message}");
        return "Dados padrão";
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Erro inesperado: {ex.Message}");
        throw;
    }
}
```

### Cancelamento de Operações

#### CancellationToken
```csharp
public async Task<string> ObterDadosComCancelamentoAsync(CancellationToken cancellationToken)
{
    await Task.Delay(5000, cancellationToken);
    return "Dados obtidos";
}

// Uso
var cts = new CancellationTokenSource();
var token = cts.Token;

var task = ObterDadosComCancelamentoAsync(token);
cts.CancelAfter(2000); // Cancela após 2 segundos

try
{
    var resultado = await task;
}
catch (OperationCanceledException)
{
    Console.WriteLine("Operação cancelada");
}
```

### Padrões Avançados

#### ConfigureAwait
```csharp
public async Task<string> ObterDadosConfigureAwaitAsync()
{
    var resultado = await ObterDadosAsync().ConfigureAwait(false);
    return resultado;
}
```

#### ValueTask (para operações simples)
```csharp
public async ValueTask<int> CalcularSimplesAsync(int a, int b)
{
    await Task.Delay(100);
    return a + b;
}
```

### Exemplos Práticos

#### Simulação de Operações de Banco
```csharp
public async Task<List<Usuario>> ObterUsuariosAsync()
{
    // Simula consulta ao banco
    await Task.Delay(1000);
    
    return new List<Usuario>
    {
        new Usuario { Id = 1, Nome = "João" },
        new Usuario { Id = 2, Nome = "Maria" }
    };
}
```

#### Processamento em Lote
```csharp
public async Task ProcessarLoteAsync(List<string> itens)
{
    var tasks = itens.Select(async item =>
    {
        await ProcessarItemAsync(item);
    });
    
    await Task.WhenAll(tasks);
}
```

## Exercícios

### Exercício 1 - Operações Assíncronas Básicas
Crie métodos assíncronos para simular operações de I/O.

### Exercício 2 - Múltiplas Operações
Implemente operações assíncronas paralelas e sequenciais.

### Exercício 3 - Tratamento de Exceções
Crie métodos assíncronos com tratamento adequado de exceções.

## Dicas
- Use async/await para operações I/O
- Evite async void (exceto para event handlers)
- Use ConfigureAwait(false) em bibliotecas
- Sempre trate exceções em operações assíncronas
- Use CancellationToken para cancelamento
- Prefira Task.WhenAll para operações paralelas 