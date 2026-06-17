using FinControl.Api.Data;
using FinControl.Api.DTOs;
using FinControl.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace FinControl.Api.Services;

public class ExpenseService(AppDbContext context) : IExpenseService
{
    public async Task<List<ExpenseResponse>> GetAllAsync(Guid userId)
    {
        return await context.Expenses
            .Where(e => e.UserId == userId)
            .OrderByDescending(e => e.Date)
            .Select(e => new ExpenseResponse(e.Id, e.Description, e.Amount, e.Category, e.Date, e.CreatedAt))
            .ToListAsync();
    }

    public async Task<ExpenseResponse> GetByIdAsync(Guid id, Guid userId)
    {
        var expense = await context.Expenses.FirstOrDefaultAsync(e => e.Id == id)
            ?? throw new KeyNotFoundException("Expense not found.");

        if (expense.UserId != userId)
            throw new UnauthorizedAccessException("Access denied.");

        return new ExpenseResponse(expense.Id, expense.Description, expense.Amount, expense.Category, expense.Date, expense.CreatedAt);
    }

    public async Task<ExpenseResponse> CreateAsync(ExpenseRequest request, Guid userId)
    {
        if (string.IsNullOrWhiteSpace(request.Description) || request.Description.Length > 200)
            throw new ArgumentException("Description is required (max 200 characters).");
        if (request.Amount <= 0)
            throw new ArgumentException("Amount must be greater than zero.");
        if (string.IsNullOrWhiteSpace(request.Category))
            throw new ArgumentException("Category is required.");

        var expense = new Expense
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Description = request.Description,
            Amount = request.Amount,
            Category = request.Category,
            Date = request.Date,
            CreatedAt = DateTime.UtcNow
        };

        context.Expenses.Add(expense);
        await context.SaveChangesAsync();

        return new ExpenseResponse(expense.Id, expense.Description, expense.Amount, expense.Category, expense.Date, expense.CreatedAt);
    }

    public async Task<ExpenseResponse> UpdateAsync(Guid id, ExpenseRequest request, Guid userId)
    {
        var expense = await context.Expenses.FirstOrDefaultAsync(e => e.Id == id)
            ?? throw new KeyNotFoundException("Expense not found.");

        if (expense.UserId != userId)
            throw new UnauthorizedAccessException("Access denied.");

        if (string.IsNullOrWhiteSpace(request.Description) || request.Description.Length > 200)
            throw new ArgumentException("Description is required (max 200 characters).");
        if (request.Amount <= 0)
            throw new ArgumentException("Amount must be greater than zero.");
        if (string.IsNullOrWhiteSpace(request.Category))
            throw new ArgumentException("Category is required.");

        expense.Description = request.Description;
        expense.Amount = request.Amount;
        expense.Category = request.Category;
        expense.Date = request.Date;

        await context.SaveChangesAsync();

        return new ExpenseResponse(expense.Id, expense.Description, expense.Amount, expense.Category, expense.Date, expense.CreatedAt);
    }

    public async Task DeleteAsync(Guid id, Guid userId)
    {
        var expense = await context.Expenses.FirstOrDefaultAsync(e => e.Id == id)
            ?? throw new KeyNotFoundException("Expense not found.");

        if (expense.UserId != userId)
            throw new UnauthorizedAccessException("Access denied.");

        context.Expenses.Remove(expense);
        await context.SaveChangesAsync();
    }
}