# Aula 6 - Arrays e Coleções

## Objetivos da Aula
- Entender o conceito de arrays e coleções
- Aprender a criar e manipular arrays
- Conhecer diferentes tipos de coleções
- Praticar operações com arrays e listas

## Conteúdo Teórico

### Arrays

#### Declaração e Inicialização
```csharp
// Declaração
int[] numeros = new int[5];

// Inicialização com valores
int[] numeros = { 1, 2, 3, 4, 5 };

// Declaração e inicialização
string[] nomes = new string[] { "João", "Maria", "Pedro" };
```

#### Acesso aos Elementos
```csharp
int[] numeros = { 10, 20, 30, 40, 50 };
int primeiro = numeros[0];  // 10
int ultimo = numeros[4];    // 50
```

#### Propriedades dos Arrays
```csharp
int[] array = { 1, 2, 3, 4, 5 };
int tamanho = array.Length;  // 5
```

### Arrays Multidimensionais

#### Array 2D
```csharp
int[,] matriz = new int[3, 3];
int[,] matriz = { { 1, 2, 3 }, { 4, 5, 6 }, { 7, 8, 9 } };
```

#### Array Jagged (Denteado)
```csharp
int[][] jagged = new int[3][];
jagged[0] = new int[] { 1, 2, 3 };
jagged[1] = new int[] { 4, 5 };
jagged[2] = new int[] { 6, 7, 8, 9 };
```

### Coleções

#### List<T>
```csharp
List<int> numeros = new List<int>();
numeros.Add(10);
numeros.Add(20);
numeros.Add(30);

// Inicialização
List<string> nomes = new List<string> { "João", "Maria" };
```

#### Dictionary<TKey, TValue>
```csharp
Dictionary<string, int> idades = new Dictionary<string, int>();
idades.Add("João", 25);
idades.Add("Maria", 30);

// Acesso
int idadeJoao = idades["João"];
```

#### HashSet<T>
```csharp
HashSet<int> numerosUnicos = new HashSet<int>();
numerosUnicos.Add(1);
numerosUnicos.Add(2);
numerosUnicos.Add(1); // Não adiciona duplicatas
```

### Operações Comuns

#### Iteração
```csharp
// For tradicional
for (int i = 0; i < array.Length; i++)
{
    Console.WriteLine(array[i]);
}

// Foreach
foreach (int numero in array)
{
    Console.WriteLine(numero);
}
```

#### Busca
```csharp
int[] numeros = { 1, 2, 3, 4, 5 };
int indice = Array.IndexOf(numeros, 3); // 2
bool contem = numeros.Contains(3); // true
```

#### Ordenação
```csharp
int[] numeros = { 5, 2, 8, 1, 9 };
Array.Sort(numeros); // Ordena o array
Array.Reverse(numeros); // Inverte a ordem
```

### Métodos Úteis

#### Para Arrays
```csharp
int[] array = { 1, 2, 3, 4, 5 };
int soma = array.Sum();
double media = array.Average();
int maximo = array.Max();
int minimo = array.Min();
```

#### Para Listas
```csharp
List<int> lista = new List<int> { 1, 2, 3, 4, 5 };
lista.Remove(3); // Remove o primeiro 3
lista.RemoveAt(0); // Remove o elemento no índice 0
lista.Insert(1, 10); // Insere 10 no índice 1
```

## Exercícios

### Exercício 1 - Manipulação de Arrays
Crie um programa que demonstre diferentes operações com arrays.

### Exercício 2 - Sistema de Notas
Crie um programa que gerencie notas de alunos usando coleções.

### Exercício 3 - Agenda de Contatos
Crie um programa que simule uma agenda de contatos usando Dictionary.

## Dicas
- Arrays têm tamanho fixo, Listas são dinâmicas
- Use foreach para iterar sobre coleções
- Dictionary é útil para pares chave-valor
- HashSet é eficiente para elementos únicos
- Sempre verifique se o índice existe antes de acessar 