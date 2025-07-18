using System;

namespace Aula3.Exercicios
{
    class Exercicio2_VerificacaoIdade
    {
        static void Main(string[] args)
        {
            // Exercício 2: Verificação de Idade
            
            Console.WriteLine("=== Verificação de Idade e Habilitação ===");
            
            // Dados da pessoa
            int idade = 22;
            bool temHabilitacao = true;
            int tempoHabilitacao = 3; // anos
            
            Console.WriteLine($"Idade: {idade} anos");
            Console.WriteLine($"Tem habilitação: {(temHabilitacao ? "Sim" : "Não")}");
            Console.WriteLine($"Tempo de habilitação: {tempoHabilitacao} anos");
            Console.WriteLine();
            
            // Verificações usando operadores lógicos
            bool maiorIdade = idade >= 18;
            bool podeDirigir = idade >= 18 && temHabilitacao;
            bool habilitacaoValida = temHabilitacao && tempoHabilitacao >= 2;
            bool podeDirigirMoto = idade >= 18 && temHabilitacao;
            bool podeDirigirCaminhao = idade >= 21 && temHabilitacao && tempoHabilitacao >= 2;
            
            // Exibindo resultados
            Console.WriteLine("--- Resultados das Verificações ---");
            Console.WriteLine($"É maior de idade: {maiorIdade}");
            Console.WriteLine($"Pode dirigir: {podeDirigir}");
            Console.WriteLine($"Habilitação válida (mínimo 2 anos): {habilitacaoValida}");
            Console.WriteLine($"Pode dirigir moto: {podeDirigirMoto}");
            Console.WriteLine($"Pode dirigir caminhão: {podeDirigirCaminhao}");
            
            // Usando operador ternário para mensagens
            Console.WriteLine("\n--- Mensagens Personalizadas ---");
            string statusIdade = idade >= 18 ? "Maior de idade" : "Menor de idade";
            string statusDirecao = podeDirigir ? "Pode dirigir" : "Não pode dirigir";
            string tipoVeiculo = idade >= 21 ? "Pode dirigir qualquer veículo" : "Pode dirigir carros e motos";
            
            Console.WriteLine($"Status de idade: {statusIdade}");
            Console.WriteLine($"Status de direção: {statusDirecao}");
            Console.WriteLine($"Tipo de veículo: {tipoVeiculo}");
            
            // Verificações adicionais
            Console.WriteLine("\n--- Verificações Específicas ---");
            bool idadeParaVotar = idade >= 16;
            bool idadeParaCandidato = idade >= 18;
            bool idadeParaAposentadoria = idade >= 65;
            
            Console.WriteLine($"Pode votar (16+): {idadeParaVotar}");
            Console.WriteLine($"Pode ser candidato (18+): {idadeParaCandidato}");
            Console.WriteLine($"Pode se aposentar (65+): {idadeParaAposentadoria}");
            
            Console.WriteLine("\nPressione qualquer tecla para continuar...");
            Console.ReadKey();
        }
    }
} 