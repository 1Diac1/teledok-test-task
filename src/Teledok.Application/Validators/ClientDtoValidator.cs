using FluentValidation;
using Teledok.Application.Dtos;
using Teledok.Domain.Entities;
using Teledok.Infrastructure.Abstractions.Repositories;

namespace Teledok.Application.Validators;

public class ClientDtoValidator : AbstractValidator<ClientDto>
{
    public ClientDtoValidator()
    {
        RuleFor(client => client.INN)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .Length(10, 12).WithMessage("{PropertyName} must be between 10 and 12 characters long.")
            .Matches(@"^\d+$").WithMessage("{PropertyName} must contain only digits.");

        RuleFor(client => client.Name)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .Length(2, 50).WithMessage("{PropertyName} must be between 2 and 50 characters long.");

        RuleFor(client => client.ClientType)
            .IsInEnum().WithMessage("{PropertyName} must be a valid enum value.");
    }
}