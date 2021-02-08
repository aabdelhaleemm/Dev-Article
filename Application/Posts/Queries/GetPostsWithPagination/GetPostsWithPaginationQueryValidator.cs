using FluentValidation;

namespace Application.Posts.Queries.GetPostsWithPagination
{
    public class GetPostsWithPaginationQueryValidator : AbstractValidator<GetPostsWithPaginationQuery>
    {
        public GetPostsWithPaginationQueryValidator()
        {
            RuleFor(x => x.PageSize)
                .NotEmpty()
                .GreaterThanOrEqualTo(1);
            RuleFor(x => x.PageNumber)
                .NotEmpty()
                .GreaterThanOrEqualTo(1);
        }
    }
}