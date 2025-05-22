using FluentValidation;
using Projetize.Api.DTOs.User;

namespace Projetize.Api.Validators
{
    public class UserUpdateDTOValidator : AbstractValidator<UserUpdateDTO>
    {
        public UserUpdateDTOValidator()
        {
            RuleFor(u => u.Name).NotEmpty().WithMessage(ValidatorErrorMessages.NonEmptyName).MaximumLength(50);
            RuleFor(u => u.UserName).NotEmpty().WithMessage(ValidatorErrorMessages.NonEmptyName).MaximumLength(50);
            RuleFor(u => u.Email).NotEmpty().WithMessage(ValidatorErrorMessages.NonEmptyName).MaximumLength(120);
        }

    }
}
