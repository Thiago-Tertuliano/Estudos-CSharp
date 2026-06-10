# Aula 2 - Variáveis e Tipos de Dados

## Objetivos da Aula
- Entender o conceito de variáveis
- Conhecer os tipos de dados básicos em C#
- Aprender a declarar e inicializar variáveis
- Compreender a conversão de tipos

## Conteúdo Teórico

### O que são Variáveis?
Variáveis são espaços na memória que armazenam dados que podem ser alterados durante a execução do programa. Cada variável tem um nome, um tipo e um valor.

### Tipos de Dados Básicos

#### Tipos Inteiros
```csharp
byte idade = 25;           // 0 a 255
short numero = 1000;       // -32.768 a 32.767
int quantidade = 1000000;  // -2.147.483.648 a 2.147.483.647
long populacao = 8000000000L; // Números muito grandes
```

#### Tipos de Ponto Flutuante
```csharp
float preco = 19.99f;      // Precisão simples
double altura = 1.75;      // Precisão dupla (padrão)
decimal salario = 5000.50m; // Alta precisão para dinheiro
```

#### Tipos de Texto
```csharp
char letra = 'A';          // Um caractere
string nome = "João";      // Texto
```

#### Tipos Lógicos
```csharp
bool ativo = true;         // true ou false
```

#### Tipos de Data
```csharp
DateTime hoje = DateTime.Now;
DateOnly dataNascimento = new DateOnly(1990, 5, 15);
TimeOnly horario = new TimeOnly(14, 30, 0);
```

### Declaração de Variáveis

#### Sintaxe Básica
```csharp
tipo nomeDaVariavel = valor;
```

#### Exemplos
```csharp
// Declaração com inicialização
int idade = 25;
string nome = "Maria";

// Declaração sem inicialização
int quantidade;
string produto;

// Inicialização posterior
quantidade = 10;
produto = "Notebook";
```

### Conversão de Tipos

#### Conversão Implícita (Automática)
```csharp
int numero = 10;
long numeroLongo = numero; // Conversão automática
```

#### Conversão Explícita (Cast)
```csharp
double preco = 19.99;
int precoInteiro = (int)preco; // Resultado: 19
```

#### Conversão com Métodos
```csharp
string numeroTexto = "123";
int numero = int.Parse(numeroTexto);
// ou
int numero2 = Convert.ToInt32(numeroTexto);
```

### Variáveis com var

O C# permite usar `var` para inferência de tipo:
```csharp
var idade = 25;        // C# infere que é int
var nome = "João";     // C# infere que é string
var ativo = true;      // C# infere que é bool
```

### Constantes

Variáveis que não podem ser alteradas:
```csharp
const double PI = 3.14159;
const string EMPRESA = "Minha Empresa";
```

## Exercícios

### Exercício 1 - Tipos Básicos
Crie variáveis de diferentes tipos e exiba seus valores.

### Exercício 2 - Calculadora Simples
Crie um programa que declare variáveis para dois números, calcule a soma e exiba o resultado.

### Exercício 3 - Informações do Produto
Crie variáveis para armazenar informações de um produto (nome, preço, quantidade) e exiba um relatório.

## Dicas
- Use nomes descritivos para variáveis
- Inicialize variáveis sempre que possível
- Use `const` para valores que não mudam
- Prefira `var` quando o tipo é óbvio
- Use tipos apropriados para cada situação 