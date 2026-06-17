# C# .NET — Estudos

Repositório com projetos desenvolvidos durante os estudos de C# .NET, evoluindo de conceitos básicos até padrões de arquitetura mais avançados.

## Estrutura do repositório

| Pasta | Tipo | Arquitetura / Padrão | Descrição |
|-------|------|----------------------|-----------|
| [Curso_Basico](./Curso_Basico) | Exercícios | Console / procedural → POO | 10 aulas com arquivos `.cs` (variáveis, loops, classes, herança, exceções) |
| [Curso_Intermediario](./Curso_Intermediario) | Exercícios | Console / conceitos avançados | LINQ, async/await, generics, delegates, SOLID, xUnit |
| [Curso_Avancado](./Curso_Avancado) | Projeto console | DI + benchmarks + testes | Memory, patterns, microservices, reactive, source generators |
| [Curso-WebAPI-CRUD](./Curso-WebAPI-CRUD) | API | **Minimal API** monolítica | Primeiro CRUD com EF Core + SQLite + Swagger |
| [Curso-WebAPI-CRUD-RepositoryPattern](./Curso-WebAPI-CRUD-RepositoryPattern) | API | **Minimal API + Repository + Service Layer** | Biblioteca (Author, Book) com separação de camadas |
| [Projetos/E-Commerce](./Projetos/E-Commerce) | API | **Controllers + Repository + Service Layer** | E-commerce com estoque, enums e relacionamentos |
| [Projetos/School](./Projetos/School) | API multi-projeto | **Clean Architecture + CQRS** | Domain / Application / Infrastructure / WebAPI |
| [Projetos/ReservEasy](./Projetos/ReservEasy) | API | **Vertical Slices + CQRS + Domain Events + FastEndpoints** | Reservas com MediatR, FluentValidation e Serilog |
| [Projetos/Notifica](./Projetos/Notifica) | Sistema distribuído | **Clean Architecture + multi-host** | API REST, gRPC, Blazor WASM, Worker, SignalR, Redis, RabbitMQ |
| [Projetos/FinControl](./Projetos/FinControl) | API | **Monolito + Service Layer + JWT** | Controle financeiro com PostgreSQL, Swagger e Docker |
| [Projetos/Restaurant-System](./Projetos/Restaurant-System) | API | **Monolito + Service Layer + JWT** | Gestão de restaurante com SQLite, mesas, pedidos, pagamentos e reservas |

## Arquitetura por pasta

### Curso_Basico / Curso_Intermediario

```
Aula_N/
├── README.md          → teoria
└── Exercicios/        → arquivos .cs com Main()
```

- **Padrão:** scripts educacionais em console (sem `.csproj` por aula).
- **Conceitos:** sintaxe C#, POO, coleções, LINQ, async, design patterns básicos.

### Curso_Avancado

```
Curso_Avancado/
├── Curso_Avancado.csproj   → net8.0, console
└── Aula_N/                 → README + exercícios
```

- **Padrão:** console app com pacotes (DI, BenchmarkDotNet, xUnit, Reactive).
- **Conceitos:** performance, IoC, microservices, testes avançados.

### Curso-WebAPI-CRUD

```
Route Groups (Minimal API) → EF Core DbContext → SQLite
```

- **Padrão:** Minimal API monolítica.
- **Entidades:** Pessoa (`CursoModel`).

### Curso-WebAPI-CRUD-RepositoryPattern / Biblioteca-Aula

```
Routes → Service (regras) → Repository<T> → EF Core → SQLite
```

- **Padrão:** Minimal API + Repository Pattern + Service Layer.
- **Entidades:** Author, Book.

### Projetos/E-Commerce

```
Controller → Service → Repository<T> → EF Core → SQLite
```

- **Padrão:** API tradicional com Controllers.
- **Entidades:** Product, Category, Order, OrderItem.

### Projetos/School

```
WebAPI (Controllers)
    ↓ MediatR
Application (Commands / Queries / Handlers / Validators)
    ↓ interfaces
Domain (Entities)
    ↓ implementação
Infrastructure (DbContext, Migrations, EF Configurations)
```

