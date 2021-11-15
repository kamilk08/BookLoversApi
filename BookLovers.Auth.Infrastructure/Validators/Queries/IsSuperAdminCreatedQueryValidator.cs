using BookLovers.Auth.Infrastructure.Queries.Users;
using FluentValidation;

namespace BookLovers.Auth.Infrastructure.Validators.Queries
{
    internal class IsSuperAdminCreatedQueryValidator
        : AbstractValidator<IsSuperAdminCreatedQuery>
    {
    }
}