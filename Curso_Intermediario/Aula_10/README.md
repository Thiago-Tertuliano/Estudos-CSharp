# Aula 10 - Unit Testing com xUnit

## Objetivos da Aula
- Entender o conceito de Unit Testing
- Aprender a usar xUnit framework
- Compreender TDD (Test-Driven Development)
- Praticar escrita de testes eficazes

## Conteúdo Teórico

### O que é Unit Testing?
Unit Testing é uma técnica de teste que verifica se unidades individuais de código (métodos, classes) funcionam corretamente de forma isolada.

### Vantagens dos Unit Tests
- **Confiança**: Garantia de que o código funciona
- **Refatoração**: Segurança para modificar código
- **Documentação**: Testes servem como documentação
- **Design**: Força melhor design de código
- **Debugging**: Facilita identificação de problemas

### Configurando xUnit

#### Instalação
```bash
# Criar projeto de testes
dotnet new xunit -n MeuProjeto.Tests

# Adicionar referência ao projeto principal
dotnet add reference ../MeuProjeto/MeuProjeto.csproj
```

#### Estrutura Básica
```csharp
using Xunit;

namespace MeuProjeto.Tests
{
    public class CalculadoraTests
    {
        [Fact]
        public void Soma_DoisNumeros_RetornaSoma()
        {
            // Arrange
            var calculadora = new Calculadora();
            int a = 2;
            int b = 3;
            
            // Act
            int resultado = calculadora.Soma(a, b);
            
            // Assert
            Assert.Equal(5, resultado);
        }
    }
}
```

### Tipos de Testes

#### Fact (Fato)
```csharp
[Fact]
public void Multiplicacao_DoisNumeros_RetornaProduto()
{
    // Arrange
    var calculadora = new Calculadora();
    
    // Act
    int resultado = calculadora.Multiplicacao(4, 5);
    
    // Assert
    Assert.Equal(20, resultado);
}
```

#### Theory (Teoria)
```csharp
[Theory]
[InlineData(2, 3, 5)]
[InlineData(0, 5, 5)]
[InlineData(-1, 1, 0)]
[InlineData(10, 20, 30)]
public void Soma_VariosParesDeNumeros_RetornaSomaCorreta(int a, int b, int esperado)
{
    // Arrange
    var calculadora = new Calculadora();
    
    // Act
    int resultado = calculadora.Soma(a, b);
    
    // Assert
    Assert.Equal(esperado, resultado);
}
```

### Assertions (Asserções)

#### Asserções Básicas
```csharp
[Fact]
public void AssertionsBasicas()
{
    // Verificar igualdade
    Assert.Equal(5, 5);
    Assert.NotEqual(5, 6);
    
    // Verificar booleanos
    Assert.True(true);
    Assert.False(false);
    
    // Verificar null
    string texto = null;
    Assert.Null(texto);
    
    texto = "teste";
    Assert.NotNull(texto);
    
    // Verificar strings
    Assert.Contains("teste", "este é um teste");
    Assert.DoesNotContain("falha", "sucesso");
    Assert.StartsWith("teste", "teste123");
    Assert.EndsWith("123", "teste123");
}
```

#### Asserções de Coleções
```csharp
[Fact]
public void AssertionsColecoes()
{
    var lista = new List<int> { 1, 2, 3, 4, 5 };
    
    // Verificar se contém item
    Assert.Contains(3, lista);
    Assert.DoesNotContain(10, lista);
    
    // Verificar tamanho
    Assert.Equal(5, lista.Count);
    
    // Verificar se está vazio
    var listaVazia = new List<int>();
    Assert.Empty(listaVazia);
    Assert.NotEmpty(lista);
    
    // Verificar ordem
    var ordenada = new List<int> { 1, 2, 3, 4, 5 };
    Assert.Equal(ordenada, lista);
}
```

#### Asserções de Exceções
```csharp
[Fact]
public void Dividir_PorZero_LancaExcecao()
{
    // Arrange
    var calculadora = new Calculadora();
    
    // Act & Assert
    var excecao = Assert.Throws<DivideByZeroException>(
        () => calculadora.Dividir(10, 0)
    );
    
    Assert.Equal("Divisão por zero não é permitida", excecao.Message);
}

[Fact]
public void Dividir_PorZero_LancaExcecaoComMensagem()
{
    // Arrange
    var calculadora = new Calculadora();
    
    // Act & Assert
    Assert.Throws<ArgumentException>(
        () => calculadora.Dividir(10, 0)
    );
}
```

### Testando Classes de Negócio