- **Padrão:** Clean Architecture + CQRS (MediatR) + FluentValidation.
- **Projetos:** `Domain`, `Application`, `Infrastructure`, `WebAPI`.
- **Entidades:** Student, Course, Enrollment.

### Projetos/ReservEasy

```
Features/                    ← Vertical Slices
├── Properties/
├── Guests/
├── Bookings/
└── Payments/
    ├── *Endpoint.cs         ← FastEndpoints (REPR)
    ├── *Command / *Query    ← CQRS via MediatR
    ├── *Handler.cs
    └── *Validator.cs

Domain/   → Entities, Enums, Domain Events
Data/     → DbContext, EF Configurations
Common/   → Behaviors, Middleware, Exceptions
```

- **Padrão:** Vertical Slice Architecture + CQRS + Domain Events.
- **Entidades:** Property, Guest, Booking, Payment.

### Projetos/Notifica

```
Notifica.Domain          → Entities, Interfaces, Events
Notifica.Application     → Services, DTOs
Notifica.Infrastructure  → EF (PostgreSQL), Redis, RabbitMQ, Repositories
Notifica.Api             → REST Controllers + SignalR + JWT
Notifica.Grpc            → gRPC (user.proto)
Notifica.Web             → Blazor WebAssembly + SignalR Client + gRPC Client
Notifica.Worker          → Background Service (consumer RabbitMQ)
```

- **Padrão:** Clean Architecture distribuída (multi-processo).
- **Infra:** PostgreSQL, Redis, RabbitMQ via `docker-compose.yml`.

### Projetos/FinControl

```
Controllers → AuthService / ExpenseService → AppDbContext → PostgreSQL
```

- **Padrão:** monolito com Service Layer + JWT + Health Checks.
- **Testes:** `FinControl.Tests` (xUnit).
- **Deploy:** Dockerfile + docker-compose.

### Projetos/Restaurant-System

```
Controller → AuthService / TableService / MenuService / OrderService / PaymentService / ReservationService → AppDbContext → SQLite
```

- **Padrão:** monolito com Service Layer + JWT, herdado do FinControl.
- **Entidades:** 9 (User, Table, Category, MenuItem, Order, OrderItem, Payment, PaymentMethod, Reservation).
- **DTOs:** 27 records organizados por módulo.
- **Banco:** SQLite (sem Docker).

## Projetos

| # | Projeto | Arquitetura | Entidades | Destaque |
|---|---------|-------------|-----------|----------|
| 1 | [Curso-WebAPI-CRUD](./Curso-WebAPI-CRUD) | Minimal API | Pessoa | Primeiro contato com Minimal API + EF Core |
| 2 | [Biblioteca-Aula](./Curso-WebAPI-CRUD-RepositoryPattern/Biblioteca-Aula) | Minimal API + Repository + Service | Author, Book | Repository Pattern, Service Layer, validações |
| 3 | [E-Commerce](./Projetos/E-Commerce) | Controller + Repository + Service | Product, Category, Order, OrderItem | Controllers, enum, estoque, relacionamentos |
| 4 | [School](./Projetos/School) | Clean Architecture + CQRS | Student, Course, Enrollment | Multi-projeto, MediatR, FluentValidation |
| 5 | [ReservEasy](./Projetos/ReservEasy) | Vertical Slices + CQRS + Event-Driven | Property, Guest, Booking, Payment | FastEndpoints, Domain Events, Serilog |
| 6 | [Notifica](./Projetos/Notifica) | Clean Architecture + distribuída | User, Notification, Message | SignalR, gRPC, Blazor WASM, RabbitMQ, Redis |
| 7 | [FinControl](./Projetos/FinControl) | Monolito + Service Layer + JWT | User, Expense | PostgreSQL, Health Checks, Docker, testes xUnit |
| 8 | [Restaurant-System](./Projetos/Restaurant-System) | Monolito + Service Layer + JWT | User, Table, MenuItem, Order, Payment, Reservation | SQLite, 9 entidades, 6 services compartilhando padrão do FinControl |

## Roadmap de conceitos

