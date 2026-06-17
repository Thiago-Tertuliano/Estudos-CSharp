using  RestSystem.Api.Models.Enums;

namespace RestSystem.Api.Models.Entities;

public class Reservation
{
    public Guid Id { get; set; }
    public Guid TableId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string CustomerPhone { get; set; } = string.Empty;
    public DateTime ReservationDate { get; set; }
    public int PartySize { get; set; }
    public ReservationStatus Status { get; set; } = ReservationStatus.Pending;
    public DateTime CreatedAt { get; set; }
}