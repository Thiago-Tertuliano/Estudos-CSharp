using System;

namespace Aula5.Exercicios
{
    class Exercicio2_CalculadoraMedia
    {
        static void Main(string[] args)
        {
            // Exercício 2: Calculadora de Média
            
            Console.WriteLine("=== Calculadora de Média ===");
            
            // Array de notas
            double[] notas = { 8.5, 7.0, 9.2, 6.8, 8.0, 7.5, 9.0, 8.8, 7.2, 8.9 };
            
            Console.WriteLine("Notas do aluno:");
            foreach (double nota in notas)
            {
                Console.WriteLine($"  {nota:F1}");
            }
            Console.WriteLine();
            
            // Calculando média usando for
            double somaFor = 0;
            for (int i = 0; i < notas.Length; i++)
            {
                somaFor += notas[i];
            }
            double mediaFor = somaFor / notas.Length;
            Console.WriteLine($"Média (usando for): {mediaFor:F2}");
            
            // Calculando média usando foreach
            double somaForeach = 0;
            foreach (double nota in notas)
            {
                somaForeach += nota;
            }
            double mediaForeach = somaForeach / notas.Length;
            Console.WriteLine($"Média (usando foreach): {mediaForeach:F2}");
            
            // Calculando média usando while
            double somaWhile = 0;
            int contador = 0;
            while (contador < notas.Length)
            {
                somaWhile += notas[contador];
                contador++;
            }
            double mediaWhile = somaWhile / notas.Length;
            Console.WriteLine($"Média (usando while): {mediaWhile:F2}");
            
            // Encontrando maior e menor nota
            double maiorNota = notas[0];
            double menorNota = notas[0];
            
            for (int i = 1; i < notas.Length; i++)
            {
                if (notas[i] > maiorNota)
                {
                    maiorNota = notas[i];
                }
                
                if (notas[i] < menorNota)
                {
                    menorNota = notas[i];
                }
            }
            
            Console.WriteLine($"\nMaior nota: {maiorNota:F1}");
            Console.WriteLine($"Menor nota: {menorNota:F1}");
            
            // Contando notas por faixa
            int notasAltas = 0;    // >= 8.0
            int notasMedias = 0;   // 6.0 - 7.9
            int notasBaixas = 0;   // < 6.0
            
            foreach (double nota in notas)
            {
                if (nota >= 8.0)
                {
                    notasAltas++;
                }
                else if (nota >= 6.0)
                {
                    notasMedias++;
                }
                else
                {
                    notasBaixas++;
                }
            }
            
            Console.WriteLine($"\nDistribuição das notas:");
            Console.WriteLine($"  Altas (>=8.0): {notasAltas}");
            Console.WriteLine($"  Médias (6.0-7.9): {notasMedias}");
            Console.WriteLine($"  Baixas (<6.0): {notasBaixas}");
            
            // Calculando média ponderada (últimas 3 notas têm peso 2)
            double somaPonderada = 0;
            int pesoTotal = 0;
            
            for (int i = 0; i < notas.Length; i++)
            {
                int peso = (i >= notas.Length - 3) ? 2 : 1; // Últimas 3 notas têm peso 2
                somaPonderada += notas[i] * peso;
                pesoTotal += peso;
            }
            
            double mediaPonderada = somaPonderada / pesoTotal;
            Console.WriteLine($"\nMédia ponderada (últimas 3 notas com peso 2): {mediaPonderada:F2}");
            
            // Verificando se foi aprovado
            bool aprovado = mediaFor >= 7.0;
            Console.WriteLine($"\nStatus: {(aprovado ? "APROVADO" : "REPROVADO")}");
            
            // Classificação do desempenho
            string classificacao = mediaFor switch
            {
                >= 9.0 => "Excelente",
                >= 8.0 => "Muito Bom",
                >= 7.0 => "Bom",
                >= 6.0 => "Regular",
                _ => "Insuficiente"
            };
            
            Console.WriteLine($"Classificação: {classificacao}");
            
            // Listando notas acima da média
            Console.WriteLine($"\nNotas acima da média ({mediaFor:F2}):");
            foreach (double nota in notas)
            {
                if (nota > mediaFor)
                {
                    Console.WriteLine($"  {nota:F1}");
                }
            }
            
            Console.WriteLine("\nPressione qualquer tecla para continuar...");
            Console.ReadKey();
        }
    }
} 