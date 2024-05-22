using FluentValidation;
using Teledok.Application.Dtos;

namespace Teledok.Application.Validators;

public class ClientDtoValidator : AbstractValidator<ClientDto>
{
    public ClientDtoValidator()
    {
        RuleFor(client => client.INN)
            .NotEmpty().WithMessage("INN is required.")
            .Length(10, 12).WithMessage("INN must be between 10 and 12 characters long.")
            .Matches(@"^\d+$").WithMessage("INN must contain only digits.");
        
        RuleFor(client => client.Name)
            .NotEmpty().WithMessage("Name is required.")
            .Length(2, 50).WithMessage("Name must be between 2 and 50 characters long.");

        RuleFor(client => client.ClientType)
            .IsInEnum().WithMessage("ClientType must be a valid enum value.");
    }
}