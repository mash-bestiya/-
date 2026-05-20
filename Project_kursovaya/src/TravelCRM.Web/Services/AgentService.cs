using FluentValidation;
using Microsoft.EntityFrameworkCore;
using TravelCRM.Application.Dtos;
using TravelCRM.Domain.Models;
using TravelCRM.Infrastructure.Data;

namespace TravelCRM.Web.Services;

/// <summary>
/// Сервис управления туристами от лица агента: создание, поиск, список.
/// </summary>
public class AgentService : IAgentService
{
    private readonly AppDbContext _context;
    private readonly IValidator<TouristCreateDto> _validator;

    public AgentService(AppDbContext context, IValidator<TouristCreateDto> validator)
    {
        _context = context;
        _validator = validator;
    }

    public async Task<Tourist> CreateTouristAsync(TouristCreateDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);
        await _validator.ValidateAndThrowAsync(dto);

        if (await _context.Tourists.AnyAsync(t => t.Phone == dto.Phone))
        {
            throw new InvalidOperationException(
                $"Турист с телефоном {dto.Phone} уже зарегистрирован.");
        }

        var tourist = new Tourist
        {
            LastName = dto.LastName.Trim(),
            FirstName = dto.FirstName.Trim(),
            MiddleName = dto.MiddleName.Trim(),
            Phone = dto.Phone.Trim(),
            EngFirstName = string.IsNullOrWhiteSpace(dto.EngFirstName) ? dto.FirstName.Trim() : dto.EngFirstName.Trim(),
            EngLastName = string.IsNullOrWhiteSpace(dto.EngLastName) ? dto.LastName.Trim() : dto.EngLastName.Trim(),
            PassportInfo = dto.PassportInfo.Trim(),
            IntPassportInfo = dto.IntPassportInfo.Trim(),
            VisaInfo = dto.VisaInfo.Trim(),
        };

        _context.Tourists.Add(tourist);
        await _context.SaveChangesAsync();
        return tourist;
    }

    public async Task<List<Tourist>> GetAllTouristsAsync()
    {
        return await _context.Tourists
            .OrderBy(t => t.LastName)
            .ThenBy(t => t.FirstName)
            .ToListAsync();
    }

    public async Task<List<Tourist>> SearchAsync(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
            return await GetAllTouristsAsync();

        var q = query.Trim();
        return await _context.Tourists
            .Where(t => t.LastName.Contains(q) ||
                        t.FirstName.Contains(q) ||
                        t.Phone.Contains(q))
            .OrderBy(t => t.LastName)
            .ToListAsync();
    }
}
