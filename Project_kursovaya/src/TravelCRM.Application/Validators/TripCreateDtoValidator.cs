using FluentValidation;
using TravelCRM.Application.Dtos;

namespace TravelCRM.Application.Validators;

/// <summary>
/// Валидатор формы создания поездки.
/// </summary>
public class TripCreateDtoValidator : AbstractValidator<TripCreateDto>
{
    public TripCreateDtoValidator()
    {
        RuleFor(t => t.DepartureCity)
            .NotEmpty().WithMessage("Укажите город отправления")
            .MaximumLength(80);

        RuleFor(t => t.ArrivalCity)
            .NotEmpty().WithMessage("Укажите город прибытия")
            .MaximumLength(80);

        RuleFor(t => t.HotelName).MaximumLength(120);

        RuleFor(t => t.StartDate)
            .LessThan(t => t.EndDate)
                .WithMessage("Дата начала должна быть раньше даты окончания");

        RuleFor(t => t.TouristId)
            .GreaterThan(0).WithMessage("Укажите туриста");
    }
}
