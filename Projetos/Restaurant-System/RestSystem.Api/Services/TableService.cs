using RestSystem.Api.Data;
using RestSystem.Api.DTOs.Tables;
using Microsoft.EntityFrameworkCore;
using RestSystem.Api.Models.Enums;
using RestSystem.Api.Models.Entities;

namespace RestSystem.Api.Services;

public class TableService(AppDbContext context) : ITableService
{
    public async Task<List<TableResponse>> GetAllAsync()
    {
        return await context.Tables
            .Select(t => new TableResponse(t.Id, t.Number, t.Capacity, t.Status))
            .ToListAsync();
    }

    public async Task<TableResponse> GetByIdAsync(Guid Id)
    {
        var table = await context.Tables.FirstOrDefaultAsync(t => t.Id == Id)
            ?? throw new KeyNotFoundException("Table not found.");

        return new TableResponse(table.Id, table.Number, table.Capacity, table.Status);
    }

    public async Task<TableResponse> CreateAsync(CreateTableRequest request)
    {
        if (request.Number <= 0)
            throw new ArgumentException("Number not specified.");
        if (request.Capacity <= 0)
            throw new ArgumentException("Capacity must be greater than zero.");

        var table = new Table
        {
            Id = Guid.NewGuid(),
            Number = request.Number,
            Capacity = request.Capacity,
            Status = TableStatus.Available,
        };

        context.Tables.Add(table);
        await context.SaveChangesAsync();

        return new TableResponse(table.Id, table.Number, table.Capacity, table.Status);
    }

    public async Task<TableResponse> UpdateAsync(Guid Id, UpdateTableRequest request)
    {
        var table = await context.Tables.FirstOrDefaultAsync(t => t.Id == Id)
            ?? throw new KeyNotFoundException("Table not found.");

        if (request.Number <= 0)
            throw new ArgumentException("Number not specified.");
        if (request.Capacity <= 0)
            throw new ArgumentException("Capacity must be greater than zero.");

        table.Number = request.Number;
        table.Capacity = request.Capacity;
        table.Status = request.Status;

        await context.SaveChangesAsync();

        return new TableResponse(table.Id, table.Number, table.Capacity, table.Status);
    }

    public async Task DeleteAsync(Guid Id)
    {
        var table = await context.Tables.FirstOrDefaultAsync(t => t.Id == Id)
            ?? throw new KeyNotFoundException("Table not found.");

        context.Tables.Remove(table);
        await context.SaveChangesAsync();
    }
}