using FluentValidation;

namespace Application.Comments.Commands.DeleteCommentCommand
{
    public class DeleteCommentCommandValidator : AbstractValidator<DeleteCommentCommand>
    {
        public DeleteCommentCommandValidator()
        {
            RuleFor(x => x.PostId)
                .NotEmpty()
                .GreaterThanOrEqualTo(1);
            RuleFor(x => x.CommentId)
                .NotEmpty()
                .GreaterThanOrEqualTo(1);
        }
    }
}