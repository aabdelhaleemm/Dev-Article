using FluentValidation;

namespace Application.Users.Commands.SignInCommand
{
    public class SignInCommandValidator : AbstractValidator<SignInCommand>
    {
        public SignInCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email must be provided")
                .EmailAddress().WithMessage("Email not valid!");
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password should be provided");
        }
    }
}