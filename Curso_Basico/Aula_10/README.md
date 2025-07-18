# Aula 10 - Tratamento de Exceções

## Objetivos da Aula
- Entender o que são exceções
- Aprender a usar try-catch-finally
- Compreender diferentes tipos de exceções
- Praticar tratamento de erros

## Conteúdo Teórico

### O que são Exceções?
Exceções são eventos que ocorrem durante a execução do programa e interrompem o fluxo normal. Elas representam erros ou condições excepcionais.

### Estrutura try-catch
```csharp
try
{
    // Código que pode gerar exceção
    int numero = int.Parse("abc");
}
catch (FormatException ex)
{
    // Tratamento específico para FormatException
    Console.WriteLine($"Erro de formato: {ex.Message}");
}
catch (Exception ex)
{
    // Tratamento genérico para outras exceções
    Console.WriteLine($"Erro: {ex.Message}");
}
```

### Bloco finally
```csharp
try
{
    // Código que pode gerar exceção
    File.Open("arquivo.txt", FileMode.Open);
}
catch (FileNotFoundException ex)
{
    Console.WriteLine($"Arquivo não encontrado: {ex.Message}");
}
finally
{
    // Sempre executado, com ou sem exceção
    Console.WriteLine("Operação finalizada");
}
```

### Tipos de Exceções Comuns

#### FormatException
```csharp
try
{
    int numero = int.Parse("abc");
}
catch (FormatException)
{
    Console.WriteLine("Formato inválido para conversão");
}
```

#### DivideByZeroException
```csharp
try
{
    int resultado = 10 / 0;
}
catch (DivideByZeroException)
{
    Console.WriteLine("Divisão por zero não é permitida");
}
```

#### IndexOutOfRangeException
```csharp
try
{
    int[] array = { 1, 2, 3 };
    int valor = array[5];
}
catch (IndexOutOfRangeException)
{
    Console.WriteLine("Índice fora dos limites do array");
}
```

#### FileNotFoundException
```csharp
try
{
    string conteudo = File.ReadAllText("arquivo_inexistente.txt");
}
catch (FileNotFoundException)
{
    Console.WriteLine("Arquivo não encontrado");
}
```

### Lançando Exceções
```csharp
public static int Dividir(int a, int b)
{
    if (b == 0)
    {
        throw new DivideByZeroException("Divisor não pode ser zero");
    }
    return a / b;
}
```

### Exceções Customizadas
```csharp
public class IdadeInvalidaException : Exception
{
    public IdadeInvalidaException() : base("Idade deve ser maior que zero")
    {
    }
    
    public IdadeInvalidaException(string message) : base(message)
    {
    }
}

public class Pessoa
{
    private int idade;
    
    public int Idade
    {
        get { return idade; }
        set
        {
            if (value < 0)
            {
                throw new IdadeInvalidaException();
            }
            idade = value;
        }
    }
}
```

### Usando using (Dispose Pattern)
```csharp
using (StreamReader reader = new StreamReader("arquivo.txt"))
{
    string conteudo = reader.ReadToEnd();
    Console.WriteLine(conteudo);
} // Dispose é chamado automaticamente
```

### Múltiplos catch
```csharp
try
{
    // Código que pode gerar diferentes exceções
    int numero = int.Parse(Console.ReadLine());
    int resultado = 100 / numero;
}
catch (FormatException)
{
    Console.WriteLine("Entrada inválida");
}
catch (DivideByZeroException)
{
    Console.WriteLine("Divisão por zero");
}
catch (Exception ex)
{
    Console.WriteLine($"Erro inesperado: {ex.Message}");
}
```

### Exceções em Métodos
```csharp
public static decimal CalcularMedia(int[] numeros)
{
    try
    {
        if (numeros == null || numeros.Length == 0)
        {
            throw new ArgumentException("Array não pode ser nulo ou vazio");
        }
        
        return numeros.Average();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Erro ao calcular média: {ex.Message}");
        throw; // Re-lança a exceção
    }
}
```

## Exercícios

### Exercício 1 - Validador de Dados
Crie métodos que validem dados e lancem exceções apropriadas.

### Exercício 2 - Calculadora Segura
Crie uma calculadora que trate exceções de forma adequada.

### Exercício 3 - Sistema de Arquivos
Crie um sistema que manipule arquivos com tratamento de exceções.

## Dicas
- Sempre trate exceções específicas antes das genéricas
- Use finally para limpeza de recursos
- Não ignore exceções silenciosamente
- Crie exceções customizadas quando necessário
- Use using para recursos que implementam IDisposable
- Documente as exceções que seus métodos podem lançar 