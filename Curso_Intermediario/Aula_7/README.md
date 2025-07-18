# Aula 7 - Attributes e Annotations

## Objetivos da Aula
- Entender o conceito de Attributes
- Aprender a criar atributos customizados
- Compreender metadados e anotações
- Praticar validação e configuração via attributes

## Conteúdo Teórico

### O que são Attributes?
Attributes são metadados que podem ser anexados a elementos do código (classes, métodos, propriedades) para fornecer informações adicionais que podem ser lidas em tempo de execução.

### Vantagens dos Attributes
- **Metadados**: Informações sobre o código
- **Validação**: Regras de validação declarativas
- **Configuração**: Comportamento configurável
- **Documentação**: Informações sobre uso e propósito

### Attributes Built-in

#### Obsolete
```csharp
[Obsolete("Use o novo método ProcessarV2")]
public void Processar(string dados)
{
    // Implementação antiga
}

[Obsolete("Este método será removido na próxima versão", true)]
public void MetodoDeprecado()
{
    // Gera erro de compilação se usado
}
```

#### Conditional
```csharp
[Conditional("DEBUG")]
public void LogDebug(string mensagem)
{
    Console.WriteLine($"[DEBUG] {mensagem}");
}

// Só é chamado em builds DEBUG
LogDebug("Teste"); // Compilado apenas em DEBUG
```

#### DllImport
```csharp
[DllImport("user32.dll")]
public static extern int MessageBox(IntPtr hWnd, string text, string caption, uint type);
```

### Criando Attributes Customizados

#### Attribute Básico
```csharp
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
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

// Uso
[Validacao("Cliente deve ser válido", true)]
public class Cliente
{
    [Validacao("Nome é obrigatório", true)]
    public string Nome { get; set; }
}
```

#### Attribute com Propriedades
```csharp
[AttributeUsage(AttributeTargets.Property)]
public class RangeAttribute : Attribute
{
    public double Minimo { get; set; }
    public double Maximo { get; set; }
    
    public RangeAttribute(double minimo, double maximo)
    {
        Minimo = minimo;
        Maximo = maximo;
    }
}

// Uso
public class Produto
{
    [Range(0, 1000)]
    public decimal Preco { get; set; }
    
    [Range(0, 100)]
    public int Quantidade { get; set; }
}
```

### Lendo Attributes

#### Reflection para Ler Attributes
```csharp
public static class Validador
{
    public static List<string> ValidarObjeto(object obj)
    {
        var erros = new List<string>();
        Type tipo = obj.GetType();
        
        // Verificar attributes da classe
        var atributosClasse = tipo.GetCustomAttributes(typeof(ValidacaoAttribute), true);
        foreach (ValidacaoAttribute attr in atributosClasse)
        {
            if (attr.Obrigatorio)
            {
                erros.Add($"Classe {tipo.Name}: {attr.Mensagem}");
            }
        }
        
        // Verificar attributes das propriedades
        foreach (var prop in tipo.GetProperties())
        {
            var atributos = prop.GetCustomAttributes(typeof(ValidacaoAttribute), true);
            foreach (ValidacaoAttribute attr in atributos)
            {
                object valor = prop.GetValue(obj);
                if (attr.Obrigatorio && valor == null)
                {
                    erros.Add($"{prop.Name}: {attr.Mensagem}");
                }
            }
        }
        
        return erros;
    }
}
```

### Sistema de Validação

#### Validadores Customizados
```csharp
[AttributeUsage(AttributeTargets.Property)]
public class EmailAttribute : Attribute
{
    public string Mensagem { get; set; }
    
    public EmailAttribute(string mensagem = "Email inválido")
    {
        Mensagem = mensagem;
    }
}

[AttributeUsage(AttributeTargets.Property)]
public class StringLengthAttribute : Attribute
{
    public int MinLength { get; set; }
    public int MaxLength { get; set; }
    public string Mensagem { get; set; }
    
    public StringLengthAttribute(int minLength, int maxLength, string mensagem = "")
    {
        MinLength = minLength;
        MaxLength = maxLength;
        Mensagem = mensagem;
    }
}

public class Usuario
{
    [StringLength(3, 50, "Nome deve ter entre 3 e 50 caracteres")]
    public string Nome { get; set; }
    
    [Email("Email deve ser válido")]
    public string Email { get; set; }
    
    [Range(18, 120)]
    public int Idade { get; set; }
}
```

#### Validador Completo
```csharp
public static class ValidadorCompleto
{
    public static List<string> Validar(object obj)
    {
        var erros = new List<string>();
        Type tipo = obj.GetType();
        
        foreach (var prop in tipo.GetProperties())
        {
            var valor = prop.GetValue(obj);
            
            // Validar StringLength
            var stringLengthAttr = prop.GetCustomAttribute<StringLengthAttribute>();
            if (stringLengthAttr != null && valor is string str)
            {
                if (str.Length < stringLengthAttr.MinLength || str.Length > stringLengthAttr.MaxLength)
                {
                    erros.Add($"{prop.Name}: {stringLengthAttr.Mensagem}");
                }
            }
            
            // Validar Email
            var emailAttr = prop.GetCustomAttribute<EmailAttribute>();
            if (emailAttr != null && valor is string email)
            {
                if (!email.Contains("@"))
                {
                    erros.Add($"{prop.Name}: {emailAttr.Mensagem}");
                }
            }
            
            // Validar Range
            var rangeAttr = prop.GetCustomAttribute<RangeAttribute>();
            if (rangeAttr != null && valor is IComparable comparable)
            {
                if (comparable.CompareTo(rangeAttr.Minimo) < 0 || 
                    comparable.CompareTo(rangeAttr.Maximo) > 0)
                {
                    erros.Add($"{prop.Name}: Valor deve estar entre {rangeAttr.Minimo} e {rangeAttr.Maximo}");
                }
            }
        }
        
        return erros;
    }
}
```

