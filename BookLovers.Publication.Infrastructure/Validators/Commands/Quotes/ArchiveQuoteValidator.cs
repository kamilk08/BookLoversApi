using BookLovers.Publication.Application.Commands.Quotes;
using FluentValidation;

namespace BookLovers.Publication.Infrastructure.Validators.Commands.Quotes
{
    internal class ArchiveQuoteValidator : AbstractValidator<ArchiveQuoteCommand>
    {
        public ArchiveQuoteValidator()
        {
            this.RuleFor(p => p.QuoteGuid).NotEmpty()
                .WithMessage("Quote guid cannot be empty");
        }
    }
}