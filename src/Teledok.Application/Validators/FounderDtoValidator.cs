using FluentValidation;
using Teledok.Application.Dtos;

namespace Teledok.Application.Validators;

public class FounderDtoValidator : AbstractValidator<FounderDto>
{
    public FounderDtoValidator()
    {
        RuleFor(founder => founder.INN)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .Length(10, 12).WithMessage("{PropertyName} must be between 10 and 12 characters long.")
            .Matches(@"^\d+$").WithMessage("{PropertyName} must contain only digits.");

        RuleFor(founder => founder.FirstName)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .Length(2, 50).WithMessage("{PropertyName} must be between 2 and 50 characters long.");

        RuleFor(founder => founder.LastName)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .Length(2, 50).WithMessage("{PropertyName} must be between 2 and 50 characters long.");

        RuleFor(founder => founder.PatronymicName)
            .Length(2, 50).When(client => !string.IsNullOrEmpty(client.PatronymicName))
            .WithMessage("{PropertyName} must be between 2 and 50 characters long if provided.");
        
        RuleFor(founder => founder.ClientId)
            .GreaterThan(0).WithMessage("{PropertyName} must be a positive integer.");
    }
}