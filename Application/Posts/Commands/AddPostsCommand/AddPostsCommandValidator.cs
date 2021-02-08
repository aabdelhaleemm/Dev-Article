using FluentValidation;

namespace Application.Posts.Commands.AddPostsCommand
{
    public class AddPostsCommandValidator : AbstractValidator<AddPostsCommand>
    {
        public AddPostsCommandValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title must be provided");
            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("Body must be provided")
                .MinimumLength(20);
            
        }
    }
}