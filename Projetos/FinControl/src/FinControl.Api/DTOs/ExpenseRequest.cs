namespace FinControl.Api.DTOs;

public record ExpenseRequest(string Description, decimal Amount, string Category, DateTime Date);