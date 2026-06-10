using System;

namespace Aula3.Exercicios
{
    class Exercicio1_CalculadoraAvancada
    {
        static void Main(string[] args)
        {
            // Exercício 1: Calculadora Avançada
            
            int a = 15;
            int b = 4;
            
            Console.WriteLine("=== Calculadora Avançada ===");
            Console.WriteLine($"Valores: a = {a}, b = {b}");
            Console.WriteLine();
            
            // Operações aritméticas básicas
            Console.WriteLine("--- Operações Aritméticas ---");
            Console.WriteLine($"Soma: {a} + {b} = {a + b}");
            Console.WriteLine($"Subtração: {a} - {b} = {a - b}");
            Console.WriteLine($"Multiplicação: {a} * {b} = {a * b}");
            Console.WriteLine($"Divisão: {a} / {b} = {a / b}");
            Console.WriteLine($"Resto: {a} % {b} = {a % b}");
            
            // Operadores de incremento/decremento
            Console.WriteLine("\n--- Incremento/Decremento ---");
            int x = 5;
            Console.WriteLine($"Valor inicial: {x}");
            Console.WriteLine($"Pós-incremento (x++): {x++}");
            Console.WriteLine($"Após pós-incremento: {x}");
            Console.WriteLine($"Pré-incremento (++x): {++x}");
            Console.WriteLine($"Após pré-incremento: {x}");
            
            // Operadores de atribuição
            Console.WriteLine("\n--- Operadores de Atribuição ---");
            int valor = 10;
            Console.WriteLine($"Valor inicial: {valor}");
            valor += 5;
            Console.WriteLine($"Após += 5: {valor}");
            valor -= 3;
            Console.WriteLine($"Após -= 3: {valor}");
            valor *= 2;
            Console.WriteLine($"Após *= 2: {valor}");
            valor /= 4;
            Console.WriteLine($"Após /= 4: {valor}");
            
            // Operadores de comparação
            Console.WriteLine("\n--- Operadores de Comparação ---");
            Console.WriteLine($"{a} == {b}: {a == b}");
            Console.WriteLine($"{a} != {b}: {a != b}");
            Console.WriteLine($"{a} > {b}: {a > b}");
            Console.WriteLine($"{a} < {b}: {a < b}");
            Console.WriteLine($"{a} >= {b}: {a >= b}");
            Console.WriteLine($"{a} <= {b}: {a <= b}");
            
            // Operadores lógicos
            Console.WriteLine("\n--- Operadores Lógicos ---");
            bool cond1 = true;
            bool cond2 = false;
            Console.WriteLine($"cond1 = {cond1}, cond2 = {cond2}");
            Console.WriteLine($"cond1 && cond2: {cond1 && cond2}");
            Console.WriteLine($"cond1 || cond2: {cond1 || cond2}");
            Console.WriteLine($"!cond1: {!cond1}");
            
            // Operador ternário
            Console.WriteLine("\n--- Operador Ternário ---");
            int idade = 20;
            string status = idade >= 18 ? "Maior de idade" : "Menor de idade";
            Console.WriteLine($"Idade: {idade} - Status: {status}");
            
            Console.WriteLine("\nPressione qualquer tecla para continuar...");
            Console.ReadKey();
        }
    }
} 