```
Console / POO
  → Minimal API
    → Repository Pattern
      → Service Layer
        → Controllers
          → Clean Architecture + CQRS
            → Vertical Slices + Domain Events + FastEndpoints
              → Sistema distribuído (API + gRPC + Worker + Frontend)
```

Cada projeto adiciona uma camada nova em relação ao anterior, evoluindo de exercícios simples até arquiteturas modulares e orientadas a eventos.

## Comandos dotnet

### Ambiente

```powershell
dotnet --version
dotnet --list-sdks
dotnet --info
```

### Ciclo de vida (qualquer projeto)

```powershell
dotnet restore
dotnet build
dotnet run
dotnet watch run          # recarrega ao salvar
dotnet clean
dotnet publish -c Release
```

### Criar projetos

```powershell
dotnet new console -n MeuProjeto
dotnet new webapi -n MinhaApi
dotnet new classlib -n MinhaLib
dotnet new xunit -n MeuProjeto.Tests
dotnet new blazorwasm -n MeuFrontend
```

### Solution (.sln / .slnx)

```powershell
dotnet sln Projetos/School/School.slnx list
dotnet build Projetos/School/School.slnx
dotnet test Projetos/FinControl/FinControl.slnx
```

### Entity Framework Core

> Instale a ferramenta global uma vez: `dotnet tool install --global dotnet-ef`

```powershell
# Criar migration
dotnet ef migrations add Initial --project <caminho-do-projeto>

# Aplicar no banco
dotnet ef database update --project <caminho-do-projeto>

# Remover última migration
dotnet ef migrations remove --project <caminho-do-projeto>
```

### Testes

```powershell
dotnet test
dotnet test --filter "FullyQualifiedName~ExpenseService"
dotnet test --collect:"XPlat Code Coverage"
```

## Como rodar cada projeto

### Exercícios (Curso_Basico / Curso_Intermediario)

Os exercícios são arquivos `.cs` isolados. Para executar:

```powershell
dotnet new console -n ExercicioTemp
# copie o .cs do exercício como Program.cs
cd ExercicioTemp
dotnet run
```

### Curso_Avancado

```powershell
cd Curso_Avancado
dotnet restore
dotnet run
dotnet test
```

### Curso-WebAPI-CRUD

```powershell
cd Curso-WebAPI-CRUD
dotnet run
# Swagger: http://localhost:5253/swagger
```

### Biblioteca-Aula

```powershell
cd Curso-WebAPI-CRUD-RepositoryPattern/Biblioteca-Aula
dotnet run
# Swagger: http://localhost:5134/swagger
```

### E-Commerce

```powershell
cd Projetos/E-Commerce
dotnet run
```

### School

```powershell
cd Projetos/School
dotnet restore
dotnet run --project WebAPI
```

### ReservEasy

```powershell
cd Projetos/ReservEasy
dotnet restore
dotnet run --project src/ReservEasy.Api
# https://localhost:7001
```

### Notifica

```powershell
cd Projetos/Notifica
docker compose up -d          # PostgreSQL, Redis, RabbitMQ

dotnet run --project src/Notifica.Api       # REST + SignalR
dotnet run --project src/Notifica.Grpc      # gRPC
dotnet run --project src/Notifica.Web         # Blazor WASM
dotnet run --project src/Notifica.Worker      # Consumer RabbitMQ

dotnet test --project tests/Notifica.IntegrationTests
```

### FinControl

```powershell
cd Projetos/FinControl
dotnet run --project src/FinControl.Api
dotnet test --project tests/FinControl.Tests

# Com Docker (PostgreSQL incluso)
cd src/FinControl.Api
docker compose up --build
```

### Restaurant-System

```powershell
cd Projetos/Restaurant-System/RestSystem.Api
dotnet run
# Swagger: https://localhost:5001/swagger
```

## Referências

| Documentação | Link |
|--------------|------|
| C# | https://learn.microsoft.com/pt-br/dotnet/csharp/ |
| .NET CLI | https://learn.microsoft.com/pt-br/dotnet/core/tools/ |
| ASP.NET Core | https://learn.microsoft.com/pt-br/aspnet/core/ |
| EF Core | https://learn.microsoft.com/pt-br/ef/core/ |
