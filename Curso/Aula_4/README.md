# Aula 4 - Estruturas de Controle

## Objetivos da Aula
- Entender o conceito de estruturas de controle
- Aprender a usar if, else e else if
- Compreender o uso do switch
- Praticar tomada de decisões em programas

## Conteúdo Teórico

### Estruturas de Controle
As estruturas de controle permitem que o programa tome decisões e execute diferentes blocos de código baseado em condições.

### Estrutura if

#### Sintaxe Básica
```csharp
if (condicao)
{
    // código a ser executado se a condição for verdadeira
}
```

#### Exemplo
```csharp
int idade = 18;
if (idade >= 18)
{
    Console.WriteLine("Maior de idade");
}
```

### Estrutura if-else

#### Sintaxe
```csharp
if (condicao)
{
    // código se condição for verdadeira
}
else
{
    // código se condição for falsa
}
```

#### Exemplo
```csharp
int nota = 7;
if (nota >= 7)
{
    Console.WriteLine("Aprovado");
}
else
{
    Console.WriteLine("Reprovado");
}
```

### Estrutura if-else if-else

#### Sintaxe
```csharp
if (condicao1)
{
    // código se condicao1 for verdadeira
}
else if (condicao2)
{
    // código se condicao2 for verdadeira
}
else
{
    // código se nenhuma condição for verdadeira
}
```

#### Exemplo
```csharp
int nota = 8;
if (nota >= 9)
{
    Console.WriteLine("Excelente");
}
else if (nota >= 7)
{
    Console.WriteLine("Bom");
}
else if (nota >= 5)
{
    Console.WriteLine("Regular");
}
else
{
    Console.WriteLine("Insuficiente");
}
```

### Estrutura switch

#### Sintaxe
```csharp
switch (expressao)
{
    case valor1:
        // código para valor1
        break;
    case valor2:
        // código para valor2
        break;
    default:
        // código padrão
        break;
}
```

#### Exemplo
```csharp
int diaSemana = 3;
switch (diaSemana)
{
    case 1:
        Console.WriteLine("Segunda-feira");
        break;
    case 2:
        Console.WriteLine("Terça-feira");
        break;
    case 3:
        Console.WriteLine("Quarta-feira");
        break;
    default:
        Console.WriteLine("Outro dia");
        break;
}
```

### Switch Expression (C# 8.0+)

#### Sintaxe Moderna
```csharp
string resultado = expressao switch
{
    valor1 => "resultado1",
    valor2 => "resultado2",
    _ => "padrão"
};
```

#### Exemplo
```csharp
int nota = 8;
string conceito = nota switch
{
    10 => "Perfeito",
    9 => "Excelente",
    8 => "Muito Bom",
    7 => "Bom",
    >= 5 => "Regular",
    _ => "Insuficiente"
};
```

### Operador Ternário

#### Sintaxe
```csharp
resultado = condicao ? valorSeVerdadeiro : valorSeFalso;
```

#### Exemplo
```csharp
int idade = 20;
string status = idade >= 18 ? "Maior de idade" : "Menor de idade";
```

### Estruturas Aninhadas

#### Exemplo
```csharp
int idade = 25;
bool temHabilitacao = true;

if (idade >= 18)
{
    if (temHabilitacao)
    {
        Console.WriteLine("Pode dirigir");
    }
    else
    {
        Console.WriteLine("É maior de idade mas não tem habilitação");
    }
}
else
{
    Console.WriteLine("Menor de idade");
}
```

## Exercícios

### Exercício 1 - Verificador de Notas
Crie um programa que receba uma nota e classifique o desempenho do aluno.

### Exercício 2 - Calculadora de IMC
Crie um programa que calcule o IMC e classifique o resultado.

### Exercício 3 - Sistema de Menu
Crie um programa com menu usando switch para diferentes opções.

## Dicas
- Sempre use chaves `{}` mesmo para blocos simples
- Use `else if` para múltiplas condições exclusivas
- O `switch` é mais eficiente que múltiplos `if-else` para valores específicos
- Use o operador ternário para decisões simples
- Mantenha as condições simples e legíveis
- Evite estruturas muito aninhadas 