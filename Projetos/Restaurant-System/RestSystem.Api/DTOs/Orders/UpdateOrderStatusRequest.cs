using RestSystem.Api.Models.Enums;

namespace RestSystem.Api.DTOs.Orders;

public record UpdateOrderStatusRequest(StatusOrder Status);