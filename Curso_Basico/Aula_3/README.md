# Aula 3 - Operadores e Expressões

## Objetivos da Aula
- Conhecer os diferentes tipos de operadores em C#
- Aprender a usar operadores aritméticos
- Compreender operadores de comparação e lógicos
- Entender a precedência de operadores

## Conteúdo Teórico

### Operadores Aritméticos

#### Operadores Básicos
```csharp
int a = 10;
int b = 3;

int soma = a + b;        // 13
int subtracao = a - b;   // 7
int multiplicacao = a * b; // 30
int divisao = a / b;     // 3 (divisão inteira)
int resto = a % b;       // 1 (módulo/resto)
```

#### Operadores de Incremento/Decremento
```csharp
int x = 5;
x++; // x = x + 1 (pós-incremento)
++x; // x = x + 1 (pré-incremento)
x--; // x = x - 1 (pós-decremento)
--x; // x = x - 1 (pré-decremento)
```

#### Operadores de Atribuição
```csharp
int valor = 10;
valor += 5;  // valor = valor + 5
valor -= 3;  // valor = valor - 3
valor *= 2;  // valor = valor * 2
valor /= 4;  // valor = valor / 4
valor %= 3;  // valor = valor % 3
```

### Operadores de Comparação

```csharp
int a = 10;
int b = 5;

bool igual = a == b;        // false
bool diferente = a != b;    // true
bool maior = a > b;         // true
bool menor = a < b;         // false
bool maiorIgual = a >= b;   // true
bool menorIgual = a <= b;   // false
```

### Operadores Lógicos

#### Operadores AND (&&) e OR (||)
```csharp
bool condicao1 = true;
bool condicao2 = false;

bool resultado1 = condicao1 && condicao2; // false (AND)
bool resultado2 = condicao1 || condicao2; // true (OR)
bool resultado3 = !condicao1;             // false (NOT)
```

#### Operadores Bit a Bit
```csharp
int a = 5;  // 0101 em binário
int b = 3;  // 0011 em binário

int and = a & b;   // 0001 (1)
int or = a | b;    // 0111 (7)
int xor = a ^ b;   // 0110 (6)
int not = ~a;      // Complemento
```

### Operadores de Deslocamento
```csharp
int numero = 8; // 1000 em binário
int deslocamentoEsquerda = numero << 1;  // 16 (10000)
int deslocamentoDireita = numero >> 1;   // 4 (0100)
```

### Operador Ternário
```csharp
int idade = 18;
string status = idade >= 18 ? "Maior de idade" : "Menor de idade";
```

### Precedência de Operadores

A ordem de execução dos operadores é:
1. **Parênteses** `()`
2. **Incremento/Decremento** `++`, `--`
3. **Multiplicação/Divisão** `*`, `/`, `%`
4. **Adição/Subtração** `+`, `-`
5. **Comparação** `<`, `>`, `<=`, `>=`
6. **Igualdade** `==`, `!=`
7. **Lógicos** `&&`, `||`

### Exemplos de Expressões

```csharp
// Expressão matemática
double resultado = (10 + 5) * 2 / 3;

// Expressão com operadores lógicos
bool aprovado = nota >= 7 && frequencia >= 75;

// Expressão com operador ternário
string mensagem = saldo >= 0 ? "Saldo positivo" : "Saldo negativo";
```

## Exercícios

### Exercício 1 - Calculadora Avançada
Crie um programa que realize operações matemáticas básicas usando diferentes operadores.

### Exercício 2 - Verificação de Idade
Crie um programa que verifique se uma pessoa é maior de idade e pode dirigir.

### Exercício 3 - Calculadora de Desconto
Crie um programa que calcule desconto baseado na quantidade de produtos comprados.

## Dicas
- Use parênteses para controlar a ordem de execução
- O operador ternário é útil para decisões simples
- Cuidado com a divisão de inteiros (resultado é inteiro)
- Use operadores de atribuição composta para código mais limpo
- Preste atenção à precedência de operadores 