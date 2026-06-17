using RestSystem.Api.Models.Enums;

namespace RestSystem.Api.DTOs.Reservations;

public record UpdateReservationStatusRequest(ReservationStatus status);