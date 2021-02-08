using FluentValidation;

namespace Application.Users.Queries.GetUserById
{
    public class GetUsersByIdQueryValidator : AbstractValidator<GetUsersByIdQuery>
    {
        public GetUsersByIdQueryValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id must be provided")
                .GreaterThanOrEqualTo(1).WithMessage("Invalid Id");
        }
    }
}