# C# .NET — Estudos

Repositório com projetos desenvolvidos durante os estudos de C# .NET, evoluindo de conceitos básicos até padrões de arquitetura mais avançados.

## Projetos

| # | Projeto | Arquitetura | Entidades | Destaque |
|---|---------|-------------|-----------|----------|
| 1 | [Curso-WebAPI-CRUD](./Curso-WebAPI-CRUD) | Minimal API | Pessoa | Primeiro contato com Minimal API + EF Core |
| 2 | [Biblioteca-Aula](./Curso-WebAPI-CRUD-RepositoryPattern/Biblioteca-Aula) | Minimal API + Repository + Service | Author, Book | **Repository Pattern**, Service Layer, validações |
| 3 | [E-Commerce](./Projetos/E-Commerce) | Controller + Repository + Service | Product, Category, Order, OrderItem | **Controllers**, enum, estoque, relacionamentos |
| 4 | [School](./Projetos/School) | Clean Architecture + CQRS | Student, Course, Enrollment | **Multi-projeto**, MediatR, FluentValidation, Exception Middleware |
| 5 | [ReservEasy](./ReservEasy) | Vertical Slices + CQRS + Event-Driven + FastEndpoints | Property, Guest, Booking, Payment | **FastEndpoints**, Domain Events, Serilog, paginação |

## Roadmap de conceitos

```
Minimal API → Repository Pattern → Service Layer → Controllers
→ Clean Architecture + CQRS → Vertical Slices + Domain Events + FastEndpoints
```

Cada projeto adiciona uma camada nova em relação ao anterior, evoluindo de APIs simples até arquiteturas modulares e orientadas a eventos.
