using RestSystem.Api.Models.Enums;

namespace RestSystem.Api.DTOs.Orders;

public record OrderSummaryResponse(Guid Id, int TableNumber, string WaiterName, StatusOrder Status, decimal TotalPrice, DateTime OpenedAt);