using System;
using BookLovers.Base.Infrastructure.Validation;
using BookLovers.Publication.Application.WriteModels.Author;
using FluentValidation;

namespace BookLovers.Publication.Infrastructure.Validators.Commands.Authors
{
    internal class AuthorDetailsValidator : AbstractValidator<AuthorDetailsWriteModel>
    {
        public AuthorDetailsValidator()
        {
            RuleFor(p => p.BirthDate.GetValueOrDefault())
                .Must(DateValidator.IsValidDate)
                .When(p => p.BirthDate != default(DateTime))
                .WithMessage("Birthdate is not a valid date");

            RuleFor(p => p.DeathDate.GetValueOrDefault())
                .Must(DateValidator.IsValidDate)
                .When(p => p.DeathDate != default(DateTime))
                .WithMessage("Deathdate is not a valid date");

            RuleFor(p => p.BirthPlace).MaximumLength(255)
                .WithMessage("Author birthplace cannot have more then 255 characters");
        }
    }
}