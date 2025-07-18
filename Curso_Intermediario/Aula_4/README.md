# Aula 4 - Reflection e Metaprogramação

## Objetivos da Aula
- Entender o conceito de Reflection
- Aprender a inspecionar tipos em tempo de execução
- Compreender metaprogramação
- Praticar criação dinâmica de objetos

## Conteúdo Teórico

### O que é Reflection?
Reflection permite inspecionar e manipular metadados de tipos, propriedades e métodos em tempo de execução. É útil para frameworks, serialização e desenvolvimento de ferramentas.

### Vantagens e Desvantagens
**Vantagens:**
- Flexibilidade em tempo de execução
- Desenvolvimento de frameworks
- Serialização dinâmica
- Plugins e extensões

**Desvantagens:**
- Performance inferior
- Código mais complexo
- Menos type safety

### Inspecionando Tipos

#### Obtendo Informações de Tipo
```csharp
Type tipo = typeof(string);

Console.WriteLine($"Nome: {tipo.Name}");
Console.WriteLine($"Namespace: {tipo.Namespace}");
Console.WriteLine($"Assembly: {tipo.Assembly.FullName}");
Console.WriteLine($"É classe: {tipo.IsClass}");
Console.WriteLine($"É interface: {tipo.IsInterface}");
```

#### Obtendo Propriedades
```csharp
public class Pessoa
{
    public string Nome { get; set; }
    public int Idade { get; set; }
    private string Senha { get; set; }
}

Type tipoPessoa = typeof(Pessoa);

// Todas as propriedades públicas
var propriedadesPublicas = tipoPessoa.GetProperties();

foreach (var prop in propriedadesPublicas)
{
    Console.WriteLine($"Propriedade: {prop.Name} - Tipo: {prop.PropertyType.Name}");
}
```

#### Obtendo Métodos
```csharp
var metodos = tipoPessoa.GetMethods();

foreach (var metodo in metodos)
{
    Console.WriteLine($"Método: {metodo.Name}");
    Console.WriteLine($"  Retorna: {metodo.ReturnType.Name}");
    Console.WriteLine($"  Parâmetros: {metodo.GetParameters().Length}");
}
```

### Criando Objetos Dinamicamente

#### Instanciação Dinâmica
```csharp
// Criar instância usando construtor padrão
Type tipo = typeof(string);
object instancia = Activator.CreateInstance(tipo);

// Criar instância com parâmetros
Type tipoLista = typeof(List<string>);
object lista = Activator.CreateInstance(tipoLista);
```

#### Usando Construtores Específicos
```csharp
public class Produto
{
    public string Nome { get; set; }
    public decimal Preco { get; set; }
    
    public Produto() { }
    
    public Produto(string nome, decimal preco)
    {
        Nome = nome;
        Preco = preco;
    }
}

// Encontrar construtor específico
Type tipoProduto = typeof(Produto);
ConstructorInfo construtor = tipoProduto.GetConstructor(new[] { typeof(string), typeof(decimal) });

// Criar instância
object produto = construtor.Invoke(new object[] { "Notebook", 3500.00m });
```

### Acessando Propriedades Dinamicamente

#### Definindo e Obtendo Valores
```csharp
var pessoa = new Pessoa { Nome = "João", Idade = 25 };
Type tipo = pessoa.GetType();

// Definir valor de propriedade
PropertyInfo propNome = tipo.GetProperty("Nome");
propNome.SetValue(pessoa, "Maria");

// Obter valor de propriedade
object valor = propNome.GetValue(pessoa);
Console.WriteLine($"Nome: {valor}");
```

#### Iterando sobre Propriedades
```csharp
public static void ExibirPropriedades(object obj)
{
    Type tipo = obj.GetType();
    
    foreach (var prop in tipo.GetProperties())
    {
        object valor = prop.GetValue(obj);
        Console.WriteLine($"{prop.Name}: {valor}");
    }
}
```

### Invocando Métodos Dinamicamente

#### Chamando Métodos
```csharp
public class Calculadora
{
    public int Soma(int a, int b) => a + b;
    public int Multiplica(int a, int b) => a * b;
}

var calculadora = new Calculadora();
Type tipo = calculadora.GetType();

// Encontrar método
MethodInfo metodoSoma = tipo.GetMethod("Soma");

// Invocar método
object resultado = metodoSoma.Invoke(calculadora, new object[] { 10, 20 });
Console.WriteLine($"Resultado: {resultado}");
```

