using System;
using System.Linq;
using BookLovers.Base.Infrastructure.Validation;
using BookLovers.Readers.Application.WriteModels.Profiles;
using FluentValidation;

namespace BookLovers.Readers.Infrastructure.Validators.Commands.SocialProfile
{
    internal class IdentityValidator : AbstractValidator<IdentityWriteModel>
    {
        public IdentityValidator()
        {
            this.RuleFor(p => p.FullName).Must(p => p.Any(a => a == ' '))
                .WithMessage("Fullname must contain space").MaximumLength(255)
                .WithMessage("Fullname cannot be longer then 255 characters");

            this.RuleFor(p => p.BirthDate)
                .Must(DateValidator.IsValidDate)
                .When(p => p.BirthDate != default(DateTime));

            this.RuleFor(p => p.Sex).NotNull().WithMessage("Invalid sex")
                .GreaterThan(0).WithMessage("Invalid sex")
                .When(p => p.Sex > 0);
        }
    }
}