### Attributes para Serialização

#### JSON Attributes
```csharp
public class Produto
{
    [JsonPropertyName("nome_produto")]
    public string Nome { get; set; }
    
    [JsonPropertyName("preco")]
    public decimal Preco { get; set; }
    
    [JsonIgnore]
    public string InformacaoInterna { get; set; }
}
```

#### XML Attributes
```csharp
[XmlRoot("produto")]
public class Produto
{
    [XmlElement("nome")]
    public string Nome { get; set; }
    
    [XmlElement("preco")]
    public decimal Preco { get; set; }
    
    [XmlAttribute("id")]
    public int Id { get; set; }
}
```

### Attributes para Testes

#### Test Attributes
```csharp
[TestClass]
public class CalculadoraTests
{
    [TestMethod]
    [TestCategory("Matemática")]
    public void Soma_DoisNumeros_RetornaSoma()
    {
        // Arrange
        var calculadora = new Calculadora();
        
        // Act
        var resultado = calculadora.Soma(2, 3);
        
        // Assert
        Assert.AreEqual(5, resultado);
    }
    
    [TestMethod]
    [ExpectedException(typeof(DivideByZeroException))]
    public void Dividir_PorZero_LancaExcecao()
    {
        var calculadora = new Calculadora();
        calculadora.Dividir(10, 0);
    }
}
```

### Attributes para Documentação

#### Documentation Attributes
```csharp
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class DocumentacaoAttribute : Attribute
{
    public string Descricao { get; set; }
    public string Autor { get; set; }
    public string Versao { get; set; }
    
    public DocumentacaoAttribute(string descricao, string autor = "", string versao = "1.0")
    {
        Descricao = descricao;
        Autor = autor;
        Versao = versao;
    }
}

[Documentacao("Cliente do sistema", "João Silva", "2.0")]
public class Cliente
{
    [Documentacao("Nome completo do cliente")]
    public string Nome { get; set; }
}
```

### Attributes para Performance

#### Performance Attributes
```csharp
[AttributeUsage(AttributeTargets.Method)]
public class MedirTempoAttribute : Attribute
{
    public string Nome { get; set; }
    
    public MedirTempoAttribute(string nome = "")
    {
        Nome = nome;
    }
}

public static class PerformanceHelper
{
    public static T Medir<T>(Func<T> acao, string nome = "")
    {
        var stopwatch = Stopwatch.StartNew();
        var resultado = acao();
        stopwatch.Stop();
        
        Console.WriteLine($"{nome}: {stopwatch.ElapsedMilliseconds}ms");
        return resultado;
    }
}
```

### Attributes para Segurança

#### Security Attributes
```csharp
[AttributeUsage(AttributeTargets.Method)]
public class RequerPermissaoAttribute : Attribute
{
    public string Permissao { get; set; }
    
    public RequerPermissaoAttribute(string permissao)
    {
        Permissao = permissao;
    }
}

public class UsuarioService
{
    [RequerPermissao("admin")]
    public void DeletarUsuario(int id)
    {
        // Implementação
    }
    
    [RequerPermissao("user")]
    public void VisualizarPerfil(int id)
    {
        // Implementação
    }
}
```

### Boas Práticas

#### Nomenclatura
```csharp
// Use sufixo "Attribute" para nomes de classes
public class ValidacaoAttribute : Attribute { }

// Use nomes descritivos
[Validacao("Campo obrigatório")]
[Range(0, 100)]
[Email("Email inválido")]
```

#### Organização
```csharp
// Agrupe attributes por funcionalidade
public class ValidacaoAttribute : Attribute { }
public class DocumentacaoAttribute : Attribute { }
public class PerformanceAttribute : Attribute { }
```

#### Performance
```csharp
// Cache attributes para melhor performance
public static class AttributeCache
{
    private static readonly Dictionary<Type, Attribute[]> _cache = new();
    
    public static T[] GetCustomAttributes<T>(Type tipo) where T : Attribute
    {
        if (!_cache.ContainsKey(tipo))
        {
            _cache[tipo] = tipo.GetCustomAttributes(true).Cast<Attribute>().ToArray();
        }
        
        return _cache[tipo].OfType<T>().ToArray();
    }
}
```

## Exercícios

### Exercício 1 - Attributes Básicos
Crie attributes customizados para validação.

### Exercício 2 - Sistema de Validação
Implemente um sistema completo de validação usando attributes.

### Exercício 3 - Attributes para Documentação
Crie attributes para documentação automática.

## Dicas
- Use attributes para metadados declarativos
- Agrupe attributes por funcionalidade
- Cache attributes para melhor performance
- Use nomes descritivos para attributes
- Documente o propósito dos attributes
- Considere reutilização de attributes
- Teste attributes em diferentes cenários 