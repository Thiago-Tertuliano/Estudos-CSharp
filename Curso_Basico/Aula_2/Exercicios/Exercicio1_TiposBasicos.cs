using System;

namespace Aula2.Exercicios
{
    class Exercicio1_TiposBasicos
    {
        static void Main(string[] args)
        {
            // Exercício 1: Tipos Básicos
            
            // Tipos inteiros
            byte idade = 25;
            short numero = 1000;
            int quantidade = 1000000;
            long populacao = 8000000000L;
            
            // Tipos de ponto flutuante
            float preco = 19.99f;
            double altura = 1.75;
            decimal salario = 5000.50m;
            
            // Tipos de texto
            char letra = 'A';
            string nome = "João Silva";
            
            // Tipo lógico
            bool ativo = true;
            
            // Tipo de data
            DateTime hoje = DateTime.Now;
            
            // Exibindo os valores
            Console.WriteLine("=== Demonstração de Tipos Básicos ===");
            Console.WriteLine($"Idade: {idade}");
            Console.WriteLine($"Número: {numero}");
            Console.WriteLine($"Quantidade: {quantidade}");
            Console.WriteLine($"População: {populacao}");
            Console.WriteLine($"Preço: {preco}");
            Console.WriteLine($"Altura: {altura}");
            Console.WriteLine($"Salário: {salario}");
            Console.WriteLine($"Letra: {letra}");
            Console.WriteLine($"Nome: {nome}");
            Console.WriteLine($"Ativo: {ativo}");
            Console.WriteLine($"Hoje: {hoje}");
            
            Console.WriteLine("\nPressione qualquer tecla para continuar...");
            Console.ReadKey();
        }
    }
} 