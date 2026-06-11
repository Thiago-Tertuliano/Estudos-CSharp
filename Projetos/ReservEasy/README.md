# ReservEasy — Booking System

Sistema de reservas com **Vertical Slices + CQRS + Domain Events + FastEndpoints**.

## 🏗️ Arquitetura

```
ReservEasy/
├── Common/           → Interfaces, Exceptions, Behaviors, Middleware
├── Domain/           → Entities, Enums, Domain Events
├── Data/             → DbContext, EF Configurations
└── Features/         → Vertical Slices
    ├── Properties/   → CRUD de propriedades
    ├── Guests/       → CRUD de hóspedes
    ├── Bookings/     → Reservas (criar, confirmar, cancelar)
    └── Payments/     → Pagamentos e estornos
```

### Pilha técnica

| Tecnologia | Uso |
|---|---|
| .NET 10 | Runtime |
| FastEndpoints | API (REPR pattern) |
| MediatR | CQRS + Domain Events |
| FluentValidation | Validação declarativa |
| EF Core + SQLite | Persistência |
| Serilog | Logging estruturado |

### Domain Events

```
BookingCreated   → BookingCreatedHandler (log)
BookingConfirmed → BookingConfirmedHandler (log)
BookingCancelled → BookingCancelledHandler (log + auto-refund)
```

## 📬 Endpoints

| Método | Rota | Descrição |
|---|---|---|
| POST | /api/properties | Criar propriedade |
| GET | /api/properties | Listar propriedades |
| GET | /api/properties/{id} | Detalhes da propriedade |
| PUT | /api/properties/{id} | Atualizar propriedade |
| POST | /api/guests | Registrar hóspede |
| GET | /api/guests | Listar hóspedes |
| GET | /api/guests/{id} | Detalhes do hóspede |
| POST | /api/bookings | Criar reserva |
| GET | /api/bookings | Listar reservas |
| GET | /api/bookings/{id} | Detalhes da reserva |
| POST | /api/bookings/{id}/confirm | Confirmar reserva |
| POST | /api/bookings/{id}/cancel | Cancelar reserva |
| POST | /api/payments/process | Processar pagamento |
| POST | /api/payments/{id}/refund | Estornar pagamento |

## Regras de Negócio

- Check-in deve ser futuro
- Mínimo 1 diária por reserva
- Propriedade não pode ter reservas com datas sobrepostas
- Máquina de estados: Pending → Confirmed → Completed | Cancelled
- Só é possível pagar reservas pendentes
- Cancelamento de reserva paga gera estorno automático
- Email de hóspede deve ser único

## 🚀 Como rodar

```powershell
dotnet restore
dotnet run --project src/ReservEasy.Api
```

Acesse `https://localhost:7001`.
