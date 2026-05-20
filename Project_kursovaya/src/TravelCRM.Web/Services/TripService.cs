using Microsoft.EntityFrameworkCore;
using TravelCRM.Application.Dtos;
using TravelCRM.Domain.Models;
using TravelCRM.Infrastructure.Data;

namespace TravelCRM.Web.Services;

/// <summary>
/// Реализация сервиса поездок поверх <see cref="AppDbContext"/>.
/// </summary>
public class TripService : ITripService
{
    private readonly AppDbContext _context;

    public TripService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Trip?> GetTripDetailsAsync(int id)
    {
        return await _context.Trips
            .Include(t => t.Documents)
            .Include(t => t.Tourist)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task UpdatePaymentStatusAsync(int tripId, PaymentStatus status)
    {
        var trip = await _context.Trips.FindAsync(tripId);
        if (trip is null) return;
        trip.PaymentStatus = status;
        await _context.SaveChangesAsync();
    }

    public async Task<Trip> CreateAsync(TripCreateDto dto)
    {
        var trip = new Trip
        {
            TouristId = dto.TouristId,
            DepartureCity = dto.DepartureCity,
            ArrivalCity = dto.ArrivalCity,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            HotelName = dto.HotelName,
            PaymentStatus = dto.PaymentStatus,
        };
        _context.Trips.Add(trip);
        await _context.SaveChangesAsync();
        return trip;
    }

    public async Task DeleteAsync(int tripId)
    {
        var trip = await _context.Trips.FindAsync(tripId);
        if (trip is null) return;
        _context.Trips.Remove(trip);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Trip>> GetForTouristAsync(int touristId)
    {
        return await _context.Trips
            .Where(t => t.TouristId == touristId)
            .OrderByDescending(t => t.StartDate)
            .ToListAsync();
    }
}
