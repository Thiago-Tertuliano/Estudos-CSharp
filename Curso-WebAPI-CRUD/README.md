# Curso WebAPI CRUD

Primeiro projeto da seção de C# .NET. Uma API REST simples usando **Minimal API** com **Entity Framework Core** e **SQLite**.

## Conceitos aplicados

- Minimal API (`MapGet`, `MapPost`, `MapPut`, `MapDelete`)
- Entity Framework Core + SQLite
- Route Groups
- Swagger / OpenAPI
- Migrations

## Como rodar

```bash
dotnet run
```

Acessar `http://localhost:5253/swagger` para testar os endpoints.

## Endpoints

| Método | Rota | Descrição |
|--------|------|-----------|
| GET | `/curso` | Listar todos |
| GET | `/curso/{id}` | Buscar por ID |
| POST | `/curso` | Criar |
| PUT | `/curso/{id}` | Atualizar |
| DELETE | `/curso/{id}` | Deletar |