#### Métodos com Parâmetros
```csharp
// Método com múltiplos parâmetros
MethodInfo metodo = tipo.GetMethod("MetodoComParametros");
ParameterInfo[] parametros = metodo.GetParameters();

foreach (var param in parametros)
{
    Console.WriteLine($"Parâmetro: {param.Name} - Tipo: {param.ParameterType.Name}");
}
```

### Atributos Customizados

#### Criando Atributos
```csharp
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
public class ValidacaoAttribute : Attribute
{
    public string Mensagem { get; set; }
    public bool Obrigatorio { get; set; }
    
    public ValidacaoAttribute(string mensagem = "", bool obrigatorio = false)
    {
        Mensagem = mensagem;
        Obrigatorio = obrigatorio;
    }
}

// Usando o atributo
[Validacao("Cliente deve ter dados válidos", true)]
public class Cliente
{
    [Validacao("Nome é obrigatório", true)]
    public string Nome { get; set; }
    
    [Validacao("Email deve ser válido")]
    public string Email { get; set; }
}
```

#### Lendo Atributos
```csharp
public static void ValidarObjeto(object obj)
{
    Type tipo = obj.GetType();
    
    // Verificar atributos da classe
    var atributosClasse = tipo.GetCustomAttributes(typeof(ValidacaoAttribute), true);
    foreach (ValidacaoAttribute attr in atributosClasse)
    {
        Console.WriteLine($"Validação da classe: {attr.Mensagem}");
    }
    
    // Verificar atributos das propriedades
    foreach (var prop in tipo.GetProperties())
    {
        var atributos = prop.GetCustomAttributes(typeof(ValidacaoAttribute), true);
        foreach (ValidacaoAttribute attr in atributos)
        {
            object valor = prop.GetValue(obj);
            if (attr.Obrigatorio && valor == null)
            {
                Console.WriteLine($"Erro: {prop.Name} é obrigatório");
            }
        }
    }
}
```

### Generics com Reflection

#### Criando Tipos Genéricos Dinamicamente
```csharp
// Criar List<string> dinamicamente
Type tipoLista = typeof(List<>);
Type tipoListaString = tipoLista.MakeGenericType(typeof(string));
object lista = Activator.CreateInstance(tipoListaString);

// Adicionar item
MethodInfo metodoAdd = tipoListaString.GetMethod("Add");
metodoAdd.Invoke(lista, new object[] { "Item 1" });
```

### Performance e Boas Práticas

#### Cache de Reflection
```csharp
public class CacheReflection
{
    private static readonly Dictionary<Type, PropertyInfo[]> _cachePropriedades = new();
    
    public static PropertyInfo[] ObterPropriedades(Type tipo)
    {
        if (!_cachePropriedades.ContainsKey(tipo))
        {
            _cachePropriedades[tipo] = tipo.GetProperties();
        }
        
        return _cachePropriedades[tipo];
    }
}
```

#### Usando Expressões para Performance
```csharp
public static class ReflectionHelper
{
    public static Action<T, object> CriarSetter<T>(string nomePropriedade)
    {
        var parametro = Expression.Parameter(typeof(T));
        var valor = Expression.Parameter(typeof(object));
        var propriedade = Expression.Property(parametro, nomePropriedade);
        var conversao = Expression.Convert(valor, propriedade.Type);
        var atribuicao = Expression.Assign(propriedade, conversao);
        var lambda = Expression.Lambda<Action<T, object>>(atribuicao, parametro, valor);
        
        return lambda.Compile();
    }
}
```

## Exercícios

### Exercício 1 - Inspeção de Tipos
Crie métodos para inspecionar tipos e suas propriedades.

### Exercício 2 - Criação Dinâmica
Implemente criação dinâmica de objetos e invocação de métodos.

### Exercício 3 - Sistema de Validação
Crie um sistema de validação baseado em atributos customizados.

## Dicas
- Use Reflection apenas quando necessário
- Cache informações de Reflection para melhor performance
- Considere expressões para operações frequentes
- Use atributos customizados para metadados
- Teste bem o código que usa Reflection
- Documente claramente o comportamento dinâmico 