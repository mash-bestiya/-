using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TravelCRM.Domain.Models;
using TravelCRM.Infrastructure.Data;

namespace TravelCRM.Infrastructure.Repositories;

/// <summary>
/// Реализация репозитория туристов через Entity Framework Core.
/// </summary>
public class TouristRepository : ITouristRepository
{
    private readonly AppDbContext _context;

    public TouristRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Tourist?> GetByPhoneAsync(string phone)
    {
        return await _context.Tourists
            .FirstOrDefaultAsync(t => t.Phone == phone);
    }

    public async Task<Tourist?> GetByIdWithDetailsAsync(int id)
    {
        return await _context.Tourists
            .Include(t => t.Trips)
                .ThenInclude(tr => tr.Documents)
            .Include(t => t.LinkedAsMain)
                .ThenInclude(lt => lt.LinkedTouristRef)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task AddAsync(Tourist tourist)
    {
        await _context.Tourists.AddAsync(tourist);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<List<Tourist>> SearchAsync(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
            return new List<Tourist>();

        return await _context.Tourists
            .Where(t => t.LastName.Contains(query) || 
                        t.FirstName.Contains(query) || 
                        t.Phone.Contains(query))
            .Take(10) // Ограничиваем выдачу
            .ToListAsync();
    }
}