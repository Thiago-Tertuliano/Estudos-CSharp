using System;
using System.Linq;

namespace Aula6.Exercicios
{
    class Exercicio1_ManipulacaoArrays
    {
        static void Main(string[] args)
        {
            // Exercício 1: Manipulação de Arrays
            
            Console.WriteLine("=== Manipulação de Arrays ===");
            
            // Array de números
            int[] numeros = { 5, 2, 8, 1, 9, 3, 7, 4, 6 };
            
            Console.WriteLine("Array original:");
            foreach (int numero in numeros)
            {
                Console.Write($"{numero} ");
            }
            Console.WriteLine();
            
            // Ordenação
            Array.Sort(numeros);
            Console.WriteLine("\nArray ordenado:");
            foreach (int numero in numeros)
            {
                Console.Write($"{numero} ");
            }
            Console.WriteLine();
            
            // Inversão
            Array.Reverse(numeros);
            Console.WriteLine("\nArray invertido:");
            foreach (int numero in numeros)
            {
                Console.Write($"{numero} ");
            }
            Console.WriteLine();
            
            // Estatísticas
            Console.WriteLine($"\nEstatísticas:");
            Console.WriteLine($"Soma: {numeros.Sum()}");
            Console.WriteLine($"Média: {numeros.Average():F2}");
            Console.WriteLine($"Máximo: {numeros.Max()}");
            Console.WriteLine($"Mínimo: {numeros.Min()}");
            
            // Busca
            int valorBusca = 7;
            int indice = Array.IndexOf(numeros, valorBusca);
            Console.WriteLine($"\nÍndice do valor {valorBusca}: {indice}");
            
            // Array 2D
            Console.WriteLine("\n=== Array 2D ===");
            int[,] matriz = { { 1, 2, 3 }, { 4, 5, 6 }, { 7, 8, 9 } };
            
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Console.Write($"{matriz[i, j]} ");
                }
                Console.WriteLine();
            }
            
            // Array jagged
            Console.WriteLine("\n=== Array Jagged ===");
            int[][] jagged = new int[3][];
            jagged[0] = new int[] { 1, 2, 3 };
            jagged[1] = new int[] { 4, 5 };
            jagged[2] = new int[] { 6, 7, 8, 9 };
            
            for (int i = 0; i < jagged.Length; i++)
            {
                for (int j = 0; j < jagged[i].Length; j++)
                {
                    Console.Write($"{jagged[i][j]} ");
                }
                Console.WriteLine();
            }
            
            Console.WriteLine("\nPressione qualquer tecla para continuar...");
            Console.ReadKey();
        }
    }
} 