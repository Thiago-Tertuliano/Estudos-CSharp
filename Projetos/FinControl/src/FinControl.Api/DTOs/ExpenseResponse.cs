namespace FinControl.Api.DTOs;

public record ExpenseResponse(Guid Id, string Description, decimal Amount, string Category, DateTime Date, DateTime CreatedAt);