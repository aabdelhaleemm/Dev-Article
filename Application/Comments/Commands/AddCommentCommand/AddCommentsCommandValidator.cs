using FluentValidation;

namespace Application.Comments.Commands.AddCommentCommand
{
    public class AddCommentsCommandValidator : AbstractValidator<AddCommentsCommand>
    {
        public AddCommentsCommandValidator()
        {
            RuleFor(x => x.Content)
                .NotEmpty();
            RuleFor(x => x.PostId)
                .NotEmpty()
                .GreaterThanOrEqualTo(1);
            
        }
    }
}