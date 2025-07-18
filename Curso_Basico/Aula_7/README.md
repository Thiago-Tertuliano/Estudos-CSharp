# Aula 7 - Métodos e Funções

## Objetivos da Aula
- Entender o conceito de métodos e funções
- Aprender a criar e usar métodos
- Compreender parâmetros e retorno
- Praticar organização de código

## Conteúdo Teórico

### O que são Métodos?
Métodos são blocos de código que executam uma tarefa específica. Eles permitem reutilizar código e organizar a lógica do programa.

### Sintaxe Básica
```csharp
[modificador] tipo_retorno NomeMetodo([parametros])
{
    // código do método
    return valor; // se necessário
}
```

### Tipos de Métodos

#### Método sem Retorno (void)
```csharp
public void Saudacao()
{
    Console.WriteLine("Olá, mundo!");
}
```

#### Método com Retorno
```csharp
public int Soma(int a, int b)
{
    return a + b;
}
```

#### Método com Parâmetros
```csharp
public void SaudacaoPersonalizada(string nome)
{
    Console.WriteLine($"Olá, {nome}!");
}
```

### Sobrecarga de Métodos
```csharp
public int Soma(int a, int b)
{
    return a + b;
}

public double Soma(double a, double b)
{
    return a + b;
}

public int Soma(int a, int b, int c)
{
    return a + b + c;
}
```

### Parâmetros

#### Parâmetros por Valor
```csharp
public void ModificarValor(int numero)
{
    numero = 100; // Não afeta o valor original
}
```

#### Parâmetros por Referência (ref)
```csharp
public void ModificarReferencia(ref int numero)
{
    numero = 100; // Afeta o valor original
}
```

#### Parâmetros de Saída (out)
```csharp
public void Dividir(int dividendo, int divisor, out int quociente, out int resto)
{
    quociente = dividendo / divisor;
    resto = dividendo % divisor;
}
```

#### Parâmetros Opcionais
```csharp
public void Saudacao(string nome = "Usuário", string saudacao = "Olá")
{
    Console.WriteLine($"{saudacao}, {nome}!");
}
```

### Métodos Estáticos
```csharp
public static class Calculadora
{
    public static int Soma(int a, int b)
    {
        return a + b;
    }
    
    public static double Media(double[] numeros)
    {
        return numeros.Average();
    }
}
```

### Recursão
```csharp
public static int Fatorial(int n)
{
    if (n <= 1)
        return 1;
    return n * Fatorial(n - 1);
}
```

## Exercícios

### Exercício 1 - Calculadora de Métodos
Crie uma classe com diferentes métodos matemáticos.

### Exercício 2 - Validador de Dados
Crie métodos para validar diferentes tipos de dados.

### Exercício 3 - Utilitários de String
Crie métodos para manipular strings.

## Dicas
- Use nomes descritivos para métodos
- Um método deve ter uma responsabilidade específica
- Evite métodos muito longos
- Use parâmetros opcionais com moderação
- Documente métodos complexos 