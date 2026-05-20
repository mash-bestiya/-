using FluentValidation;
using TravelCRM.Application.Dtos;

namespace TravelCRM.Application.Validators;

/// <summary>
/// Валидатор формы создания туриста.
/// </summary>
public class TouristCreateDtoValidator : AbstractValidator<TouristCreateDto>
{
    public TouristCreateDtoValidator()
    {
        RuleFor(t => t.LastName)
            .NotEmpty().WithMessage("Укажите фамилию")
            .MaximumLength(60).WithMessage("Фамилия не должна превышать 60 символов");

        RuleFor(t => t.FirstName)
            .NotEmpty().WithMessage("Укажите имя")
            .MaximumLength(60).WithMessage("Имя не должно превышать 60 символов");

        RuleFor(t => t.MiddleName)
            .MaximumLength(60);

        RuleFor(t => t.Phone)
            .NotEmpty().WithMessage("Укажите телефон")
            .Matches(@"^\+?[0-9]{10,15}$")
                .WithMessage("Телефон должен содержать 10-15 цифр, опционально с '+'");

        RuleFor(t => t.PassportInfo).MaximumLength(200);
        RuleFor(t => t.IntPassportInfo).MaximumLength(200);
        RuleFor(t => t.VisaInfo).MaximumLength(500);
    }
}
