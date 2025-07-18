using System;

namespace Aula2.Exercicios
{
    class Exercicio3_InformacoesProduto
    {
        static void Main(string[] args)
        {
            // Exercício 3: Informações do Produto
            
            // Declarando variáveis para informações do produto
            string nomeProduto = "Notebook Dell Inspiron";
            decimal preco = 3499.99m;
            int quantidade = 5;
            bool disponivel = true;
            DateTime dataCadastro = DateTime.Now;
            
            // Calculando o valor total
            decimal valorTotal = preco * quantidade;
            
            // Exibindo o relatório
            Console.WriteLine("=== Relatório de Produto ===");
            Console.WriteLine($"Nome do Produto: {nomeProduto}");
            Console.WriteLine($"Preço Unitário: R$ {preco:F2}");
            Console.WriteLine($"Quantidade em Estoque: {quantidade}");
            Console.WriteLine($"Disponível: {(disponivel ? "Sim" : "Não")}");
            Console.WriteLine($"Data de Cadastro: {dataCadastro:dd/MM/yyyy HH:mm}");
            Console.WriteLine($"Valor Total em Estoque: R$ {valorTotal:F2}");
            
            // Informações adicionais
            Console.WriteLine("\n=== Informações Adicionais ===");
            Console.WriteLine($"Categoria: Eletrônicos");
            Console.WriteLine($"Fornecedor: Dell");
            Console.WriteLine($"Garantia: 12 meses");
            
            Console.WriteLine("\nPressione qualquer tecla para continuar...");
            Console.ReadKey();
        }
    }
} 