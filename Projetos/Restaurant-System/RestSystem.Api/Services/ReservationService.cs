using Microsoft.EntityFrameworkCore;
using RestSystem.Api.Data;
using RestSystem.Api.DTOs.Reservations;
using RestSystem.Api.Models.Entities;
using RestSystem.Api.Models.Enums;

namespace RestSystem.Api.Services;

public class ReservationService(AppDbContext context) : IReservationService
{
    public async Task<List<ReservationResponse>> GetAllAsync()
    {
        return await context.Reservations
            .Select(r => new ReservationResponse(r.Id, r.TableId, r.CustomerName, r.CustomerPhone, r.ReservationDate, r.PartySize, r.Status, r.CreatedAt))
            .ToListAsync();
    }

    public async Task<ReservationResponse> GetByIdAsync(Guid Id)
    {
        var reservation = await context.Reservations.FirstOrDefaultAsync(r => r.Id == Id)
            ?? throw new KeyNotFoundException("Reservation not found.");

        return new ReservationResponse(reservation.Id, reservation.TableId, reservation.CustomerName, reservation.CustomerPhone, reservation.ReservationDate, reservation.PartySize, reservation.Status, reservation.CreatedAt);
    }

    public async Task<ReservationResponse> CreateAsync(CreateReservationRequest request)
    {
        if (request.TableId == Guid.Empty)
            throw new ArgumentException("Table not specified.");
        if (string.IsNullOrWhiteSpace(request.CustomerName))
            throw new ArgumentException("Customer name is required.");
        if (request.PartySize <= 0)
            throw new ArgumentException("Party size must be greater than zero.");

        var reservation = new Reservation
        {
            Id = Guid.NewGuid(),
            TableId = request.TableId,
            CustomerName = request.CustomerName,
            CustomerPhone = request.CustomerPhone,
            ReservationDate = request.ReservationDate,
            PartySize = request.PartySize,
            Status = ReservationStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };

        context.Reservations.Add(reservation);
        await context.SaveChangesAsync();

        return new ReservationResponse(reservation.Id, reservation.TableId, reservation.CustomerName, reservation.CustomerPhone, reservation.ReservationDate, reservation.PartySize, reservation.Status, reservation.CreatedAt);
    }

    public async Task<ReservationResponse> UpdateAsync(Guid Id, UpdateReservationStatusRequest request)
    {
        var reservation = await context.Reservations.FirstOrDefaultAsync(r => r.Id == Id)
            ?? throw new KeyNotFoundException("Reservation not found.");

        reservation.Status = request.status;

        await context.SaveChangesAsync();

        return new ReservationResponse(reservation.Id, reservation.TableId, reservation.CustomerName, reservation.CustomerPhone, reservation.ReservationDate, reservation.PartySize, reservation.Status, reservation.CreatedAt);
    }

    public async Task DeleteAsync(Guid Id)
    {
        var reservation = await context.Reservations.FirstOrDefaultAsync(r => r.Id == Id)
            ?? throw new KeyNotFoundException("Reservation not found.");

        context.Reservations.Remove(reservation);
        await context.SaveChangesAsync();
    }
}
