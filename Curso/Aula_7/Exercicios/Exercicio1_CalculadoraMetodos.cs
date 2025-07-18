using System;
using System.Linq; // Added for Average(), Max(), Min()

namespace Aula7.Exercicios
{
    class Exercicio1_CalculadoraMetodos
    {
        static void Main(string[] args)
        {
            // Exercício 1: Calculadora de Métodos
            
            Console.WriteLine("=== Calculadora com Métodos ===");
            
            int a = 15;
            int b = 7;
            double x = 10.5;
            double y = 3.2;
            
            // Testando métodos de soma
            Console.WriteLine($"Soma de {a} + {b} = {Soma(a, b)}");
            Console.WriteLine($"Soma de {x} + {y} = {Soma(x, y)}");
            Console.WriteLine($"Soma de {a} + {b} + 10 = {Soma(a, b, 10)}");
            
            // Testando outros métodos matemáticos
            Console.WriteLine($"\nSubtração: {a} - {b} = {Subtracao(a, b)}");
            Console.WriteLine($"Multiplicação: {a} * {b} = {Multiplicacao(a, b)}");
            Console.WriteLine($"Divisão: {a} / {b} = {Divisao(a, b):F2}");
            Console.WriteLine($"Potência: {a} ^ 2 = {Potencia(a, 2)}");
            Console.WriteLine($"Raiz quadrada de {a} = {RaizQuadrada(a):F2}");
            
            // Testando métodos com parâmetros por referência
            int resultado = 0;
            int resto = 0;
            DividirComResto(a, b, ref resultado, ref resto);
            Console.WriteLine($"\nDivisão com resto: {a} / {b} = {resultado} (resto {resto})");
            
            // Testando métodos com parâmetros de saída
            double media, maximo, minimo;
            CalcularEstatisticas(new double[] { 1, 2, 3, 4, 5 }, out media, out maximo, out minimo);
            Console.WriteLine($"\nEstatísticas: Média={media:F2}, Máximo={maximo}, Mínimo={minimo}");
            
            // Testando métodos estáticos
            Console.WriteLine($"\nFatorial de 5 = {Fatorial(5)}");
            Console.WriteLine($"Fibonacci(7) = {Fibonacci(7)}");
            
            // Testando métodos com parâmetros opcionais
            Saudacao();
            Saudacao("João");
            Saudacao("Maria", "Bom dia");
            
            Console.WriteLine("\nPressione qualquer tecla para continuar...");
            Console.ReadKey();
        }
        
        // Métodos de soma com sobrecarga
        public static int Soma(int a, int b)
        {
            return a + b;
        }
        
        public static double Soma(double a, double b)
        {
            return a + b;
        }
        
        public static int Soma(int a, int b, int c)
        {
            return a + b + c;
        }
        
        // Outros métodos matemáticos
        public static int Subtracao(int a, int b)
        {
            return a - b;
        }
        
        public static int Multiplicacao(int a, int b)
        {
            return a * b;
        }
        
        public static double Divisao(int a, int b)
        {
            if (b == 0)
            {
                throw new DivideByZeroException("Divisão por zero não é permitida");
            }
            return (double)a / b;
        }
        
        public static double Potencia(double base_, double expoente)
        {
            return Math.Pow(base_, expoente);
        }
        
        public static double RaizQuadrada(double numero)
        {
            if (numero < 0)
            {
                throw new ArgumentException("Não é possível calcular raiz quadrada de número negativo");
            }
            return Math.Sqrt(numero);
        }
        
        // Método com parâmetros por referência
        public static void DividirComResto(int dividendo, int divisor, ref int quociente, ref int resto)
        {
            if (divisor == 0)
            {
                throw new DivideByZeroException("Divisor não pode ser zero");
            }
            
            quociente = dividendo / divisor;
            resto = dividendo % divisor;
        }
        
        // Método com parâmetros de saída
        public static void CalcularEstatisticas(double[] numeros, out double media, out double maximo, out double minimo)
        {
            if (numeros == null || numeros.Length == 0)
            {
                throw new ArgumentException("Array não pode ser nulo ou vazio");
            }
            
            media = numeros.Average();
            maximo = numeros.Max();
            minimo = numeros.Min();
        }
        
        // Métodos estáticos
        public static int Fatorial(int n)
        {
            if (n < 0)
            {
                throw new ArgumentException("Fatorial não é definido para números negativos");
            }
            
            if (n <= 1)
            {
                return 1;
            }
            
            return n * Fatorial(n - 1);
        }
        
        public static int Fibonacci(int n)
        {
            if (n <= 0)
            {
                return 0;
            }
            else if (n == 1)
            {
                return 1;
            }
            else
            {
                return Fibonacci(n - 1) + Fibonacci(n - 2);
            }
        }
        
        // Método com parâmetros opcionais
        public static void Saudacao(string nome = "Usuário", string saudacao = "Olá")
        {
            Console.WriteLine($"{saudacao}, {nome}!");
        }
    }
} 