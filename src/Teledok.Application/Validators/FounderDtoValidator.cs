using FluentValidation;
using Teledok.Application.Dtos;

namespace Teledok.Application.Validators;

public class FounderDtoValidator : AbstractValidator<FounderDto>
{
    public FounderDtoValidator()
    {
        RuleFor(client => client.INN)
            .NotEmpty().WithMessage("INN is required.")
            .Length(10, 12).WithMessage("INN must be between 10 and 12 characters long.")
            .Matches(@"^\d+$").WithMessage("INN must contain only digits.");

        RuleFor(client => client.FirstName)
            .NotEmpty().WithMessage("FirstName is required.")
            .Length(2, 50).WithMessage("FirstName must be between 2 and 50 characters long.");

        RuleFor(client => client.LastName)
            .NotEmpty().WithMessage("LastName is required.")
            .Length(2, 50).WithMessage("LastName must be between 2 and 50 characters long.");

        RuleFor(client => client.PatronymicName)
            .Length(2, 50).When(client => !string.IsNullOrEmpty(client.PatronymicName))
            .WithMessage("PatronymicName must be between 2 and 50 characters long if provided.");
    }
}