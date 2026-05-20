using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using TravelCRM.Domain.Models;
using TravelCRM.Infrastructure.Data;
using TravelCRM.Infrastructure.Repositories;
using Xunit;

namespace TravelCRM.Tests.Repositories;

public class TouristRepositoryTests
{
    private static AppDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        return new AppDbContext(options);
    }

    [Fact]
    public async Task AddAsync_SavesTourist()
    {
        await using var ctx = CreateContext();
        var repo = new TouristRepository(ctx);

        await repo.AddAsync(new Tourist
        {
            LastName = "Иванов",
            FirstName = "Иван",
            Phone = "+79001112233",
        });
        await repo.SaveChangesAsync();

        (await ctx.Tourists.CountAsync()).Should().Be(1);
    }

    [Fact]
    public async Task GetByPhoneAsync_FindsTourist()
    {
        await using var ctx = CreateContext();
        ctx.Tourists.Add(new Tourist { LastName = "Иванов", FirstName = "Иван", Phone = "+79991112233" });
        await ctx.SaveChangesAsync();

        var repo = new TouristRepository(ctx);
        var found = await repo.GetByPhoneAsync("+79991112233");

        found.Should().NotBeNull();
        found!.LastName.Should().Be("Иванов");
    }

    [Fact]
    public async Task SearchAsync_ByLastName_ReturnsMatches()
    {
        await using var ctx = CreateContext();
        ctx.Tourists.AddRange(
            new Tourist { LastName = "Пушкин", FirstName = "Александр", Phone = "+79001112233" },
            new Tourist { LastName = "Толстой", FirstName = "Лев", Phone = "+79002223344" });
        await ctx.SaveChangesAsync();

        var repo = new TouristRepository(ctx);
        var results = await repo.SearchAsync("Пушкин");

        results.Should().HaveCount(1);
        results[0].FirstName.Should().Be("Александр");
    }

    [Fact]
    public async Task GetByPhoneAsync_NoSuchTourist_ReturnsNull()
    {
        await using var ctx = CreateContext();
        var repo = new TouristRepository(ctx);

        var found = await repo.GetByPhoneAsync("+70000000000");

        found.Should().BeNull();
    }
}
