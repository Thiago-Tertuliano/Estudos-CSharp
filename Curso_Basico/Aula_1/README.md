# Aula 1 - Introdução ao C# e Configuração do Ambiente

## Objetivos da Aula
- Entender o que é C# e sua história
- Configurar o ambiente de desenvolvimento
- Criar o primeiro programa "Hello World"
- Compreender a estrutura básica de um programa C#

## Conteúdo Teórico

### O que é C#?
C# (C Sharp) é uma linguagem de programação moderna, orientada a objetos, desenvolvida pela Microsoft como parte da plataforma .NET. Foi criada por Anders Hejlsberg e sua equipe.

### Características do C#
- **Orientada a Objetos**: Suporta herança, encapsulamento e polimorfismo
- **Tipagem Forte**: Cada variável tem um tipo específico
- **Gerenciamento Automático de Memória**: Garbage collector integrado
- **Multiplataforma**: Funciona em Windows, Linux e macOS
- **Open Source**: Código fonte disponível no GitHub

### Configuração do Ambiente

#### 1. Instalar .NET SDK
- Baixe o .NET SDK em: https://dotnet.microsoft.com/download
- Instale seguindo as instruções para seu sistema operacional

#### 2. Verificar a Instalação
```bash
dotnet --version
```

#### 3. IDEs Recomendadas
- **Visual Studio**: IDE completa da Microsoft
- **Visual Studio Code**: Editor leve com extensões
- **Rider**: IDE da JetBrains (pago)

### Estrutura Básica de um Programa C#

```csharp
using System;

namespace MeuPrimeiroPrograma
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }
}
```

### Componentes da Estrutura:
- **using System**: Importa bibliotecas necessárias
- **namespace**: Organiza o código em grupos lógicos
- **class**: Define uma classe (unidade básica de organização)
- **Main**: Método principal, ponto de entrada do programa
- **Console.WriteLine**: Função para exibir texto no console

## Exercícios

### Exercício 1 - Hello World
Crie um programa que exiba "Olá, mundo!" no console.

### Exercício 2 - Seu Nome
Crie um programa que exiba seu nome e idade.

### Exercício 3 - Informações Pessoais
Crie um programa que exiba suas informações pessoais (nome, idade, cidade, profissão).

## Dicas
- Sempre use ponto e vírgula (;) no final das instruções
- C# é case-sensitive (diferencia maiúsculas de minúsculas)
- Use comentários para explicar seu código
- Mantenha o código organizado e legível 