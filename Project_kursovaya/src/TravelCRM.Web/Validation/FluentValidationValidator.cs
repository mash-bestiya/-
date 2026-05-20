using FluentValidation;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.DependencyInjection;

namespace TravelCRM.Web.Validation;

/// <summary>
/// Компонент-адаптер, который подключает FluentValidation-валидаторы
/// к Blazor <see cref="EditContext"/>.
/// Размещайте внутри &lt;EditForm&gt; рядом с DataAnnotationsValidator.
/// </summary>
public class FluentValidationValidator : ComponentBase
{
    [CascadingParameter]
    private EditContext? CurrentEditContext { get; set; }

    [Inject]
    private IServiceProvider ServiceProvider { get; set; } = default!;

    private ValidationMessageStore? _messageStore;

    protected override void OnInitialized()
    {
        if (CurrentEditContext is null)
        {
            throw new InvalidOperationException(
                "FluentValidationValidator должен использоваться внутри EditForm.");
        }

        _messageStore = new ValidationMessageStore(CurrentEditContext);

        CurrentEditContext.OnValidationRequested +=
            (_, _) => ValidateModel(CurrentEditContext, _messageStore);

        CurrentEditContext.OnFieldChanged +=
            (_, e) => ValidateField(CurrentEditContext, _messageStore, e.FieldIdentifier);
    }

    private void ValidateModel(EditContext editContext, ValidationMessageStore messageStore)
    {
        var validator = GetValidator(editContext.Model.GetType());
        if (validator is null) return;

        var context = new ValidationContext<object>(editContext.Model);
        var result = validator.Validate(context);

        messageStore.Clear();
        foreach (var error in result.Errors)
        {
            messageStore.Add(editContext.Field(error.PropertyName), error.ErrorMessage);
        }

        editContext.NotifyValidationStateChanged();
    }

    private void ValidateField(EditContext editContext, ValidationMessageStore messageStore, FieldIdentifier fieldIdentifier)
    {
        var validator = GetValidator(editContext.Model.GetType());
        if (validator is null) return;

        var fieldName = fieldIdentifier.FieldName;
        var context = new ValidationContext<object>(editContext.Model);
        var result = validator.Validate(context);

        messageStore.Clear(fieldIdentifier);
        foreach (var error in result.Errors.Where(e => e.PropertyName == fieldName))
        {
            messageStore.Add(fieldIdentifier, error.ErrorMessage);
        }

        editContext.NotifyValidationStateChanged();
    }

    private IValidator? GetValidator(Type modelType)
    {
        var validatorType = typeof(IValidator<>).MakeGenericType(modelType);
        return ServiceProvider.GetService(validatorType) as IValidator;
    }
}
