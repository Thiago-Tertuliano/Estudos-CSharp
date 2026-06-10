# Aula 5 - Loops

## Objetivos da Aula
- Entender o conceito de loops (laços de repetição)
- Aprender a usar for, while, do-while e foreach
- Compreender quando usar cada tipo de loop
- Praticar iterações e processamento de dados

## Conteúdo Teórico

### O que são Loops?
Loops são estruturas de controle que permitem executar um bloco de código múltiplas vezes. São essenciais para processar dados, iterar sobre coleções e automatizar tarefas repetitivas.

### Loop for

#### Sintaxe Básica
```csharp
for (inicializacao; condicao; incremento)
{
    // código a ser executado
}
```

#### Exemplo
```csharp
for (int i = 0; i < 5; i++)
{
    Console.WriteLine($"Número: {i}");
}
```

#### Variações do for
```csharp
// Contagem regressiva
for (int i = 10; i >= 0; i--)
{
    Console.WriteLine($"Contagem: {i}");
}

// Incremento personalizado
for (int i = 0; i < 100; i += 5)
{
    Console.WriteLine($"Múltiplo de 5: {i}");
}
```

### Loop while

#### Sintaxe
```csharp
while (condicao)
{
    // código a ser executado
}
```

#### Exemplo
```csharp
int contador = 0;
while (contador < 5)
{
    Console.WriteLine($"Contador: {contador}");
    contador++;
}
```

### Loop do-while

#### Sintaxe
```csharp
do
{
    // código a ser executado
} while (condicao);
```

#### Exemplo
```csharp
int numero;
do
{
    Console.WriteLine("Digite um número positivo: ");
    numero = int.Parse(Console.ReadLine());
} while (numero <= 0);
```

### Loop foreach

#### Sintaxe
```csharp
foreach (tipo elemento in colecao)
{
    // código a ser executado
}
```

#### Exemplo
```csharp
string[] frutas = { "Maçã", "Banana", "Laranja" };
foreach (string fruta in frutas)
{
    Console.WriteLine($"Fruta: {fruta}");
}
```

### Controle de Loops

#### break
Interrompe a execução do loop:
```csharp
for (int i = 0; i < 10; i++)
{
    if (i == 5)
        break; // Para no 5
    Console.WriteLine(i);
}
```

#### continue
Pula para a próxima iteração:
```csharp
for (int i = 0; i < 10; i++)
{
    if (i % 2 == 0)
        continue; // Pula números pares
    Console.WriteLine(i);
}
```

### Loops Aninhados

#### Exemplo
```csharp
for (int i = 1; i <= 3; i++)
{
    for (int j = 1; j <= 3; j++)
    {
        Console.WriteLine($"i={i}, j={j}");
    }
}
```

### Quando Usar Cada Loop

#### for
- Quando você sabe o número de iterações
- Para contagens e progressões
- Para acessar índices de arrays

#### while
- Quando não sabe o número de iterações
- Para loops baseados em condições
- Para processamento até uma condição ser atendida

#### do-while
- Quando precisa executar pelo menos uma vez
- Para validação de entrada
- Quando a condição é verificada no final

#### foreach
- Para iterar sobre coleções
- Quando não precisa do índice
- Para processar todos os elementos de uma lista

### Exemplos Práticos

#### Tabuada
```csharp
int numero = 7;
for (int i = 1; i <= 10; i++)
{
    Console.WriteLine($"{numero} x {i} = {numero * i}");
}
```

#### Soma de Números
```csharp
int soma = 0;
for (int i = 1; i <= 100; i++)
{
    soma += i;
}
Console.WriteLine($"Soma: {soma}");
```

#### Processamento de Array
```csharp
int[] numeros = { 1, 2, 3, 4, 5 };
int soma = 0;
foreach (int numero in numeros)
{
    soma += numero;
}
Console.WriteLine($"Média: {soma / numeros.Length}");
```

## Exercícios

### Exercício 1 - Tabuada Completa
Crie um programa que gere a tabuada de 1 a 10 para um número específico.

### Exercício 2 - Calculadora de Média
Crie um programa que calcule a média de um conjunto de notas usando loops.

### Exercício 3 - Padrões com Loops
Crie um programa que desenhe padrões usando loops aninhados.

## Dicas
- Use `for` quando souber o número de iterações
- Use `while` quando a condição pode mudar durante a execução
- Use `foreach` para coleções quando não precisar do índice
- Sempre verifique se o loop tem uma condição de parada
- Evite loops infinitos
- Use `break` e `continue` com moderação 