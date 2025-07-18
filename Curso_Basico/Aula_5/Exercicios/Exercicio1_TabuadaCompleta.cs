using System;

namespace Aula5.Exercicios
{
    class Exercicio1_TabuadaCompleta
    {
        static void Main(string[] args)
        {
            // Exercício 1: Tabuada Completa
            
            Console.WriteLine("=== Tabuada Completa ===");
            
            int numero = 7;
            Console.WriteLine($"Gerando tabuada do {numero}:");
            Console.WriteLine();
            
            // Usando for para gerar a tabuada
            for (int i = 1; i <= 10; i++)
            {
                int resultado = numero * i;
                Console.WriteLine($"{numero} x {i} = {resultado}");
            }
            
            Console.WriteLine("\n=== Tabuadas de 1 a 10 ===");
            
            // Loops aninhados para gerar todas as tabuadas
            for (int tabuada = 1; tabuada <= 10; tabuada++)
            {
                Console.WriteLine($"\n--- Tabuada do {tabuada} ---");
                
                for (int multiplicador = 1; multiplicador <= 10; multiplicador++)
                {
                    int resultado = tabuada * multiplicador;
                    Console.WriteLine($"{tabuada} x {multiplicador} = {resultado}");
                }
            }
            
            Console.WriteLine("\n=== Tabuada com while ===");
            
            // Usando while para uma tabuada específica
            int numeroWhile = 9;
            int contador = 1;
            
            Console.WriteLine($"Tabuada do {numeroWhile}:");
            while (contador <= 10)
            {
                int resultado = numeroWhile * contador;
                Console.WriteLine($"{numeroWhile} x {contador} = {resultado}");
                contador++;
            }
            
            Console.WriteLine("\n=== Tabuada com do-while ===");
            
            // Usando do-while
            int numeroDoWhile = 6;
            int contadorDoWhile = 1;
            
            Console.WriteLine($"Tabuada do {numeroDoWhile}:");
            do
            {
                int resultado = numeroDoWhile * contadorDoWhile;
                Console.WriteLine($"{numeroDoWhile} x {contadorDoWhile} = {resultado}");
                contadorDoWhile++;
            } while (contadorDoWhile <= 10);
            
            Console.WriteLine("\n=== Tabuada com foreach ===");
            
            // Usando foreach com array de multiplicadores
            int numeroForeach = 8;
            int[] multiplicadores = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            
            Console.WriteLine($"Tabuada do {numeroForeach}:");
            foreach (int multiplicador in multiplicadores)
            {
                int resultado = numeroForeach * multiplicador;
                Console.WriteLine($"{numeroForeach} x {multiplicador} = {resultado}");
            }
            
            Console.WriteLine("\n=== Tabuada com break e continue ===");
            
            // Demonstração de break e continue
            int numeroEspecial = 5;
            Console.WriteLine($"Tabuada do {numeroEspecial} (pulando múltiplos de 3):");
            
            for (int i = 1; i <= 10; i++)
            {
                if (i % 3 == 0)
                {
                    continue; // Pula múltiplos de 3
                }
                
                if (i > 8)
                {
                    break; // Para no 8
                }
                
                int resultado = numeroEspecial * i;
                Console.WriteLine($"{numeroEspecial} x {i} = {resultado}");
            }
            
            Console.WriteLine("\nPressione qualquer tecla para continuar...");
            Console.ReadKey();
        }
    }
} 