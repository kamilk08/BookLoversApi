using BookLovers.Publication.Application.WriteModels.Author;
using BookLovers.Shared.SharedSexes;
using FluentValidation;

namespace BookLovers.Publication.Infrastructure.Validators.Commands.Authors
{
    internal class AuthorBasicsValidator : AbstractValidator<AuthorBasicsWriteModel>
    {
        public AuthorBasicsValidator()
        {
            this.RuleFor(p => p.FirstName).MaximumLength(128)
                .WithMessage("Author firstname cannot have more 128 characters");

            this.RuleFor(p => p.SecondName)
                .NotNull().WithMessage("Author second name cannot be null")
                .NotEmpty().WithMessage("Author second name cannot be empty")
                .MaximumLength(128)
                .WithMessage("Author second name cannot have more 128 characters");

            this.RuleFor(p => p.Sex).NotNull()
                .WithMessage("Invalid sex option").NotEqual(0)
                .WithMessage("Invalid sex option")
                .Must(p => Sexes.Has(p)).WithMessage("Invalid sex option");
        }
    }
}