using System;

namespace Aula3.Exercicios
{
    class Exercicio3_CalculadoraDesconto
    {
        static void Main(string[] args)
        {
            // Exercício 3: Calculadora de Desconto
            
            Console.WriteLine("=== Calculadora de Desconto ===");
            
            // Dados do produto
            decimal precoUnitario = 29.99m;
            int quantidade = 15;
            
            Console.WriteLine($"Preço unitário: R$ {precoUnitario:F2}");
            Console.WriteLine($"Quantidade: {quantidade}");
            Console.WriteLine();
            
            // Cálculo do valor total sem desconto
            decimal valorTotal = precoUnitario * quantidade;
            
            // Aplicando descontos baseados na quantidade
            decimal desconto = 0m;
            string tipoDesconto = "";
            
            // Regras de desconto usando operadores lógicos
            if (quantidade >= 20)
            {
                desconto = valorTotal * 0.15m; // 15% de desconto
                tipoDesconto = "15% (20+ unidades)";
            }
            else if (quantidade >= 10)
            {
                desconto = valorTotal * 0.10m; // 10% de desconto
                tipoDesconto = "10% (10-19 unidades)";
            }
            else if (quantidade >= 5)
            {
                desconto = valorTotal * 0.05m; // 5% de desconto
                tipoDesconto = "5% (5-9 unidades)";
            }
            else
            {
                tipoDesconto = "Sem desconto";
            }
            
            // Cálculo do valor final
            decimal valorFinal = valorTotal - desconto;
            
            // Exibindo resultados
            Console.WriteLine("--- Cálculo do Desconto ---");
            Console.WriteLine($"Valor total: R$ {valorTotal:F2}");
            Console.WriteLine($"Tipo de desconto: {tipoDesconto}");
            Console.WriteLine($"Valor do desconto: R$ {desconto:F2}");
            Console.WriteLine($"Valor final: R$ {valorFinal:F2}");
            
            // Verificações adicionais usando operadores
            bool descontoAplicado = desconto > 0;
            bool descontoAlto = desconto >= valorTotal * 0.10m;
            bool quantidadeGrande = quantidade >= 15;
            
            Console.WriteLine("\n--- Análise do Pedido ---");
            Console.WriteLine($"Desconto foi aplicado: {descontoAplicado}");
            Console.WriteLine($"Desconto alto (>=10%): {descontoAlto}");
            Console.WriteLine($"Quantidade grande (>=15): {quantidadeGrande}");
            
            // Usando operador ternário para mensagens
            string mensagemDesconto = descontoAplicado ? "Parabéns! Você ganhou desconto!" : "Compre mais para ganhar desconto!";
            string categoriaCliente = quantidade >= 20 ? "Cliente Premium" : 
                                    quantidade >= 10 ? "Cliente Regular" : 
                                    quantidade >= 5 ? "Cliente Básico" : "Cliente Iniciante";
            
            Console.WriteLine($"\n--- Mensagens ---");
            Console.WriteLine($"Mensagem: {mensagemDesconto}");
            Console.WriteLine($"Categoria: {categoriaCliente}");
            
            // Economia em porcentagem
            decimal economiaPercentual = valorTotal > 0 ? (desconto / valorTotal) * 100 : 0;
            Console.WriteLine($"Economia: {economiaPercentual:F1}%");
            
            Console.WriteLine("\nPressione qualquer tecla para continuar...");
            Console.ReadKey();
        }
    }
} 