# Como Usar o Curso de C#

## Pré-requisitos

### 1. Instalar .NET SDK
- Baixe o .NET SDK em: https://dotnet.microsoft.com/download
- Instale seguindo as instruções para seu sistema operacional
- Verifique a instalação executando: `dotnet --version`

### 2. IDE Recomendada
- **Visual Studio Code** (gratuito)
  - Instale a extensão "C#" da Microsoft
  - Instale a extensão "C# Dev Kit" para melhor experiência
- **Visual Studio** (Community Edition é gratuita)
- **Rider** (pago, mas muito completo)

## Estrutura do Curso

```
Estudos-C#/Curso/
├── README.md                    # Visão geral do curso
├── COMO_USAR.md                # Este arquivo
├── Aula_1/                     # Introdução ao C#
│   ├── README.md               # Material teórico
│   └── Exercicios/             # Exercícios práticos
├── Aula_2/                     # Variáveis e Tipos
├── Aula_3/                     # Operadores
├── Aula_4/                     # Estruturas de Controle
├── Aula_5/                     # Loops
├── Aula_6/                     # Arrays e Coleções
├── Aula_7/                     # Métodos e Funções
├── Aula_8/                     # Classes e Objetos
├── Aula_9/                     # Herança e Polimorfismo
└── Aula_10/                    # Tratamento de Exceções
```

## Como Estudar

### 1. Ordem das Aulas
Siga a ordem numérica das aulas (1 a 10). Cada aula constrói sobre os conceitos das anteriores.

### 2. Para Cada Aula:
1. **Leia o README.md** - Entenda a teoria
2. **Execute os exercícios** - Pratique o código
3. **Modifique os exercícios** - Experimente mudanças
4. **Crie seus próprios exemplos** - Aplique o conhecimento

### 3. Executando os Exercícios

#### Usando Visual Studio Code:
1. Abra a pasta do exercício no VS Code
2. Abra o terminal (Ctrl+`)
3. Execute: `dotnet run`

#### Usando Visual Studio:
1. Abra o arquivo .cs no Visual Studio
2. Pressione F5 para executar

#### Usando Terminal:
```bash
cd "caminho/para/exercicio"
dotnet run
```

### 4. Criando Novos Projetos
```bash
# Criar novo projeto console
dotnet new console -n MeuProjeto

# Navegar para o projeto
cd MeuProjeto

# Executar o projeto
dotnet run
```

## Dicas de Estudo

### 1. Prática Diária
- Dedique pelo menos 1 hora por dia
- Execute todos os exercícios
- Experimente modificar o código

### 2. Experimentação
- Não tenha medo de "quebrar" o código
- Teste diferentes valores
- Adicione novos recursos aos exercícios

### 3. Documentação
- Use o IntelliSense da IDE
- Consulte a documentação oficial: https://docs.microsoft.com/pt-br/dotnet/csharp/
- Use o Stack Overflow para dúvidas

### 4. Projetos Práticos
Após completar o curso, crie projetos práticos:
- Calculadora
- Sistema de cadastro
- Jogo simples
- API básica

## Recursos Adicionais

### Documentação Oficial
- [C# Documentation](https://docs.microsoft.com/pt-br/dotnet/csharp/)
- [.NET Documentation](https://docs.microsoft.com/pt-br/dotnet/)

### Cursos Online
- Microsoft Learn: C# Fundamentals
- Pluralsight: C# Path
- Udemy: C# Complete Course

### Comunidades
- Stack Overflow
- Reddit: r/csharp
- Discord: C# Community

## Solução de Problemas

### Erro: "dotnet não é reconhecido"
- Reinstale o .NET SDK
- Verifique se o PATH está configurado

### Erro de Compilação
- Verifique a sintaxe
- Use o IntelliSense da IDE
- Leia as mensagens de erro cuidadosamente

### IDE não funciona
- Reinstale as extensões
- Verifique se o .NET SDK está instalado
- Reinicie a IDE

## Próximos Passos

Após completar este curso básico, considere:

1. **C# Intermediário**
   - LINQ
   - Async/Await
   - Generics
   - Reflection

2. **Frameworks**
   - ASP.NET Core (Web)
   - WPF (Desktop)
   - Xamarin (Mobile)

3. **Conceitos Avançados**
   - Design Patterns
   - SOLID Principles
   - Unit Testing
   - Dependency Injection

## Suporte

Se encontrar problemas:
1. Verifique se o .NET SDK está instalado
2. Consulte a documentação oficial
3. Pesquise no Stack Overflow
4. Experimente diferentes abordagens

**Boa sorte com seus estudos de C#!** 🚀 