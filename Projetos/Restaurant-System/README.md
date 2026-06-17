# RestSystem — Restaurant Management API

API REST para gestão de restaurantes com **mesas, cardápio, pedidos, pagamentos e reservas**. Construída com .NET 10, Entity Framework Core (SQLite) e JWT.

## Arquitetura

```
Controller → Service → AppDbContext → SQLite
```

Monolito com Service Layer, inspirado no FinControl.

## Entidades

| Entidade | Descrição |
|----------|-----------|
| User | Funcionários (Admin, Waiter, Kitchen) |
| Table | Mesas com capacidade e status |
| Category | Categorias do cardápio |
| MenuItem | Itens do cardápio com preço e disponibilidade |
| Order | Pedidos com status e controle de abertura/fechamento |
| OrderItem | Itens do pedido com preço congelado no momento |
| Payment | Pagamentos com método e status |
| PaymentMethod | Formas de pagamento |
| Reservation | Reservas de mesas por clientes |

## Tecnologias

- **.NET 10** — ASP.NET Core Web API
- **Entity Framework Core** — SQLite (zero config)
- **JWT + BCrypt** — Autenticação
- **Swagger / OpenAPI** — Documentação
- **xUnit + Moq** — Testes unitários

## Como rodar

```bash
cd "Projetos/Restaurant-System/RestSystem.Api"
dotnet run
# Swagger: https://localhost:5001/swagger
```

### Autenticação

1. `POST /api/auth/register` — criar usuário
2. `POST /api/auth/login` — obter JWT
3. Usar token no Swagger (Authorize) ou header `Authorization: Bearer <token>`

## Estrutura

```
RestSystem.Api/
├── Controllers/        → (implementar)
├── Data/               → AppDbContext, Migrations
├── DTOs/               → Request/Response records por módulo
├── Models/
│   ├── Entities/       → 9 entidades
│   └── Enums/          → 5 enums
├── Services/           → Interfaces + implementações
└── Tests/              → xUnit + Moq
```

## Status do projeto

- [x] Entidades e Enums
- [x] DbContext
- [x] DTOs (todos os módulos)
- [x] AuthService (JWT + BCrypt)
- [x] TableService
- [x] MenuService
- [x] OrderService
- [x] PaymentService
- [x] ReservationService
- [ ] Controllers
- [ ] Testes unitários
