using BookLovers.Base.Infrastructure.Extensions;
using BookLovers.Base.Infrastructure.Validation;
using BookLovers.Readers.Application.WriteModels.Readers;
using FluentValidation;

namespace BookLovers.Readers.Infrastructure.Validators.Commands.Readers
{
    internal class ReaderSocialDetailsValidator : AbstractValidator<ReaderSocialDetailsWriteModel>
    {
        public ReaderSocialDetailsValidator()
        {
            this.RuleFor(p => p.AboutUser).MinimumLength(3)
                .WithMessage("User about information should have at least 3 characters")
                .MaximumLength(500).WithMessage("User about information cannot contain more then 500 characters")
                .When(p => !p.AboutUser.IsEmpty());

            this.RuleFor(p => p.UserWebSite).Must(UrlValidator.IsValidUrl)
                .When(p => !p.UserWebSite.IsEmpty())
                .WithMessage("Invalid website url");
        }
    }
}