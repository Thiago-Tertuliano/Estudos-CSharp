# Aula 9 - Herança e Polimorfismo

## Objetivos da Aula
- Entender o conceito de herança
- Aprender a usar herança em C#
- Compreender polimorfismo
- Praticar hierarquias de classes

## Conteúdo Teórico

### Herança
Herança permite que uma classe herde características de outra classe, promovendo reutilização de código.

### Classe Base e Derivada
```csharp
// Classe base (pai)
public class Animal
{
    public string Nome { get; set; }
    public int Idade { get; set; }
    
    public virtual void EmitirSom()
    {
        Console.WriteLine("Som genérico");
    }
}

// Classe derivada (filha)
public class Cachorro : Animal
{
    public string Raca { get; set; }
    
    public override void EmitirSom()
    {
        Console.WriteLine("Au au!");
    }
    
    public void AbanarRabo()
    {
        Console.WriteLine("Abanando o rabo!");
    }
}
```

### Construtores na Herança
```csharp
public class Veiculo
{
    public string Marca { get; set; }
    public string Modelo { get; set; }
    
    public Veiculo(string marca, string modelo)
    {
        Marca = marca;
        Modelo = modelo;
    }
}

public class Carro : Veiculo
{
    public int NumeroPortas { get; set; }
    
    public Carro(string marca, string modelo, int numeroPortas) 
        : base(marca, modelo)
    {
        NumeroPortas = numeroPortas;
    }
}
```

### Polimorfismo
Polimorfismo permite que objetos de classes derivadas sejam tratados como objetos da classe base.

#### Polimorfismo de Sobrescrita
```csharp
public class Forma
{
    public virtual double CalcularArea()
    {
        return 0;
    }
}

public class Retangulo : Forma
{
    public double Largura { get; set; }
    public double Altura { get; set; }
    
    public override double CalcularArea()
    {
        return Largura * Altura;
    }
}

public class Circulo : Forma
{
    public double Raio { get; set; }
    
    public override double CalcularArea()
    {
        return Math.PI * Raio * Raio;
    }
}
```

### Classes Abstratas
```csharp
public abstract class Funcionario
{
    public string Nome { get; set; }
    public decimal SalarioBase { get; set; }
    
    public abstract decimal CalcularSalario();
    
    public virtual void Trabalhar()
    {
        Console.WriteLine("Trabalhando...");
    }
}

public class Gerente : Funcionario
{
    public decimal Bonus { get; set; }
    
    public override decimal CalcularSalario()
    {
        return SalarioBase + Bonus;
    }
    
    public override void Trabalhar()
    {
        Console.WriteLine("Gerenciando equipe...");
    }
}
```

### Interfaces
```csharp
public interface ICalculavel
{
    double CalcularArea();
    double CalcularPerimetro();
}

public class Quadrado : ICalculavel
{
    public double Lado { get; set; }
    
    public double CalcularArea()
    {
        return Lado * Lado;
    }
    
    public double CalcularPerimetro()
    {
        return 4 * Lado;
    }
}
```

### Sealed (Classes Finais)
```csharp
public sealed class StringUtils
{
    public static string Reverter(string texto)
    {
        return new string(texto.Reverse().ToArray());
    }
}
```

## Exercícios

### Exercício 1 - Hierarquia de Animais
Crie uma hierarquia de classes para diferentes tipos de animais.

### Exercício 2 - Sistema de Formas
Crie classes para diferentes formas geométricas com polimorfismo.

### Exercício 3 - Sistema de Funcionários
Crie um sistema de funcionários com herança e interfaces.

## Dicas
- Use herança para relacionamentos "é um"
- Prefira composição sobre herança quando possível
- Use virtual/override para polimorfismo
- Interfaces são úteis para contratos
- Classes abstratas não podem ser instanciadas 