# Aula 3 - Generics e Collections Avançadas

## Objetivos da Aula
- Entender o conceito de Generics
- Aprender a criar classes e métodos genéricos
- Compreender collections avançadas
- Praticar reutilização de código

## Conteúdo Teórico

### O que são Generics?
Generics permitem criar classes, interfaces e métodos que trabalham com tipos definidos em tempo de compilação, proporcionando type safety e reutilização de código.

### Vantagens dos Generics
- **Type Safety**: Erros de tipo detectados em tempo de compilação
- **Performance**: Evita boxing/unboxing
- **Reutilização**: Um código para múltiplos tipos
- **Legibilidade**: Código mais claro e expressivo

### Classes Genéricas

#### Classe Genérica Básica
```csharp
public class Caixa<T>
{
    private T item;
    
    public void Guardar(T item)
    {
        this.item = item;
    }
    
    public T Obter()
    {
        return item;
    }
}

// Uso
var caixaString = new Caixa<string>();
caixaString.Guardar("Olá");
string texto = caixaString.Obter();

var caixaInt = new Caixa<int>();
caixaInt.Guardar(42);
int numero = caixaInt.Obter();
```

#### Múltiplos Parâmetros de Tipo
```csharp
public class Par<TKey, TValue>
{
    public TKey Chave { get; set; }
    public TValue Valor { get; set; }
    
    public Par(TKey chave, TValue valor)
    {
        Chave = chave;
        Valor = valor;
    }
}

// Uso
var par = new Par<string, int>("idade", 25);
```

### Métodos Genéricos

#### Método Genérico em Classe Não-Genérica
```csharp
public class Utilitarios
{
    public static void Trocar<T>(ref T a, ref T b)
    {
        T temp = a;
        a = b;
        b = temp;
    }
    
    public static T Maximo<T>(T a, T b) where T : IComparable<T>
    {
        return a.CompareTo(b) > 0 ? a : b;
    }
}

// Uso
int x = 10, y = 20;
Utilitarios.Trocar(ref x, ref y);

string s1 = "abc", s2 = "def";
string maior = Utilitarios.Maximo(s1, s2);
```

### Constraints (Restrições)

#### Interface Constraint
```csharp
public class Repositorio<T> where T : IEntity
{
    private List<T> itens = new List<T>();
    
    public void Adicionar(T item)
    {
        itens.Add(item);
    }
    
    public T Buscar(int id)
    {
        return itens.FirstOrDefault(item => item.Id == id);
    }
}
```

#### Class Constraint
```csharp
public class Comparador<T> where T : class
{
    public bool SaoIguais(T a, T b)
    {
        return a == b;
    }
}
```

#### Struct Constraint
```csharp
public class Calculadora<T> where T : struct
{
    public T Soma(T a, T b)
    {
        // Implementação específica para tipos numéricos
        return (T)((dynamic)a + (dynamic)b);
    }
}
```

#### Constructor Constraint
```csharp
public class Fabrica<T> where T : new()
{
    public T Criar()
    {
        return new T();
    }
}
```

### Collections Avançadas

#### Dictionary<TKey, TValue>
```csharp
var dicionario = new Dictionary<string, int>
{
    ["João"] = 25,
    ["Maria"] = 30,
    ["Pedro"] = 35
};

// Adicionar
dicionario["Ana"] = 28;

// Verificar se existe
if (dicionario.ContainsKey("João"))
{
    int idade = dicionario["João"];
}

// Iterar
foreach (var kvp in dicionario)
{
    Console.WriteLine($"{kvp.Key}: {kvp.Value}");
}
```

#### HashSet<T>
```csharp
var conjunto = new HashSet<int> { 1, 2, 3, 4, 5 };

// Adicionar (ignora duplicatas)
conjunto.Add(3); // Não adiciona, já existe

// Operações de conjunto
var conjunto2 = new HashSet<int> { 4, 5, 6, 7 };
var uniao = conjunto.Union(conjunto2);
var intersecao = conjunto.Intersect(conjunto2);
var diferenca = conjunto.Except(conjunto2);
```

#### Queue<T> e Stack<T>
```csharp
// Queue (FIFO - First In, First Out)
var fila = new Queue<string>();
fila.Enqueue("Primeiro");
fila.Enqueue("Segundo");
fila.Enqueue("Terceiro");

string primeiro = fila.Dequeue(); // Remove e retorna o primeiro

// Stack (LIFO - Last In, First Out)
var pilha = new Stack<string>();
pilha.Push("Primeiro");
pilha.Push("Segundo");
pilha.Push("Terceiro");

string ultimo = pilha.Pop(); // Remove e retorna o último
```

#### LinkedList<T>
```csharp
var lista = new LinkedList<string>();
lista.AddLast("Primeiro");
lista.AddLast("Segundo");
lista.AddLast("Terceiro");

// Inserir no meio
var segundo = lista.Find("Segundo");
lista.AddAfter(segundo, "Meio");

// Iterar
foreach (var item in lista)
{
    Console.WriteLine(item);
}
```

### Interfaces Genéricas

#### IComparer<T>
```csharp
public class ComparadorPessoa : IComparer<Pessoa>
{
    public int Compare(Pessoa x, Pessoa y)
    {
        return x.Nome.CompareTo(y.Nome);
    }
}

// Uso
var pessoas = new List<Pessoa> { /* ... */ };
pessoas.Sort(new ComparadorPessoa());
```

#### IEqualityComparer<T>
```csharp
public class ComparadorProduto : IEqualityComparer<Produto>
{
    public bool Equals(Produto x, Produto y)
    {
        return x.Id == y.Id;
    }
    
    public int GetHashCode(Produto obj)
    {
        return obj.Id.GetHashCode();
    }
}
```

### Covariância e Contravariância

#### Covariância (out)
```csharp
public interface IProdutor<out T>
{
    T Produzir();
}

public class ProdutorString : IProdutor<string>
{
    public string Produzir() => "Texto";
}

// Uso
IProdutor<object> produtor = new ProdutorString(); // Covariância
```

#### Contravariância (in)
```csharp
public interface IConsumidor<in T>
{
    void Consumir(T item);
}

public class ConsumidorObject : IConsumidor<object>
{
    public void Consumir(object item) { }
}

// Uso
IConsumidor<string> consumidor = new ConsumidorObject(); // Contravariância
```

## Exercícios

### Exercício 1 - Classes Genéricas
Crie classes genéricas para diferentes tipos de dados.

### Exercício 2 - Collections Avançadas
Implemente operações com diferentes tipos de collections.

### Exercício 3 - Constraints e Interfaces
Crie classes genéricas com constraints apropriados.

## Dicas
- Use Generics para reutilização de código
- Aplique constraints quando necessário
- Prefira collections genéricas às não-genéricas
- Use HashSet para conjuntos únicos
- Use Dictionary para mapeamentos chave-valor
- Considere covariância/contravariância em interfaces 