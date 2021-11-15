using BookLovers.Publication.Application.WriteModels.Author;
using FluentValidation;

namespace BookLovers.Publication.Infrastructure.Validators.Commands.Authors
{
    internal class AuthorDtoValidator : AbstractValidator<AuthorWriteModel>
    {
        public AuthorDtoValidator()
        {
            this.RuleFor(p => p.ReaderGuid)
                .NotEmpty().WithMessage("Invalid reader guid");
            this.RuleFor(p => p.AuthorGuid)
                .NotEmpty().WithMessage("Author guid cannot be empty");

            this.RuleFor(p => p.Basics)
                .NotNull().WithMessage("Basics dto cannot be null");

            this.When(
                p => p.Basics != null,
                () => this.RuleFor(p => p.Basics)
                    .SetValidator(new AuthorBasicsValidator()));

            this.RuleFor(p => p.Description)
                .NotNull().WithMessage("Description dto cannot be null");

            this.When(
                p => p.Description != null,
                () => this.RuleFor(p => p.Description)
                    .SetValidator(new AuthorDescriptionValidator()));

            this.RuleFor(p => p.Details)
                .NotNull().WithMessage("Details dto cannot be null");

            this.When(
                p => p.Details != null,
                () => this.RuleFor(p => p.Details)
                    .SetValidator(new AuthorDetailsValidator()));

            this.RuleFor(p => p.AuthorGenres)
                .NotNull().WithMessage("Author genres cannot be null");
        }
    }
}