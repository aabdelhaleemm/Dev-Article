using FluentValidation;

namespace Application.Posts.Commands.DeletePostsCommand
{
    public class DeletePostsCommandValidator : AbstractValidator<DeletePostsCommand>
    {
        public DeletePostsCommandValidator()
        {
            RuleFor(x => x.PostId)
                .NotEmpty()
                .GreaterThanOrEqualTo(1);
            
        }
    }
}