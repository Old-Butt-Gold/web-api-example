using Application.Commands;
using FluentValidation;

namespace Application.Validators;

public sealed class CreateCompanyCommandValidator :
    AbstractValidator<CreateCompanyCommand>
{
    public CreateCompanyCommandValidator()
    {
        RuleFor(c => c.Company.Name).NotEmpty().MaximumLength(60);
        RuleFor(c => c.Company.Address).NotEmpty().MaximumLength(60);
    }
}