#### Testando Validações
```csharp
public class UsuarioValidator
{
    public bool Validar(Usuario usuario)
    {
        if (string.IsNullOrEmpty(usuario.Nome))
            return false;
        
        if (string.IsNullOrEmpty(usuario.Email))
            return false;
        
        if (!usuario.Email.Contains("@"))
            return false;
        
        if (usuario.Idade < 0 || usuario.Idade > 150)
            return false;
        
        return true;
    }
}

[Fact]
public void Validar_UsuarioValido_RetornaTrue()
{
    // Arrange
    var validator = new UsuarioValidator();
    var usuario = new Usuario
    {
        Nome = "João Silva",
        Email = "joao@email.com",
        Idade = 25
    };
    
    // Act
    bool resultado = validator.Validar(usuario);
    
    // Assert
    Assert.True(resultado);
}

[Theory]
[InlineData("", "joao@email.com", 25)] // Nome vazio
[InlineData("João", "", 25)] // Email vazio
[InlineData("João", "emailinvalido", 25)] // Email sem @
[InlineData("João", "joao@email.com", -1)] // Idade negativa
[InlineData("João", "joao@email.com", 200)] // Idade muito alta
public void Validar_UsuarioInvalido_RetornaFalse(string nome, string email, int idade)
{
    // Arrange
    var validator = new UsuarioValidator();
    var usuario = new Usuario { Nome = nome, Email = email, Idade = idade };
    
    // Act
    bool resultado = validator.Validar(usuario);
    
    // Assert
    Assert.False(resultado);
}
```

### Testando com Mocks

#### Usando Moq
```csharp
// Instalar Moq: dotnet add package Moq

public interface IEmailService
{
    void EnviarEmail(string destinatario, string assunto, string mensagem);
}

public class UsuarioService
{
    private readonly IEmailService _emailService;
    
    public UsuarioService(IEmailService emailService)
    {
        _emailService = emailService;
    }
    
    public void RegistrarUsuario(string nome, string email)
    {
        // Lógica de registro
        _emailService.EnviarEmail(email, "Bem-vindo!", $"Olá {nome}!");
    }
}

[Fact]
public void RegistrarUsuario_ChamaEmailService()
{
    // Arrange
    var mockEmailService = new Mock<IEmailService>();
    var usuarioService = new UsuarioService(mockEmailService.Object);
    
    // Act
    usuarioService.RegistrarUsuario("João", "joao@email.com");
    
    // Assert
    mockEmailService.Verify(
        x => x.EnviarEmail("joao@email.com", "Bem-vindo!", "Olá João!"),
        Times.Once
    );
}
```

### Testando Repositórios

#### Testando com Dados em Memória
```csharp
public interface IUsuarioRepository
{
    Usuario GetById(int id);
    IEnumerable<Usuario> GetAll();
    void Add(Usuario usuario);
    void Update(Usuario usuario);
    void Delete(int id);
}

public class UsuarioRepository : IUsuarioRepository
{
    private List<Usuario> _usuarios = new List<Usuario>();
    private int _nextId = 1;
    
    public Usuario GetById(int id)
    {
        return _usuarios.FirstOrDefault(u => u.Id == id);
    }
    
    public IEnumerable<Usuario> GetAll()
    {
        return _usuarios;
    }
    
    public void Add(Usuario usuario)
    {
        usuario.Id = _nextId++;
        _usuarios.Add(usuario);
    }
    
    public void Update(Usuario usuario)
    {
        var existing = GetById(usuario.Id);
        if (existing != null)
        {
            existing.Nome = usuario.Nome;
            existing.Email = usuario.Email;
        }
    }
    
    public void Delete(int id)
    {
        var usuario = GetById(id);
        if (usuario != null)
        {
            _usuarios.Remove(usuario);
        }
    }
}

[Fact]
public void GetById_UsuarioExistente_RetornaUsuario()
{
    // Arrange
    var repository = new UsuarioRepository();
    var usuario = new Usuario { Nome = "João", Email = "joao@email.com" };
    repository.Add(usuario);
    
    // Act
    var resultado = repository.GetById(1);
    
    // Assert
    Assert.NotNull(resultado);
    Assert.Equal("João", resultado.Nome);
    Assert.Equal("joao@email.com", resultado.Email);
}

[Fact]
public void GetById_UsuarioInexistente_RetornaNull()
{
    // Arrange
    var repository = new UsuarioRepository();
    
    // Act
    var resultado = repository.GetById(999);
    
    // Assert
    Assert.Null(resultado);
}
```

### Test-Driven Development (TDD)

