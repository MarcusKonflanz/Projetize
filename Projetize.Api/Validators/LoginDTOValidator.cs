using FluentValidation;
using Projetize.Api.DTOs.Login;
using Projetize.Api.DTOs.User;

namespace Projetize.Api.Validators
{
    public class LoginDTOValidator : AbstractValidator<UserLoginDTO>
    {
        public LoginDTOValidator()
        {
            RuleFor(l => l.Login).NotEmpty().WithMessage(ValidatorErrorMessages.NonEmptyLogin);
            RuleFor(l => l.Password).NotEmpty().WithMessage(ValidatorErrorMessages.NonEmptyPassword);
        }
    }
}
