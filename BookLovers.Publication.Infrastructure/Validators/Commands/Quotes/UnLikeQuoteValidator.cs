using BookLovers.Publication.Application.Commands.Quotes;
using FluentValidation;

namespace BookLovers.Publication.Infrastructure.Validators.Commands.Quotes
{
    internal class UnLikeQuoteValidator : AbstractValidator<UnLikeQuoteCommand>
    {
        public UnLikeQuoteValidator()
        {
            this.RuleFor(p => p.QuoteGuid).NotEmpty().WithMessage("Invalid quote guid");
        }
    }
}