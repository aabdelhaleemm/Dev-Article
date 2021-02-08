using FluentValidation;

namespace Application.Users.Commands.AddProfilePhotoCommand
{
    public class AddProfilePhotoCommandValidator : AbstractValidator<AddProfilePhotoCommand>
    {
        public AddProfilePhotoCommandValidator()
        {
            RuleFor(x => x.File)
                .NotNull()
                .NotEmpty().WithMessage("Cant find the photo");
            RuleFor(x => x.UserId)
                .NotNull()
                .NotEmpty()
                .GreaterThanOrEqualTo(1);
        }
        
    }
}