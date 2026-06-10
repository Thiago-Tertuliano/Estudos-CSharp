# Aula 6 - Extension Methods

## Objetivos da Aula
- Entender o conceito de Extension Methods
- Aprender a criar métodos de extensão
- Compreender quando usar extension methods
- Praticar extensão de tipos existentes

## Conteúdo Teórico

### O que são Extension Methods?
Extension Methods permitem adicionar métodos a tipos existentes sem modificar o código original, criando uma sintaxe mais fluente e legível.

### Vantagens dos Extension Methods
- **Extensibilidade**: Adicionar funcionalidade a tipos fechados
- **Legibilidade**: Sintaxe mais fluente
- **Reutilização**: Métodos utilitários organizados
- **Encadeamento**: Métodos podem ser encadeados

### Sintaxe Básica

#### Criando Extension Method
```csharp
public static class StringExtensions
{
    public static string Capitalizar(this string texto)
    {
        if (string.IsNullOrEmpty(texto))
            return texto;
            
        return char.ToUpper(texto[0]) + texto.Substring(1).ToLower();
    }
    
    public static bool EhPalindromo(this string texto)
    {
        if (string.IsNullOrEmpty(texto))
            return false;
            
        string limpo = new string(texto.Where(char.IsLetterOrDigit).ToArray()).ToLower();
        return limpo == new string(limpo.Reverse().ToArray());
    }
}

// Uso
string nome = "joão silva";
string capitalizado = nome.Capitalizar(); // "João silva"
bool ehPalindromo = "ana".EhPalindromo(); // true
```

### Extension Methods para Tipos Primitivos

#### Int Extensions
```csharp
public static class IntExtensions
{
    public static bool EhPar(this int numero)
    {
        return numero % 2 == 0;
    }
    
    public static bool EhPrimo(this int numero)
    {
        if (numero < 2) return false;
        if (numero == 2) return true;
        if (numero % 2 == 0) return false;
        
        for (int i = 3; i <= Math.Sqrt(numero); i += 2)
        {
            if (numero % i == 0) return false;
        }
        return true;
    }
    
    public static int ParaSegundos(this int minutos)
    {
        return minutos * 60;
    }
}

// Uso
int numero = 7;
bool ehPrimo = numero.EhPrimo(); // true
int segundos = 5.ParaSegundos(); // 300
```

#### DateTime Extensions
```csharp
public static class DateTimeExtensions
{
    public static bool EhFimDeSemana(this DateTime data)
    {
        return data.DayOfWeek == DayOfWeek.Saturday || 
               data.DayOfWeek == DayOfWeek.Sunday;
    }
    
    public static int Idade(this DateTime dataNascimento)
    {
        var hoje = DateTime.Today;
        int idade = hoje.Year - dataNascimento.Year;
        
        if (dataNascimento.Date > hoje.AddYears(-idade))
            idade--;
            
        return idade;
    }
    
    public static string ParaTextoRelativo(this DateTime data)
    {
        var diferenca = DateTime.Now - data;
        
        if (diferenca.TotalDays > 365)
            return $"há {(int)(diferenca.TotalDays / 365)} anos";
        else if (diferenca.TotalDays > 30)
            return $"há {(int)(diferenca.TotalDays / 30)} meses";
        else if (diferenca.TotalDays > 1)
            return $"há {(int)diferenca.TotalDays} dias";
        else if (diferenca.TotalHours > 1)
            return $"há {(int)diferenca.TotalHours} horas";
        else
            return "há alguns minutos";
    }
}
```

### Extension Methods para Collections

#### List Extensions
```csharp
public static class ListExtensions
{
    public static void AdicionarSeNaoExiste<T>(this List<T> lista, T item)
    {
        if (!lista.Contains(item))
            lista.Add(item);
    }
    
    public static List<T> Filtrar<T>(this List<T> lista, Func<T, bool> predicado)
    {
        return lista.Where(predicado).ToList();
    }
    
    public static void ParaCada<T>(this List<T> lista, Action<T> acao)
    {
        foreach (var item in lista)
        {
            acao(item);
        }
    }
}

// Uso
var numeros = new List<int> { 1, 2, 3, 4, 5 };
numeros.AdicionarSeNaoExiste(3); // Não adiciona, já existe
numeros.AdicionarSeNaoExiste(6); // Adiciona

var pares = numeros.Filtrar(n => n % 2 == 0);
numeros.ParaCada(n => Console.WriteLine(n));
```

#### IEnumerable Extensions
```csharp
public static class EnumerableExtensions
{
    public static string ParaString<T>(this IEnumerable<T> enumerable, string separador = ", ")
    {
        return string.Join(separador, enumerable);
    }
    
    public static bool ContemDuplicatas<T>(this IEnumerable<T> enumerable)
    {
        var hashSet = new HashSet<T>();
        foreach (var item in enumerable)
        {
            if (!hashSet.Add(item))
                return true;
        }
        return false;
    }
    
    public static IEnumerable<T> RemoverDuplicatas<T>(this IEnumerable<T> enumerable)
    {
        return enumerable.Distinct();
    }
}
```

### Extension Methods Encadeados

