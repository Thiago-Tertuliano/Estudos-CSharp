using System;

namespace Aula4.Exercicios
{
    class Exercicio3_SistemaMenu
    {
        static void Main(string[] args)
        {
            // Exercício 3: Sistema de Menu
            
            Console.WriteLine("=== Sistema de Menu ===");
            
            // Simulando diferentes opções do menu
            int[] opcoes = { 1, 2, 3, 4, 5, 0, 9 };
            
            foreach (int opcao in opcoes)
            {
                Console.WriteLine($"\n--- Opção selecionada: {opcao} ---");
                
                // Menu usando switch tradicional
                switch (opcao)
                {
                    case 1:
                        Console.WriteLine("Ação: Cadastrar novo usuário");
                        Console.WriteLine("Status: Executando...");
                        break;
                    case 2:
                        Console.WriteLine("Ação: Listar usuários");
                        Console.WriteLine("Status: Buscando dados...");
                        break;
                    case 3:
                        Console.WriteLine("Ação: Editar usuário");
                        Console.WriteLine("Status: Abrindo formulário...");
                        break;
                    case 4:
                        Console.WriteLine("Ação: Excluir usuário");
                        Console.WriteLine("Status: Confirmando exclusão...");
                        break;
                    case 5:
                        Console.WriteLine("Ação: Relatórios");
                        Console.WriteLine("Status: Gerando relatório...");
                        break;
                    case 0:
                        Console.WriteLine("Ação: Sair do sistema");
                        Console.WriteLine("Status: Encerrando...");
                        break;
                    default:
                        Console.WriteLine("Ação: Opção inválida");
                        Console.WriteLine("Status: Erro - opção não reconhecida");
                        break;
                }
                
                // Usando switch expression para mensagens
                string mensagem = opcao switch
                {
                    1 => "Usuário cadastrado com sucesso!",
                    2 => "Lista de usuários carregada!",
                    3 => "Usuário editado com sucesso!",
                    4 => "Usuário excluído com sucesso!",
                    5 => "Relatório gerado com sucesso!",
                    0 => "Sistema encerrado!",
                    _ => "Erro: Opção inválida!"
                };
                
                Console.WriteLine($"Mensagem: {mensagem}");
                
                // Verificações adicionais
                bool opcaoValida = opcao >= 0 && opcao <= 5;
                bool opcaoCritica = opcao == 4 || opcao == 0;
                bool opcaoLeitura = opcao == 2 || opcao == 5;
                
                Console.WriteLine($"Opção válida: {opcaoValida}");
                Console.WriteLine($"Opção crítica: {opcaoCritica}");
                Console.WriteLine($"Opção de leitura: {opcaoLeitura}");
            }
            
            // Menu interativo simulado
            Console.WriteLine("\n=== Menu Interativo ===");
            Console.WriteLine("1 - Cadastrar usuário");
            Console.WriteLine("2 - Listar usuários");
            Console.WriteLine("3 - Editar usuário");
            Console.WriteLine("4 - Excluir usuário");
            Console.WriteLine("5 - Relatórios");
            Console.WriteLine("0 - Sair");
            
            // Simulando seleção do usuário
            int opcaoSelecionada = 3;
            Console.WriteLine($"\nOpção selecionada pelo usuário: {opcaoSelecionada}");
            
            // Processando a seleção
            if (opcaoSelecionada >= 1 && opcaoSelecionada <= 5)
            {
                string acao = opcaoSelecionada switch
                {
                    1 => "Cadastrando usuário...",
                    2 => "Listando usuários...",
                    3 => "Editando usuário...",
                    4 => "Excluindo usuário...",
                    5 => "Gerando relatório...",
                    _ => "Processando..."
                };
                
                Console.WriteLine($"Executando: {acao}");
                
                // Verificação de permissão
                bool temPermissao = opcaoSelecionada != 4; // Simula que exclusão precisa de permissão especial
                if (temPermissao)
                {
                    Console.WriteLine("Operação executada com sucesso!");
                }
                else
                {
                    Console.WriteLine("Erro: Sem permissão para esta operação!");
                }
            }
            else if (opcaoSelecionada == 0)
            {
                Console.WriteLine("Saindo do sistema...");
            }
            else
            {
                Console.WriteLine("Opção inválida!");
            }
            
            Console.WriteLine("\nPressione qualquer tecla para continuar...");
            Console.ReadKey();
        }
    }
} 