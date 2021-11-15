using BookLovers.Readers.Infrastructure.Queries.Readers.Profiles;
using FluentValidation;

namespace BookLovers.Readers.Infrastructure.Validators.Queries.Readers
{
    internal class ReaderProfileQueryValidator : AbstractValidator<ReaderProfileQuery>
    {
        public ReaderProfileQueryValidator()
        {
            RuleFor(p => p.ReaderId).NotEmpty()
                .GreaterThan(byte.MinValue)
                .NotNull()
                .WithMessage("Invalid id");
        }
    }
}