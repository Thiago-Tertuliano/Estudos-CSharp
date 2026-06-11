# Biblioteca API

API REST para gerenciamento de biblioteca com **Repository Pattern** e **Service Layer**. Construída com .NET 10, Minimal API, Entity Framework Core e SQLite.

## Arquitetura

```
Route (HTTP) → Service (regras de negócio) → Repository (EF Core) → SQLite
```

## Conceitos aplicados

- **Repository Pattern** — Interface genérica `IRepository<T>` com implementação em EF Core
- **Service Layer** — Regras de negócio separadas da camada HTTP
- **Encapsulamento** — Models com `private set` e métodos `Update()`
- **DTOs** — `record` types para separar request/response
- **Minimal API** — `MapGroup`, `WithTags`, extension methods
- **Injeção de Dependência** — `AddScoped`, `AddDbContext`
- **Validação** — Regras de negócio com exceções específicas (`ArgumentException`, `InvalidOperationException`)

## Regras de negócio

| Regra | Onde |
|-------|------|
| Nome do autor obrigatório | AuthorService |
| Idade não pode ser negativa | AuthorService |
| Gênero obrigatório | AuthorService |
| **Não deletar autor com livros** | AuthorService → `InvalidOperationException` |
| Título do livro obrigatório | BookService |
| Ano entre 1400 e ano que vem | BookService |
| Preço não pode ser negativo | BookService |
| **AuthorId deve existir** | BookService |
| Swagger separado por tags | AuthorRoutes / BookRoutes |

## Como rodar

```bash
cd "Curso-WebAPI-CRUD-RepositoryPattern/Biblioteca-Aula"
dotnet run
```

Acessar `http://localhost:5134/swagger`.

## Endpoints

### Author
| Método | Rota | Descrição |
|--------|------|-----------|
| GET | `/author` | Listar autores |
| GET | `/author/{id}` | Buscar autor |
| POST | `/author` | Criar autor |
| PUT | `/author/{id}` | Atualizar autor |
| DELETE | `/author/{id}` | Deletar autor |
| GET | `/author/{id}/books` | Livros do autor |

### Book
| Método | Rota | Descrição |
|--------|------|-----------|
| GET | `/book` | Listar livros |
| GET | `/book/{id}` | Buscar livro |
| POST | `/book` | Criar livro |
| PUT | `/book/{id}` | Atualizar livro |
| DELETE | `/book/{id}` | Deletar livro |
