# E-Commerce API

API REST de e-commerce com **Controllers**, **Repository Pattern** e **Service Layer**. Construída com .NET 10, Entity Framework Core e SQLite.

## Arquitetura

```
Controller (HTTP) → Service (regras de negócio) → Repository (EF Core) → SQLite
```

## Conceitos aplicados

- **Controller-based** — `[ApiController]`, `[Route("api/[controller]")]`, `ActionResult<T>`
- **Repository Pattern** — Interface genérica reaproveitada da Biblioteca
- **Service Layer** — Validação e regras de negócio
- **Enum** — `OrderStatus` (Pending, Paid, Shipped, Delivered, Cancelled)
- **Relacionamentos** — Order → OrderItem → Product (1:N)
- **Controle de estoque** — Ao criar pedido, estoque é verificado e reduzido
- **Cálculo de totais** — Total do pedido calculado a partir dos itens

## Regras de negócio

| Regra | Onde |
|-------|------|
| Nome do produto obrigatório | ProductService |
| Preço > 0 | ProductService |
| Estoque >= 0 | ProductService |
| Categoria deve existir | ProductService |
| Nome da categoria obrigatório | CategoryService |
| **Não deletar categoria com produtos** | CategoryService → `Conflict` (409) |
| Nome do cliente obrigatório | OrderService |
| **Mínimo 1 item por pedido** | OrderService |
| **Produto deve existir** | OrderService |
| **Estoque suficiente** | OrderService → `Conflict` (409) |
| **Reduzir estoque ao criar pedido** | OrderService |
| **Total calculado automaticamente** | OrderService → `Quantity * UnitPrice` |

## Como rodar

```bash
cd "Projetos/E-Commerce"
dotnet run
```

## Endpoints

### Product
| Método | Rota | Descrição |
|--------|------|-----------|
| GET | `/api/product` | Listar produtos |
| GET | `/api/product/{id}` | Buscar produto |
| POST | `/api/product` | Criar produto |
| PUT | `/api/product/{id}` | Atualizar produto |
| DELETE | `/api/product/{id}` | Deletar produto |

### Category
| Método | Rota | Descrição |
|--------|------|-----------|
| GET | `/api/category` | Listar categorias |
| GET | `/api/category/{id}` | Buscar categoria |
| POST | `/api/category` | Criar categoria |
| PUT | `/api/category/{id}` | Atualizar categoria |
| DELETE | `/api/category/{id}` | Deletar categoria |

### Order
| Método | Rota | Descrição |
|--------|------|-----------|
| GET | `/api/order` | Listar pedidos |
| GET | `/api/order/{id}` | Buscar pedido |
| POST | `/api/order` | Criar pedido |
| PUT | `/api/order/{id}` | Atualizar pedido |
| DELETE | `/api/order/{id}` | Deletar pedido |

## Exemplo de criação de pedido

```json
{
  "customerName": "João Silva",
  "items": [
    { "productId": "d3e3b849-3b3f-4b2b-8b3b-3b3f4b2b8b3b", "quantity": 2 }
  ]
}
```