#### Fluent API
```csharp
public static class StringExtensions
{
    public static string RemoverEspacos(this string texto)
    {
        return texto?.Replace(" ", "") ?? "";
    }
    
    public static string Capitalizar(this string texto)
    {
        if (string.IsNullOrEmpty(texto))
            return texto;
            
        return char.ToUpper(texto[0]) + texto.Substring(1).ToLower();
    }
    
    public static string AdicionarSufixo(this string texto, string sufixo)
    {
        return texto + sufixo;
    }
}

// Uso encadeado
string resultado = "  joão silva  "
    .RemoverEspacos()
    .Capitalizar()
    .AdicionarSufixo("!");
// Resultado: "Joãosilva!"
```

### Extension Methods para Tipos Customizados

#### Extensions para Classes Próprias
```csharp
public class Pessoa
{
    public string Nome { get; set; }
    public int Idade { get; set; }
    public string Email { get; set; }
}

public static class PessoaExtensions
{
    public static bool EhMaiorDeIdade(this Pessoa pessoa)
    {
        return pessoa.Idade >= 18;
    }
    
    public static string NomeCompleto(this Pessoa pessoa)
    {
        return $"{pessoa.Nome} ({pessoa.Idade} anos)";
    }
    
    public static bool EmailValido(this Pessoa pessoa)
    {
        return !string.IsNullOrEmpty(pessoa.Email) && 
               pessoa.Email.Contains("@");
    }
}
```

### Extension Methods com Parâmetros

#### Métodos com Parâmetros Adicionais
```csharp
public static class StringExtensions
{
    public static string Repetir(this string texto, int vezes)
    {
        if (vezes <= 0) return "";
        return string.Concat(Enumerable.Repeat(texto, vezes));
    }
    
    public static string Truncar(this string texto, int maxLength, string sufixo = "...")
    {
        if (texto.Length <= maxLength)
            return texto;
            
        return texto.Substring(0, maxLength - sufixo.Length) + sufixo;
    }
    
    public static string SubstituirVogais(this string texto, char substituto)
    {
        return new string(texto.Select(c => "aeiouAEIOU".Contains(c) ? substituto : c).ToArray());
    }
}

// Uso
string resultado = "Olá".Repetir(3); // "OláOláOlá"
string truncado = "Texto muito longo".Truncar(10); // "Texto m..."
string semVogais = "Hello".SubstituirVogais('*'); // "H*ll*"
```

### Extension Methods para Nullable Types

#### Extensions para Nullable
```csharp
public static class NullableExtensions
{
    public static string ParaStringOuVazio<T>(this T? valor) where T : struct
    {
        return valor?.ToString() ?? "";
    }
    
    public static bool TemValorOuZero<T>(this T? valor) where T : struct
    {
        return valor.HasValue && !valor.Value.Equals(default(T));
    }
    
    public static T ValorOuPadrao<T>(this T? valor, T padrao) where T : struct
    {
        return valor ?? padrao;
    }
}

// Uso
int? numero = null;
string texto = numero.ParaStringOuVazio(); // ""
int valor = numero.ValorOuPadrao(0); // 0
```

### Extension Methods para LINQ

#### Extensions para LINQ
```csharp
public static class LinqExtensions
{
    public static IEnumerable<T> OndeNaoNulo<T>(this IEnumerable<T> enumerable) where T : class
    {
        return enumerable.Where(item => item != null);
    }
    
    public static IEnumerable<T> Paginar<T>(this IEnumerable<T> enumerable, int pagina, int tamanhoPagina)
    {
        return enumerable.Skip((pagina - 1) * tamanhoPagina).Take(tamanhoPagina);
    }
    
    public static T PrimeiroOuPadrao<T>(this IEnumerable<T> enumerable, T padrao)
    {
        return enumerable.FirstOrDefault(padrao);
    }
}
```

### Boas Práticas

#### Organização
```csharp
// Agrupe extensions por funcionalidade
public static class StringExtensions { }
public static class DateTimeExtensions { }
public static class CollectionExtensions { }
public static class LinqExtensions { }
```

#### Nomenclatura
```csharp
// Use nomes descritivos
public static string Capitalizar(this string texto) { }
public static bool EhPalindromo(this string texto) { }
public static int ParaSegundos(this int minutos) { }
```

#### Performance
```csharp
// Evite operações custosas em extensions
public static class PerformanceExtensions
{
    // Bom: operação simples
    public static bool EhPar(this int numero) => numero % 2 == 0;
    
    // Evite: operações complexas
    public static string ProcessamentoComplexo(this string texto)
    {
        // Operação muito custosa...
        return resultado;
    }
}
```

## Exercícios

### Exercício 1 - Extensions Básicas
Crie extension methods para tipos primitivos.

### Exercício 2 - Extensions para Collections
Implemente extension methods para List e IEnumerable.

### Exercício 3 - Fluent API
Crie uma API fluente usando extension methods encadeados.

## Dicas
- Use extension methods para melhorar legibilidade
- Agrupe extensions por funcionalidade
- Mantenha extensions simples e focadas
- Use nomes descritivos
- Evite operações custosas em extensions
- Considere performance ao criar extensions
- Use extensions para criar APIs fluentes 