using System;
using BookLovers.Publication.Application.Commands.Quotes;
using FluentValidation;

namespace BookLovers.Publication.Infrastructure.Validators.Commands.Quotes
{
    internal class AddBookQuoteValidator : AbstractValidator<AddBookQuoteCommand>
    {
        public AddBookQuoteValidator()
        {
            this.RuleFor(p => p.WriteModel).NotNull().WithMessage("Dto cannot be null");

            this.When(p => p.WriteModel != null, () =>
            {
                this.RuleFor(p => p.WriteModel.QuoteGuid).NotEmpty()
                    .WithMessage("Invalid quote guid");
                this.RuleFor(p => p.WriteModel.Quote).NotNull().NotEmpty()
                    .WithMessage("Invalid quote content.");

                this.RuleFor(p => p.WriteModel.AddedAt)
                    .NotEqual(default(DateTime))
                    .NotNull().WithMessage("Invalid quote date.");

                this.RuleFor(p => p.WriteModel.QuotedGuid).NotNull()
                    .NotEmpty().WithMessage("Invalid quote object guid.");
            });
        }
    }
}