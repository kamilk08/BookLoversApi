using System;
using BookLovers.Base.Infrastructure.Validation;
using BookLovers.Readers.Application.WriteModels.Readers;
using FluentValidation;

namespace BookLovers.Readers.Infrastructure.Validators.Commands.Readers
{
    internal class ReaderDetailsValidator : AbstractValidator<ReaderDetailsWriteModel>
    {
        public ReaderDetailsValidator()
        {
            this.RuleFor(p => p.FullName).MinimumLength(5)
                .WithMessage("Fullname should have at least 5 characters");

            this.RuleFor(p => p.BirthPlace).MinimumLength(3)
                .WithMessage("User birth place should have at least 3 characters");

            this.RuleFor(p => p.BirthDate).Must(DateValidator.IsValidDate)
                .When(p => p.BirthDate != default(DateTime))
                .WithMessage("Invalid birth date");
        }
    }
}