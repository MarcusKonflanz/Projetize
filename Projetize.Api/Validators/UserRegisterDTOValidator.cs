using FluentValidation;
using Projetize.Api.DTOs.User;

namespace Projetize.Api.Validators
{
    public class UserRegisterDTOValidator : AbstractValidator<UserRegisterDTO>
    {
        public UserRegisterDTOValidator()
        {
            RuleFor(u => u.Name).NotEmpty().WithMessage(ValidatorErrorMessages.NonEmptyName).MaximumLength(50).WithMessage(ValidatorErrorMessages.NameHighestLimit);
            RuleFor(u => u.Email).NotEmpty().WithMessage(ValidatorErrorMessages.NonEmptyEmail).MaximumLength(120).WithMessage(ValidatorErrorMessages.EmailHighestLimit).EmailAddress();
            RuleFor(u => u.UserName).NotEmpty().WithMessage(ValidatorErrorMessages.NonEmptyUserName).MaximumLength(25).WithMessage(ValidatorErrorMessages.UserNameHighestLimit);
            RuleFor(u => u.Password).NotEmpty().WithMessage(ValidatorErrorMessages.NonEmptyPassword).MinimumLength(4).WithMessage(ValidatorErrorMessages.PasswordLowerLimit)
                                                .MaximumLength(15).WithMessage(ValidatorErrorMessages.PasswordHighestLimit).Matches("[A-Z]").WithMessage(ValidatorErrorMessages.NonUpperCaseLetter)
                                                .Matches("[a-z]").WithMessage(ValidatorErrorMessages.NonLowerCaseLetter).Matches("[0-9]").WithMessage(ValidatorErrorMessages.NonLeastOneNumber)
                                                .Matches("[^a-zA-Z0-9]").WithMessage(ValidatorErrorMessages.NonLeastOneSpecialCharacter);
        
        
        
        }
    }
}
