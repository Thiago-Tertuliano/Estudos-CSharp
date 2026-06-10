using System;

namespace Aula4.Exercicios
{
    class Exercicio1_VerificadorNotas
    {
        static void Main(string[] args)
        {
            // Exercício 1: Verificador de Notas
            
            Console.WriteLine("=== Verificador de Notas ===");
            
            // Simulando diferentes notas
            int[] notas = { 10, 8, 6, 4, 2 };
            
            foreach (int nota in notas)
            {
                Console.WriteLine($"\n--- Nota: {nota} ---");
                
                // Usando if-else if-else
                Console.WriteLine("Classificação (if-else):");
                if (nota >= 9)
                {
                    Console.WriteLine("  Excelente");
                }
                else if (nota >= 7)
                {
                    Console.WriteLine("  Bom");
                }
                else if (nota >= 5)
                {
                    Console.WriteLine("  Regular");
                }
                else
                {
                    Console.WriteLine("  Insuficiente");
                }
                
                // Usando switch
                Console.WriteLine("Classificação (switch):");
                switch (nota)
                {
                    case 10:
                        Console.WriteLine("  Perfeito");
                        break;
                    case 9:
                        Console.WriteLine("  Excelente");
                        break;
                    case 8:
                        Console.WriteLine("  Muito Bom");
                        break;
                    case 7:
                        Console.WriteLine("  Bom");
                        break;
                    case 6:
                        Console.WriteLine("  Regular");
                        break;
                    case 5:
                        Console.WriteLine("  Suficiente");
                        break;
                    default:
                        Console.WriteLine("  Insuficiente");
                        break;
                }
                
                // Usando switch expression (C# 8.0+)
                string conceito = nota switch
                {
                    10 => "Perfeito",
                    9 => "Excelente",
                    8 => "Muito Bom",
                    7 => "Bom",
                    6 => "Regular",
                    5 => "Suficiente",
                    _ => "Insuficiente"
                };
                Console.WriteLine($"Classificação (switch expression): {conceito}");
                
                // Usando operador ternário
                string status = nota >= 7 ? "Aprovado" : "Reprovado";
                Console.WriteLine($"Status: {status}");
            }
            
            // Verificação adicional com estruturas aninhadas
            Console.WriteLine("\n=== Verificação Detalhada ===");
            int notaExemplo = 8;
            bool frequenciaOk = true;
            
            if (notaExemplo >= 7)
            {
                if (frequenciaOk)
                {
                    Console.WriteLine("Aluno aprovado com frequência adequada");
                }
                else
                {
                    Console.WriteLine("Aluno aprovado mas com frequência baixa");
                }
            }
            else
            {
                if (notaExemplo >= 5)
                {
                    Console.WriteLine("Aluno em recuperação");
                }
                else
                {
                    Console.WriteLine("Aluno reprovado");
                }
            }
            
            Console.WriteLine("\nPressione qualquer tecla para continuar...");
            Console.ReadKey();
        }
    }
} 