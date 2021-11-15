using System;
using BookLovers.Publication.Application.Commands.Quotes;
using FluentValidation;

namespace BookLovers.Publication.Infrastructure.Validators.Commands.Quotes
{
    internal class AddAuthorQuoteValidator : AbstractValidator<AddAuthorQuoteCommand>
    {
        public AddAuthorQuoteValidator()
        {
            this.RuleFor(p => p.WriteModel).NotNull().WithMessage("Dto cannot be null");

            this.When(p => p.WriteModel != null, () =>
            {
                this.RuleFor(p => p.WriteModel.QuoteGuid).NotEmpty()
                    .WithMessage("Quote guid should not be empty.");

                this.RuleFor(p => p.WriteModel.Quote).NotEmpty()
                    .WithMessage("Invalid quote content.");

                this.RuleFor(p => p.WriteModel.AddedAt)
                    .NotEqual(default(DateTime)).NotNull()
                    .WithMessage("Invalid quote date.");

                this.RuleFor(p => p.WriteModel.QuotedGuid)
                    .NotNull().NotEmpty()
                    .WithMessage("Invalid quote object guid.");
            });
        }
    }
}