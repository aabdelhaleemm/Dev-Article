using FluentValidation;

namespace Application.Likes.Commands.DeleteLikesCommand
{
    public class DeleteLikesCommandValidator : AbstractValidator<DeleteLikesCommand>
    {
        public DeleteLikesCommandValidator()
        {
            RuleFor(x => x.PostId)
                .NotEmpty()
                .GreaterThanOrEqualTo(1);
           
        }
    }
}