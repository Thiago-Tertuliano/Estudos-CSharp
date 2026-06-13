using FinControl.Api.Data;
using FinControl.Api.DTOs;
using FinControl.Api.Models;
using FinControl.Api.Services;
using Microsoft.EntityFrameworkCore;

namespace FinControl.Tests.Services;

public class ExpenseServiceTests
{
    private AppDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new AppDbContext(options);
    }

    private readonly Guid _userId = Guid.NewGuid();

    [Fact]
    public async Task Create_WithValidData_ReturnsExpense()
    {
        var context = CreateContext();
        var service = new ExpenseService(context);
        var request = new ExpenseRequest("Food", 50, "Meals", DateTime.UtcNow);

        var result = await service.CreateAsync(request, _userId);

        Assert.Equal("Food", result.Description);
        Assert.Equal(50, result.Amount);
    }

    [Fact]
    public async Task Create_WithAmountZero_ThrowsArgumentException()
    {
        var context = CreateContext();
        var service = new ExpenseService(context);
        var request = new ExpenseRequest("Food", 0, "Meals", DateTime.UtcNow);

        await Assert.ThrowsAsync<ArgumentException>(() => service.CreateAsync(request, _userId));
    }

    [Fact]
    public async Task GetById_OwnExpense_ReturnsExpense()
    {
        var context = CreateContext();
        var expense = new Expense { UserId = _userId, Description = "Test", Amount = 10, Category = "X", Date = DateTime.UtcNow };
        context.Expenses.Add(expense);
        await context.SaveChangesAsync();

        var service = new ExpenseService(context);
        var result = await service.GetByIdAsync(expense.Id, _userId);

        Assert.NotNull(result);
        Assert.Equal(expense.Id, result.Id);
    }

    [Fact]
    public async Task GetById_OtherUserExpense_ThrowsUnauthorizedAccessException()
    {
        var context = CreateContext();
        var expense = new Expense { UserId = Guid.NewGuid(), Description = "Test", Amount = 10, Category = "X", Date = DateTime.UtcNow };
        context.Expenses.Add(expense);
        await context.SaveChangesAsync();

        var service = new ExpenseService(context);

        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => service.GetByIdAsync(expense.Id, _userId));
    }

    [Fact]
    public async Task Delete_OwnExpense_RemovesExpense()
    {
        var context = CreateContext();
        var expense = new Expense { UserId = _userId, Description = "Test", Amount = 10, Category = "X", Date = DateTime.UtcNow };
        context.Expenses.Add(expense);
        await context.SaveChangesAsync();

        var service = new ExpenseService(context);
        await service.DeleteAsync(expense.Id, _userId);

        Assert.Empty(context.Expenses);
    }

    [Fact]
    public async Task Delete_OtherUserExpense_ThrowsUnauthorizedAccessException()
    {
        var context = CreateContext();
        var expense = new Expense { UserId = Guid.NewGuid(), Description = "Test", Amount = 10, Category = "X", Date = DateTime.UtcNow };
        context.Expenses.Add(expense);
        await context.SaveChangesAsync();

        var service = new ExpenseService(context);

        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => service.DeleteAsync(expense.Id, _userId));
    }

    [Fact]
    public async Task Delete_NonExistentExpense_ThrowsKeyNotFoundException()
    {
        var context = CreateContext();
        var service = new ExpenseService(context);

        await Assert.ThrowsAsync<KeyNotFoundException>(() => service.DeleteAsync(Guid.NewGuid(), _userId));
    }
}