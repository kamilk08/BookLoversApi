using BookLovers.Bookcases.Infrastructure.Queries.Shelves;
using FluentValidation;

namespace BookLovers.Bookcases.Infrastructure.Validators.Queries
{
    internal class ShelfRecordQueryValidator : AbstractValidator<ShelfRecordQuery>
    {
        public ShelfRecordQueryValidator()
        {
            this.RuleFor(p => p.BookId).GreaterThanOrEqualTo(0);
            this.RuleFor(p => p.ShelfId).GreaterThanOrEqualTo(0);
        }
    }
}