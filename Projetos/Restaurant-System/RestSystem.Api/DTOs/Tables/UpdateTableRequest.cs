using RestSystem.Api.Models.Enums;

namespace RestSystem.Api.DTOs.Tables;

public record UpdateTableRequest(int Number, int Capacity, TableStatus Status);