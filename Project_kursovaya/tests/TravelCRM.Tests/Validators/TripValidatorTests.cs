using FluentAssertions;
using FluentValidation.TestHelper;
using TravelCRM.Application.Dtos;
using TravelCRM.Application.Validators;
using Xunit;

namespace TravelCRM.Tests.Validators;

public class TripValidatorTests
{
    private readonly TripCreateDtoValidator _validator = new();

    [Fact]
    public void Validate_EmptyDepartureCity_Fails()
    {
        var dto = new TripCreateDto
        {
            TouristId = 1,
            ArrivalCity = "Анталия",
            StartDate = DateTime.Today.AddDays(1),
            EndDate = DateTime.Today.AddDays(7),
        };
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.DepartureCity);
    }

    [Fact]
    public void Validate_EndBeforeStart_Fails()
    {
        var dto = new TripCreateDto
        {
            TouristId = 1,
            DepartureCity = "Москва",
            ArrivalCity = "Анталия",
            StartDate = DateTime.Today.AddDays(7),
            EndDate = DateTime.Today.AddDays(1),
        };
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.StartDate);
    }

    [Fact]
    public void Validate_TouristIdZero_Fails()
    {
        var dto = new TripCreateDto
        {
            TouristId = 0,
            DepartureCity = "Москва",
            ArrivalCity = "Анталия",
            StartDate = DateTime.Today.AddDays(1),
            EndDate = DateTime.Today.AddDays(7),
        };
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.TouristId);
    }

    [Fact]
    public void Validate_AllValid_Passes()
    {
        var dto = new TripCreateDto
        {
            TouristId = 5,
            DepartureCity = "Москва",
            ArrivalCity = "Анталия",
            HotelName = "Rixos Premium",
            StartDate = DateTime.Today.AddDays(1),
            EndDate = DateTime.Today.AddDays(7),
        };
        var result = _validator.TestValidate(dto);
        result.IsValid.Should().BeTrue();
    }
}
