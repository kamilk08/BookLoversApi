using BookLovers.Bookcases.Infrastructure.Queries.Shelves;
using FluentValidation;

namespace BookLovers.Bookcases.Infrastructure.Validators.Queries
{
    internal class MultipleShelfRecordsQueryValidator : AbstractValidator<MultipleShelfRecordsQuery>
    {
        public MultipleShelfRecordsQueryValidator()
        {
            RuleFor(p => p.BookIds).NotNull().WithMessage("Invalid book ids.");
        }
    }
}