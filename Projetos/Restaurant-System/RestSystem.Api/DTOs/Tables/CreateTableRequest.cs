using RestSystem.Api.Models.Enums;

namespace RestSystem.Api.DTOs.Tables;

public record CreateTableRequest(int Number, int Capacity, TableStatus Status);