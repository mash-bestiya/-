using FluentValidation;
using TravelCRM.Domain.Models;

namespace TravelCRM.Application.Validators;

/// <summary>
/// Валидатор данных туриста.
/// Проверяет обязательные поля и форматы перед сохранением в БД.
/// </summary>
public class TouristValidator : AbstractValidator<Tourist>
{
    public TouristValidator()
    {
        RuleFor(t => t.LastName)
            .NotEmpty().WithMessage("Фамилия обязательна для заполнения")
            .MaximumLength(60).WithMessage("Фамилия не должна превышать 60 символов");

        RuleFor(t => t.FirstName)
            .NotEmpty().WithMessage("Имя обязательно для заполнения")
            .MaximumLength(60).WithMessage("Имя не должно превышать 60 символов");

        RuleFor(t => t.MiddleName)
            .MaximumLength(60).WithMessage("Отчество не должно превышать 60 символов");

        RuleFor(t => t.Phone)
            .NotEmpty().WithMessage("Телефон обязателен")
            .Matches(@"^\+?[0-9]{10,15}$")
                .WithMessage("Телефон должен содержать 10-15 цифр, опционально с '+'");

        RuleFor(t => t.PassportInfo)
            .MaximumLength(200);

        RuleFor(t => t.IntPassportInfo)
            .MaximumLength(200);

        RuleFor(t => t.VisaInfo)
            .MaximumLength(500);
    }
}
