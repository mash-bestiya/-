using FluentValidation;
using TravelCRM.Domain.Models;

namespace TravelCRM.Application.Validators;

/// <summary>
/// Валидатор данных поездки.
/// </summary>
public class TripValidator : AbstractValidator<Trip>
{
    public TripValidator()
    {
        RuleFor(t => t.DepartureCity)
            .NotEmpty().WithMessage("Город отправления обязателен")
            .MaximumLength(80);

        RuleFor(t => t.ArrivalCity)
            .NotEmpty().WithMessage("Город прибытия обязателен")
            .MaximumLength(80);

        RuleFor(t => t.HotelName)
            .MaximumLength(120);

        RuleFor(t => t.StartDate)
            .LessThan(t => t.EndDate)
                .WithMessage("Дата начала поездки должна быть раньше даты окончания");

        RuleFor(t => t.TouristId)
            .GreaterThan(0).WithMessage("Турист должен быть выбран");
    }
}
