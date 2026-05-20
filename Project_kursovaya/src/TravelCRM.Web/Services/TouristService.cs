using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TravelCRM.Domain.Models;
using TravelCRM.Infrastructure.Repositories;

namespace TravelCRM.Web.Services;

/// <summary>
/// Сервис работы с туристами. Использует репозиторий для доступа к БД.
/// </summary>
public class TouristService : ITouristService
{
    private readonly ITouristRepository _repository;

    public TouristService(ITouristRepository repository)
    {
        _repository = repository;
    }

    public async Task<Tourist?> LoginAsync(string phone)
    {
        return await _repository.GetByPhoneAsync(phone);
    }

    public async Task<Tourist?> GetProfileAsync(int id)
    {
        return await _repository.GetByIdWithDetailsAsync(id);
    }

    public async Task CreateTouristAsync(Tourist tourist)
    {
        await _repository.AddAsync(tourist);
        await _repository.SaveChangesAsync();
    }

    public async Task<List<Tourist>> SearchAgentsAsync(string query)
    {
        return await _repository.SearchAsync(query);
    }
}