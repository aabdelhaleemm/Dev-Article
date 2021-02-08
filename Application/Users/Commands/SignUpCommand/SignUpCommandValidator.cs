using FluentValidation;

namespace Application.Users.Commands.SignUpCommand
{
    public class SignUpCommandValidator : AbstractValidator<SignUpCommand>
    {
        public SignUpCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email must be provided")
                .EmailAddress().WithMessage("Email not valid!");
            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(6).WithMessage("Password should be at least 6 char");
            
            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("Username must be provided");
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Username must be provided");
            

        }
    }
}