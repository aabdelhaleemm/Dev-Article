using FluentValidation;

namespace Application.Likes.Commands.AddLikesCommand
{
    public class AddLikesCommandValidator : AbstractValidator<AddLikesCommand>
    {
        public AddLikesCommandValidator()
        {
            RuleFor(x => x.PostId)
                .NotEmpty()
                .GreaterThanOrEqualTo(1);
            
        }
    }
}