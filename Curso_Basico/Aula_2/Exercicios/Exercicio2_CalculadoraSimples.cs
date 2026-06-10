using System;

namespace Aula2.Exercicios
{
    class Exercicio2_CalculadoraSimples
    {
        static void Main(string[] args)
        {
            // Exercício 2: Calculadora Simples
            
            // Declarando variáveis para os números
            int numero1 = 15;
            int numero2 = 7;
            
            // Calculando a soma
            int soma = numero1 + numero2;
            
            // Exibindo o resultado
            Console.WriteLine("=== Calculadora Simples ===");
            Console.WriteLine($"Número 1: {numero1}");
            Console.WriteLine($"Número 2: {numero2}");
            Console.WriteLine($"Soma: {numero1} + {numero2} = {soma}");
            
            // Calculando outras operações
            int subtracao = numero1 - numero2;
            int multiplicacao = numero1 * numero2;
            double divisao = (double)numero1 / numero2; // Cast para double
            
            Console.WriteLine($"Subtração: {numero1} - {numero2} = {subtracao}");
            Console.WriteLine($"Multiplicação: {numero1} * {numero2} = {multiplicacao}");
            Console.WriteLine($"Divisão: {numero1} / {numero2} = {divisao:F2}");
            
            Console.WriteLine("\nPressione qualquer tecla para continuar...");
            Console.ReadKey();
        }
    }
} 