#### Ciclo Red-Green-Refactor
```csharp
// 1. Red - Escrever teste que falha
[Fact]
public void CalcularDesconto_ClienteVIP_Retorna10PorCento()
{
    // Arrange
    var calculadora = new CalculadoraDesconto();
    
    // Act
    decimal desconto = calculadora.CalcularDesconto("VIP", 100m);
    
    // Assert
    Assert.Equal(10m, desconto);
}

// 2. Green - Implementar código que passa no teste
public class CalculadoraDesconto
{
    public decimal CalcularDesconto(string tipoCliente, decimal valor)
    {
        if (tipoCliente == "VIP")
            return valor * 0.1m;
        
        return 0m;
    }
}

// 3. Refactor - Melhorar o código mantendo os testes passando
public class CalculadoraDesconto
{
    private readonly Dictionary<string, decimal> _descontos = new()
    {
        ["VIP"] = 0.1m,
        ["Premium"] = 0.15m,
        ["Padrao"] = 0.05m
    };
    
    public decimal CalcularDesconto(string tipoCliente, decimal valor)
    {
        return _descontos.GetValueOrDefault(tipoCliente, 0m) * valor;
    }
}
```

### Organizando Testes

#### Test Classes
```csharp
public class CalculadoraTests
{
    private readonly Calculadora _calculadora;
    
    public CalculadoraTests()
    {
        _calculadora = new Calculadora();
    }
    
    [Fact]
    public void Soma_DoisNumeros_RetornaSoma()
    {
        // Arrange
        int a = 2, b = 3;
        
        // Act
        int resultado = _calculadora.Soma(a, b);
        
        // Assert
        Assert.Equal(5, resultado);
    }
    
    [Fact]
    public void Multiplicacao_DoisNumeros_RetornaProduto()
    {
        // Arrange
        int a = 4, b = 5;
        
        // Act
        int resultado = _calculadora.Multiplicacao(a, b);
        
        // Assert
        Assert.Equal(20, resultado);
    }
}
```

#### Test Categories
```csharp
[Trait("Category", "Matemática")]
[Fact]
public void Soma_DoisNumeros_RetornaSoma()
{
    // Test implementation
}

[Trait("Category", "Validação")]
[Fact]
public void Validar_UsuarioValido_RetornaTrue()
{
    // Test implementation
}
```

### Boas Práticas

#### Nomenclatura de Testes
```csharp
// Padrão: Metodo_Cenario_ResultadoEsperado
[Fact]
public void CalcularDesconto_ClienteVIP_Retorna10PorCento()
{
    // Implementation
}

[Fact]
public void Validar_EmailInvalido_RetornaFalse()
{
    // Implementation
}

[Fact]
public void Salvar_UsuarioNovo_AdicionaAoRepositorio()
{
    // Implementation
}
```

#### Arrange-Act-Assert
```csharp
[Fact]
public void ExemploAAA()
{
    // Arrange - Preparar dados e objetos
    var calculadora = new Calculadora();
    int a = 2, b = 3;
    
    // Act - Executar ação sendo testada
    int resultado = calculadora.Soma(a, b);
    
    // Assert - Verificar resultado
    Assert.Equal(5, resultado);
}
```

#### Testes Independentes
```csharp
public class UsuarioTests
{
    [Fact]
    public void CriarUsuario_ComDadosValidos_RetornaUsuario()
    {
        // Cada teste deve ser independente
        // Não deve depender de outros testes
        var usuario = new Usuario
        {
            Nome = "João",
            Email = "joao@email.com"
        };
        
        Assert.Equal("João", usuario.Nome);
        Assert.Equal("joao@email.com", usuario.Email);
    }
}
```

### Executando Testes

#### Comandos CLI
```bash
# Executar todos os testes
dotnet test

# Executar testes com detalhes
dotnet test --verbosity normal

# Executar testes específicos
dotnet test --filter "FullyQualifiedName~CalculadoraTests"

# Executar testes por categoria
dotnet test --filter "Category=Matemática"

# Executar testes com cobertura
dotnet test --collect:"XPlat Code Coverage"
```

#### Configuração do Projeto
```xml
<!-- MeuProjeto.Tests.csproj -->
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net6.0</TargetFramework>
    <IsPackable>false</IsPackable>
  </PropertyGroup>
  
  <ItemGroup>
    <PackageReference Include="Microsoft.NET.Test.Sdk" Version="17.1.0" />
    <PackageReference Include="xunit" Version="2.4.1" />
    <PackageReference Include="xunit.runner.visualstudio" Version="2.4.3" />
    <PackageReference Include="Moq" Version="4.18.2" />
  </ItemGroup>
  
  <ItemGroup>
    <ProjectReference Include="..\MeuProjeto\MeuProjeto.csproj" />
  </ItemGroup>
</Project>
```

## Exercícios

### Exercício 1 - Testes Básicos
Crie testes para uma calculadora simples.

### Exercício 2 - Testes com Mocks
Implemente testes usando mocks para serviços externos.

### Exercício 3 - TDD
Pratique TDD criando uma funcionalidade do zero.

## Dicas
- Escreva testes antes do código (TDD)
- Mantenha testes simples e focados
- Use nomes descritivos para testes
- Teste casos de borda e exceções
- Mantenha testes independentes
- Use mocks para dependências externas
- Execute testes frequentemente
- Mantenha boa cobertura de código 