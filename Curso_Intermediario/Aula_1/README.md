# Aula 1 - LINQ (Language Integrated Query)

## Objetivos da Aula
- Entender o conceito de LINQ
- Aprender a usar LINQ to Objects
- Compreender operadores de consulta e método
- Praticar consultas complexas

## Conteúdo Teórico

### O que é LINQ?
LINQ (Language Integrated Query) é um conjunto de recursos que adiciona capacidades de consulta diretamente na linguagem C#. Permite consultar dados de diferentes fontes usando uma sintaxe consistente.

### Vantagens do LINQ
- **Sintaxe consistente** para diferentes fontes de dados
- **IntelliSense** e verificação de tipos em tempo de compilação
- **Performance otimizada** com lazy evaluation
- **Legibilidade** do código

### Sintaxe de Consulta vs Sintaxe de Método

#### Sintaxe de Consulta (Query Syntax)
```csharp
var resultado = from item in colecao
                where item.Condicao
                select item.Propriedade;
```

#### Sintaxe de Método (Method Syntax)
```csharp
var resultado = colecao.Where(item => item.Condicao)
                       .Select(item => item.Propriedade);
```

### Operadores LINQ Básicos

#### Where - Filtragem
```csharp
var numeros = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

// Números pares
var pares = numeros.Where(n => n % 2 == 0);

// Números maiores que 5
var maioresQue5 = numeros.Where(n => n > 5);
```

#### Select - Projeção
```csharp
var pessoas = new List<Pessoa>
{
    new Pessoa { Nome = "João", Idade = 25 },
    new Pessoa { Nome = "Maria", Idade = 30 },
    new Pessoa { Nome = "Pedro", Idade = 35 }
};

// Apenas os nomes
var nomes = pessoas.Select(p => p.Nome);

// Nomes em maiúsculo
var nomesMaiusculo = pessoas.Select(p => p.Nome.ToUpper());
```

#### OrderBy - Ordenação
```csharp
// Ordenar por idade
var ordenadosPorIdade = pessoas.OrderBy(p => p.Idade);

// Ordenar por idade decrescente
var ordenadosPorIdadeDesc = pessoas.OrderByDescending(p => p.Idade);

// Ordenação múltipla
var ordenacaoMultipla = pessoas.OrderBy(p => p.Idade)
                               .ThenBy(p => p.Nome);
```

#### First/FirstOrDefault - Primeiro Elemento
```csharp
// Primeiro elemento (lança exceção se não encontrar)
var primeiro = pessoas.First();

// Primeiro elemento ou null
var primeiroOuNull = pessoas.FirstOrDefault();

// Primeiro com condição
var primeiroMaior30 = pessoas.FirstOrDefault(p => p.Idade > 30);
```

#### Single/SingleOrDefault - Elemento Único
```csharp
// Elemento único (lança exceção se não for único)
var unico = pessoas.Single(p => p.Nome == "João");

// Elemento único ou null
var unicoOuNull = pessoas.SingleOrDefault(p => p.Nome == "João");
```

### Operadores de Agregação

#### Count - Contagem
```csharp
var totalPessoas = pessoas.Count();
var pessoasMaior30 = pessoas.Count(p => p.Idade > 30);
```

#### Sum - Soma
```csharp
var somaIdades = pessoas.Sum(p => p.Idade);
var somaNumeros = numeros.Sum();
```

#### Average - Média
```csharp
var mediaIdades = pessoas.Average(p => p.Idade);
var mediaNumeros = numeros.Average();
```

#### Min/Max - Mínimo/Máximo
```csharp
var idadeMinima = pessoas.Min(p => p.Idade);
var idadeMaxima = pessoas.Max(p => p.Idade);
var pessoaMaisVelha = pessoas.Max(p => p.Nome);
```

### Operadores de Agrupamento

#### GroupBy - Agrupamento
```csharp
// Agrupar por idade
var gruposPorIdade = pessoas.GroupBy(p => p.Idade);

foreach (var grupo in gruposPorIdade)
{
    Console.WriteLine($"Idade: {grupo.Key}");
    foreach (var pessoa in grupo)
    {
        Console.WriteLine($"  - {pessoa.Nome}");
    }
}
```

### Operadores de Junção

#### Join - Junção
```csharp
var produtos = new List<Produto>
{
    new Produto { Id = 1, Nome = "Notebook", CategoriaId = 1 },
    new Produto { Id = 2, Nome = "Mouse", CategoriaId = 2 }
};

var categorias = new List<Categoria>
{
    new Categoria { Id = 1, Nome = "Eletrônicos" },
    new Categoria { Id = 2, Nome = "Periféricos" }
};

var produtosComCategoria = produtos.Join(
    categorias,
    produto => produto.CategoriaId,
    categoria => categoria.Id,
    (produto, categoria) => new { produto.Nome, categoria.Nome }
);
```

### Operadores de Conjunto

#### Distinct - Elementos Únicos
```csharp
var numerosRepetidos = new List<int> { 1, 2, 2, 3, 3, 4 };
var numerosUnicos = numerosRepetidos.Distinct();
```

#### Union - União
```csharp
var lista1 = new List<int> { 1, 2, 3 };
var lista2 = new List<int> { 3, 4, 5 };
var uniao = lista1.Union(lista2); // 1, 2, 3, 4, 5
```

#### Intersect - Interseção
```csharp
var intersecao = lista1.Intersect(lista2); // 3
```

#### Except - Diferença
```csharp
var diferenca = lista1.Except(lista2); // 1, 2
```

### Execução Lazy vs Eager

#### Execução Lazy (Deferred)
```csharp
var query = pessoas.Where(p => p.Idade > 25); // Não executa ainda
var resultado = query.ToList(); // Executa aqui
```

#### Execução Eager (Immediate)
```csharp
var resultado = pessoas.Where(p => p.Idade > 25).ToList(); // Executa imediatamente
```

### LINQ com Tipos Anônimos
```csharp
var resultado = pessoas.Select(p => new 
{
    Nome = p.Nome,
    Idade = p.Idade,
    Status = p.Idade >= 18 ? "Maior de idade" : "Menor de idade"
});
```

## Exercícios

### Exercício 1 - Consultas Básicas
Crie consultas LINQ para filtrar e projetar dados de uma lista de produtos.

### Exercício 2 - Agregações e Agrupamentos
Implemente consultas que usem operadores de agregação e agrupamento.

### Exercício 3 - Consultas Complexas
Crie consultas LINQ complexas combinando múltiplos operadores.

## Dicas
- Use sintaxe de método para consultas simples
- Use sintaxe de consulta para consultas complexas
- Aproveite a execução lazy para otimizar performance
- Use tipos anônimos para projeções temporárias
- Sempre considere a legibilidade do código 