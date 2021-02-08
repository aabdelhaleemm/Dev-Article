using FluentValidation;

namespace Application.Posts.Commands.UpdatePostsCommand
{
    public class UpdatePostsCommandValidator : AbstractValidator<UpdatePostsCommand>
    {
        public UpdatePostsCommandValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title must be provided");
            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("Body must be provided")
                .MinimumLength(20);
            
        }
    }
}