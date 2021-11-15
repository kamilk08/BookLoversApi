using BookLovers.Publication.Application.Commands.Quotes;
using FluentValidation;

namespace BookLovers.Publication.Infrastructure.Validators.Commands.Quotes
{
    internal class LikeQuoteValidator : AbstractValidator<LikeQuoteCommand>
    {
        public LikeQuoteValidator()
        {
            this.RuleFor(p => p.QuoteGuid).NotEmpty()
                .WithMessage("Quote guid cannot be empty");
        }
    }
}