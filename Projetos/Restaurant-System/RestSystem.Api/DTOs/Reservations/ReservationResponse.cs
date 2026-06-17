using RestSystem.Api.Models.Enums;

namespace RestSystem.Api.DTOs.Reservations;

public record ReservationResponse(Guid Id, Guid TableId, string CustomerName, string CustomerPhone, DateTime ReservationDate, int PartySize, ReservationStatus Status, DateTime CreatedAt);