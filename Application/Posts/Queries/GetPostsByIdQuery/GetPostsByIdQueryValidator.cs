using FluentValidation;

namespace Application.Posts.Queries.GetPostsByIdQuery
{
    public class GetPostsByIdQueryValidator : AbstractValidator<GetPostsByIdQuery>
    {
        public GetPostsByIdQueryValidator()
        {
            RuleFor(x => x.Id)
                .NotNull()
                .NotEmpty()
                .GreaterThanOrEqualTo(1).WithMessage("Invalid Id");
           
        }
    }
}