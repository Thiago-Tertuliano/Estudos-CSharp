using RestSystem.Api.Models.Enums;

namespace RestSystem.Api.DTOs.Reservations;

public record CreateReservationRequest(Guid TableId, string CustomerName, string CustomerPhone, DateTime ReservationDate, int PartySize);
