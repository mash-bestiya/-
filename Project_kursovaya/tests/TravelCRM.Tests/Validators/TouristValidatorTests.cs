using FluentAssertions;
using FluentValidation.TestHelper;
using TravelCRM.Application.Dtos;
using TravelCRM.Application.Validators;
using Xunit;

namespace TravelCRM.Tests.Validators;

public class TouristValidatorTests
{
    private readonly TouristCreateDtoValidator _validator = new();

    [Fact]
    public void Validate_EmptyLastName_Fails()
    {
        var dto = new TouristCreateDto { FirstName = "Иван", Phone = "+79001112233" };
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.LastName);
    }

    [Fact]
    public void Validate_EmptyFirstName_Fails()
    {
        var dto = new TouristCreateDto { LastName = "Иванов", Phone = "+79001112233" };
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.FirstName);
    }

    [Theory]
    [InlineData("abc")]
    [InlineData("12345")]
    [InlineData("+1234")]
    [InlineData("+790abc11223")]
    public void Validate_BadPhone_Fails(string phone)
    {
        var dto = new TouristCreateDto { LastName = "А", FirstName = "Б", Phone = phone };
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.Phone);
    }

    [Theory]
    [InlineData("+79001112233")]
    [InlineData("79001112233")]
    [InlineData("9001112233")]
    public void Validate_GoodPhone_Passes(string phone)
    {
        var dto = new TouristCreateDto { LastName = "Иванов", FirstName = "Иван", Phone = phone };
        var result = _validator.TestValidate(dto);
        result.ShouldNotHaveValidationErrorFor(x => x.Phone);
    }

    [Fact]
    public void Validate_AllValid_Passes()
    {
        var dto = new TouristCreateDto
        {
            LastName = "Иванов",
            FirstName = "Иван",
            Phone = "+79001112233",
            PassportInfo = "4500 123456"
        };
        var result = _validator.TestValidate(dto);
        result.IsValid.Should().BeTrue();
    }
}
