using System;

namespace Aula5.Exercicios
{
    class Exercicio3_PadroesLoops
    {
        static void Main(string[] args)
        {
            // Exercício 3: Padrões com Loops
            
            Console.WriteLine("=== Padrões com Loops ===");
            
            // Padrão 1: Triângulo de asteriscos
            Console.WriteLine("\n--- Padrão 1: Triângulo ---");
            int altura = 5;
            
            for (int i = 1; i <= altura; i++)
            {
                // Espaços antes dos asteriscos
                for (int j = 1; j <= altura - i; j++)
                {
                    Console.Write(" ");
                }
                
                // Asteriscos
                for (int k = 1; k <= 2 * i - 1; k++)
                {
                    Console.Write("*");
                }
                
                Console.WriteLine();
            }
            
            // Padrão 2: Quadrado de números
            Console.WriteLine("\n--- Padrão 2: Quadrado de Números ---");
            int tamanho = 4;
            
            for (int i = 1; i <= tamanho; i++)
            {
                for (int j = 1; j <= tamanho; j++)
                {
                    Console.Write($"{i * j,3}");
                }
                Console.WriteLine();
            }
            
            // Padrão 3: Pirâmide de números
            Console.WriteLine("\n--- Padrão 3: Pirâmide de Números ---");
            int linhas = 4;
            
            for (int i = 1; i <= linhas; i++)
            {
                // Espaços
                for (int j = 1; j <= linhas - i; j++)
                {
                    Console.Write("  ");
                }
                
                // Números crescentes
                for (int k = 1; k <= i; k++)
                {
                    Console.Write($"{k} ");
                }
                
                // Números decrescentes
                for (int k = i - 1; k >= 1; k--)
                {
                    Console.Write($"{k} ");
                }
                
                Console.WriteLine();
            }
            
            // Padrão 4: Tabuleiro de xadrez
            Console.WriteLine("\n--- Padrão 4: Tabuleiro de Xadrez ---");
            int dimensao = 8;
            
            for (int i = 0; i < dimensao; i++)
            {
                for (int j = 0; j < dimensao; j++)
                {
                    if ((i + j) % 2 == 0)
                    {
                        Console.Write("██");
                    }
                    else
                    {
                        Console.Write("  ");
                    }
                }
                Console.WriteLine();
            }
            
            // Padrão 5: Losango
            Console.WriteLine("\n--- Padrão 5: Losango ---");
            int meio = 5;
            
            // Parte superior
            for (int i = 1; i <= meio; i++)
            {
                // Espaços
                for (int j = 1; j <= meio - i; j++)
                {
                    Console.Write(" ");
                }
                
                // Asteriscos
                for (int k = 1; k <= 2 * i - 1; k++)
                {
                    Console.Write("*");
                }
                
                Console.WriteLine();
            }
            
            // Parte inferior
            for (int i = meio - 1; i >= 1; i--)
            {
                // Espaços
                for (int j = 1; j <= meio - i; j++)
                {
                    Console.Write(" ");
                }
                
                // Asteriscos
                for (int k = 1; k <= 2 * i - 1; k++)
                {
                    Console.Write("*");
                }
                
                Console.WriteLine();
            }
            
            // Padrão 6: Espiral de números
            Console.WriteLine("\n--- Padrão 6: Espiral de Números ---");
            int n = 4;
            int numero = 1;
            
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    Console.Write($"{numero,3}");
                    numero++;
                }
                Console.WriteLine();
            }
            
            // Padrão 7: Triângulo de Pascal (simplificado)
            Console.WriteLine("\n--- Padrão 7: Triângulo de Pascal ---");
            int niveis = 6;
            
            for (int i = 0; i < niveis; i++)
            {
                // Espaços
                for (int j = 0; j < niveis - i - 1; j++)
                {
                    Console.Write(" ");
                }
                
                // Números (coeficientes binomiais simplificados)
                int coeficiente = 1;
                for (int j = 0; j <= i; j++)
                {
                    Console.Write($"{coeficiente} ");
                    coeficiente = coeficiente * (i - j) / (j + 1);
                }
                
                Console.WriteLine();
            }
            
            // Padrão 8: Padrão de setas
            Console.WriteLine("\n--- Padrão 8: Padrão de Setas ---");
            int largura = 7;
            
            for (int i = 0; i < largura; i++)
            {
                for (int j = 0; j < largura; j++)
                {
                    if (i == j || i == largura - 1 - j)
                    {
                        Console.Write("→");
                    }
                    else
                    {
                        Console.Write(" ");
                    }
                }
                Console.WriteLine();
            }
            
            Console.WriteLine("\nPressione qualquer tecla para continuar...");
            Console.ReadKey();
        }
    }
} 