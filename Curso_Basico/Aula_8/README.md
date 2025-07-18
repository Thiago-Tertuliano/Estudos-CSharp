# Aula 8 - Classes e Objetos

## Objetivos da Aula
- Entender Programação Orientada a Objetos (POO)
- Aprender a criar classes e objetos
- Compreender encapsulamento e construtores
- Praticar criação de classes

## Conteúdo Teórico

### O que é POO?
Programação Orientada a Objetos é um paradigma que organiza o código em objetos que contêm dados e comportamento.

### Classe vs Objeto
- **Classe**: Modelo/template que define estrutura e comportamento
- **Objeto**: Instância específica de uma classe

### Criando uma Classe
```csharp
public class Pessoa
{
    // Campos (atributos)
    private string nome;
    private int idade;
    
    // Propriedades
    public string Nome
    {
        get { return nome; }
        set { nome = value; }
    }
    
    public int Idade
    {
        get { return idade; }
        set { idade = value; }
    }
    
    // Construtor
    public Pessoa(string nome, int idade)
    {
        this.nome = nome;
        this.idade = idade;
    }
    
    // Métodos
    public void Apresentar()
    {
        Console.WriteLine($"Olá, sou {nome} e tenho {idade} anos.");
    }
}
```

### Propriedades Auto-implementadas
```csharp
public class Produto
{
    public string Nome { get; set; }
    public decimal Preco { get; set; }
    public int Quantidade { get; set; }
    
    public decimal ValorTotal => Preco * Quantidade;
}
```

### Construtores
```csharp
public class Carro
{
    public string Marca { get; set; }
    public string Modelo { get; set; }
    public int Ano { get; set; }
    
    // Construtor padrão
    public Carro()
    {
        Marca = "Desconhecida";
        Modelo = "Desconhecido";
        Ano = 2024;
    }
    
    // Construtor parametrizado
    public Carro(string marca, string modelo, int ano)
    {
        Marca = marca;
        Modelo = modelo;
        Ano = ano;
    }
    
    // Construtor de cópia
    public Carro(Carro outro)
    {
        Marca = outro.Marca;
        Modelo = outro.Modelo;
        Ano = outro.Ano;
    }
}
```

### Encapsulamento
```csharp
public class ContaBancaria
{
    private decimal saldo;
    
    public decimal Saldo
    {
        get { return saldo; }
        private set { saldo = value; }
    }
    
    public void Depositar(decimal valor)
    {
        if (valor > 0)
        {
            saldo += valor;
        }
    }
    
    public bool Sacar(decimal valor)
    {
        if (valor > 0 && valor <= saldo)
        {
            saldo -= valor;
            return true;
        }
        return false;
    }
}
```

### Métodos Estáticos
```csharp
public static class Utilitarios
{
    public static bool ValidarEmail(string email)
    {
        return email.Contains("@");
    }
    
    public static string FormatarCPF(string cpf)
    {
        return cpf.Replace(".", "").Replace("-", "");
    }
}
```

## Exercícios

### Exercício 1 - Classe Aluno
Crie uma classe Aluno com propriedades e métodos.

### Exercício 2 - Classe Produto
Crie uma classe Produto com encapsulamento.

### Exercício 3 - Classe Calculadora
Crie uma classe Calculadora com métodos estáticos.

## Dicas
- Use encapsulamento para proteger dados
- Construtores devem inicializar o objeto corretamente
- Propriedades auto-implementadas simplificam o código
- Métodos estáticos não precisam de instância
- Use nomes descritivos para classes e propriedades 