using RestSystem.Api.Models.Enums;

namespace RestSystem.Api.DTOs.Tables;

public record TableResponse(Guid Id, int Number, int Capacity, TableStatus Status);