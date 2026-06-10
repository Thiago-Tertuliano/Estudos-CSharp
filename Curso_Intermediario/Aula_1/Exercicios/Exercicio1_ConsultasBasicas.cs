using System;
using System.Collections.Generic;
using System.Linq;

namespace Aula1.Exercicios
{
    // Classes para os exercícios
    public class Produto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public decimal Preco { get; set; }
        public string Categoria { get; set; }
        public int Estoque { get; set; }
    }

    public class Exercicio1_ConsultasBasicas
    {
        static void Main(string[] args)
        {
            // Exercício 1: Consultas LINQ Básicas
            
            Console.WriteLine("=== Consultas LINQ Básicas ===");
            
            // Lista de produtos para os exercícios
            var produtos = new List<Produto>
            {
                new Produto { Id = 1, Nome = "Notebook Dell", Preco = 3500.00m, Categoria = "Eletrônicos", Estoque = 10 },
                new Produto { Id = 2, Nome = "Mouse Logitech", Preco = 89.90m, Categoria = "Periféricos", Estoque = 50 },
                new Produto { Id = 3, Nome = "Teclado Mecânico", Preco = 299.90m, Categoria = "Periféricos", Estoque = 25 },
                new Produto { Id = 4, Nome = "Monitor LG", Preco = 899.90m, Categoria = "Eletrônicos", Estoque = 15 },
                new Produto { Id = 5, Nome = "Headset Gamer", Preco = 199.90m, Categoria = "Periféricos", Estoque = 30 },
                new Produto { Id = 6, Nome = "SSD 500GB", Preco = 399.90m, Categoria = "Componentes", Estoque = 20 },
                new Produto { Id = 7, Nome = "Memória RAM 16GB", Preco = 299.90m, Categoria = "Componentes", Estoque = 40 },
                new Produto { Id = 8, Nome = "Webcam HD", Preco = 149.90m, Categoria = "Periféricos", Estoque = 35 }
            };
            
            Console.WriteLine("Lista de produtos:");
            foreach (var produto in produtos)
            {
                Console.WriteLine($"- {produto.Nome} (R$ {produto.Preco:F2}) - {produto.Categoria}");
            }
            Console.WriteLine();
            
            // 1. Filtragem com Where
            Console.WriteLine("=== 1. Filtragem ===");
            
            // Produtos da categoria Eletrônicos
            var eletronicos = produtos.Where(p => p.Categoria == "Eletrônicos");
            Console.WriteLine("Produtos da categoria Eletrônicos:");
            foreach (var produto in eletronicos)
            {
                Console.WriteLine($"  - {produto.Nome}");
            }
            
            // Produtos com preço maior que R$ 200
            var produtosCaros = produtos.Where(p => p.Preco > 200);
            Console.WriteLine("\nProdutos com preço maior que R$ 200:");
            foreach (var produto in produtosCaros)
            {
                Console.WriteLine($"  - {produto.Nome}: R$ {produto.Preco:F2}");
            }
            
            // Produtos com estoque baixo (menos de 20)
            var estoqueBaixo = produtos.Where(p => p.Estoque < 20);
            Console.WriteLine("\nProdutos com estoque baixo (< 20):");
            foreach (var produto in estoqueBaixo)
            {
                Console.WriteLine($"  - {produto.Nome}: {produto.Estoque} unidades");
            }
            
            // 2. Projeção com Select
            Console.WriteLine("\n=== 2. Projeção ===");
            
            // Apenas os nomes dos produtos
            var nomesProdutos = produtos.Select(p => p.Nome);
            Console.WriteLine("Nomes dos produtos:");
            foreach (var nome in nomesProdutos)
            {
                Console.WriteLine($"  - {nome}");
            }
            
            // Produtos com informações formatadas
            var produtosFormatados = produtos.Select(p => new
            {
                Nome = p.Nome.ToUpper(),
                PrecoFormatado = $"R$ {p.Preco:F2}",
                Categoria = p.Categoria
            });
            
            Console.WriteLine("\nProdutos formatados:");
            foreach (var produto in produtosFormatados)
            {
                Console.WriteLine($"  - {produto.Nome} ({produto.PrecoFormatado}) - {produto.Categoria}");
            }
            
            // 3. Ordenação
            Console.WriteLine("\n=== 3. Ordenação ===");
            
            // Ordenar por preço (crescente)
            var ordenadosPorPreco = produtos.OrderBy(p => p.Preco);
            Console.WriteLine("Produtos ordenados por preço (crescente):");
            foreach (var produto in ordenadosPorPreco)
            {
                Console.WriteLine($"  - {produto.Nome}: R$ {produto.Preco:F2}");
            }
            
            // Ordenar por nome (decrescente)
            var ordenadosPorNomeDesc = produtos.OrderByDescending(p => p.Nome);
            Console.WriteLine("\nProdutos ordenados por nome (decrescente):");
            foreach (var produto in ordenadosPorNomeDesc)
            {
                Console.WriteLine($"  - {produto.Nome}");
            }
            
            // Ordenação múltipla: categoria, depois preço
            var ordenacaoMultipla = produtos.OrderBy(p => p.Categoria).ThenBy(p => p.Preco);
            Console.WriteLine("\nProdutos ordenados por categoria e preço:");
            foreach (var produto in ordenacaoMultipla)
            {
                Console.WriteLine($"  - {produto.Categoria}: {produto.Nome} (R$ {produto.Preco:F2})");
            }
            
            // 4. Primeiro e Último
            Console.WriteLine("\n=== 4. Primeiro e Último ===");
            
            var primeiroProduto = produtos.First();
            Console.WriteLine($"Primeiro produto: {primeiroProduto.Nome}");
            
            var ultimoProduto = produtos.Last();
            Console.WriteLine($"Último produto: {ultimoProduto.Nome}");
            
            var primeiroEletronico = produtos.FirstOrDefault(p => p.Categoria == "Eletrônicos");
            Console.WriteLine($"Primeiro produto eletrônico: {primeiroEletronico?.Nome}");
            
            // 5. Combinando operadores
            Console.WriteLine("\n=== 5. Consultas Combinadas ===");
            
            // Produtos periféricos ordenados por preço
            var perifericosOrdenados = produtos
                .Where(p => p.Categoria == "Periféricos")
                .OrderBy(p => p.Preco)
                .Select(p => new { p.Nome, p.Preco });
            
            Console.WriteLine("Periféricos ordenados por preço:");
            foreach (var produto in perifericosOrdenados)
            {
                Console.WriteLine($"  - {produto.Nome}: R$ {produto.Preco:F2}");
            }
            
            // Produtos caros com estoque alto
            var produtosCarosEstoqueAlto = produtos
                .Where(p => p.Preco > 300 && p.Estoque > 20)
                .Select(p => new { p.Nome, p.Preco, p.Estoque });
            
            Console.WriteLine("\nProdutos caros com estoque alto:");
            foreach (var produto in produtosCarosEstoqueAlto)
            {
                Console.WriteLine($"  - {produto.Nome}: R$ {produto.Preco:F2} ({produto.Estoque} unidades)");
            }
            
            Console.WriteLine("\nPressione qualquer tecla para continuar...");
            Console.ReadKey();
        }
    }
} 