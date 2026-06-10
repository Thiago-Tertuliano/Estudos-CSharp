using System;

namespace Aula4.Exercicios
{
    class Exercicio2_CalculadoraIMC
    {
        static void Main(string[] args)
        {
            // Exercício 2: Calculadora de IMC
            
            Console.WriteLine("=== Calculadora de IMC ===");
            
            // Dados da pessoa
            double peso = 70.5; // kg
            double altura = 1.75; // metros
            
            Console.WriteLine($"Peso: {peso} kg");
            Console.WriteLine($"Altura: {altura} m");
            Console.WriteLine();
            
            // Cálculo do IMC
            double imc = peso / (altura * altura);
            Console.WriteLine($"IMC: {imc:F2}");
            
            // Classificação do IMC usando if-else if-else
            Console.WriteLine("\n--- Classificação (if-else) ---");
            if (imc < 18.5)
            {
                Console.WriteLine("Classificação: Abaixo do peso");
                Console.WriteLine("Recomendação: Consulte um nutricionista");
            }
            else if (imc < 25)
            {
                Console.WriteLine("Classificação: Peso normal");
                Console.WriteLine("Recomendação: Mantenha hábitos saudáveis");
            }
            else if (imc < 30)
            {
                Console.WriteLine("Classificação: Sobrepeso");
                Console.WriteLine("Recomendação: Pratique exercícios regularmente");
            }
            else if (imc < 35)
            {
                Console.WriteLine("Classificação: Obesidade Grau I");
                Console.WriteLine("Recomendação: Consulte um médico");
            }
            else if (imc < 40)
            {
                Console.WriteLine("Classificação: Obesidade Grau II");
                Console.WriteLine("Recomendação: Procure tratamento especializado");
            }
            else
            {
                Console.WriteLine("Classificação: Obesidade Grau III");
                Console.WriteLine("Recomendação: Procure tratamento urgente");
            }
            
            // Usando switch para faixas de IMC
            Console.WriteLine("\n--- Classificação (switch) ---");
            int faixaIMC = (int)(imc / 5); // Agrupa em faixas de 5
            switch (faixaIMC)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                    Console.WriteLine("Faixa: Peso baixo a normal");
                    break;
                case 4:
                case 5:
                    Console.WriteLine("Faixa: Sobrepeso");
                    break;
                case 6:
                case 7:
                    Console.WriteLine("Faixa: Obesidade Grau I");
                    break;
                case 8:
                    Console.WriteLine("Faixa: Obesidade Grau II");
                    break;
                default:
                    Console.WriteLine("Faixa: Obesidade Grau III");
                    break;
            }
            
            // Usando switch expression
            string categoria = imc switch
            {
                < 18.5 => "Abaixo do peso",
                < 25 => "Peso normal",
                < 30 => "Sobrepeso",
                < 35 => "Obesidade Grau I",
                < 40 => "Obesidade Grau II",
                _ => "Obesidade Grau III"
            };
            Console.WriteLine($"\nCategoria (switch expression): {categoria}");
            
            // Verificações adicionais
            Console.WriteLine("\n--- Análise Detalhada ---");
            bool pesoNormal = imc >= 18.5 && imc < 25;
            bool precisaAtencao = imc >= 25;
            bool riscoAlto = imc >= 30;
            
            Console.WriteLine($"Peso normal: {pesoNormal}");
            Console.WriteLine($"Precisa atenção: {precisaAtencao}");
            Console.WriteLine($"Risco alto: {riscoAlto}");
            
            // Recomendações baseadas no IMC
            string recomendacao = imc switch
            {
                < 18.5 => "Aumente a ingestão calórica com alimentos nutritivos",
                < 25 => "Mantenha uma dieta equilibrada e exercícios regulares",
                < 30 => "Reduza a ingestão calórica e aumente a atividade física",
                < 35 => "Procure orientação médica para um plano de emagrecimento",
                < 40 => "Considere tratamento médico especializado",
                _ => "Procure tratamento médico urgente"
            };
            
            Console.WriteLine($"\nRecomendação: {recomendacao}");
            
            Console.WriteLine("\nPressione qualquer tecla para continuar...");
            Console.ReadKey();
        }
    }
} 