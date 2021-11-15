using BookLovers.Base.Infrastructure.Extensions;
using BookLovers.Base.Infrastructure.Validation;
using BookLovers.Publication.Application.WriteModels.Author;
using FluentValidation;

namespace BookLovers.Publication.Infrastructure.Validators.Commands.Authors
{
    internal class AuthorDescriptionValidator : AbstractValidator<AuthorDescriptionWriteModel>
    {
        public AuthorDescriptionValidator()
        {
            this.RuleFor(p => p.WebSite).Must(UrlValidator.IsValidUrl)
                .When(p => !p.WebSite.IsEmpty()).WithMessage("Invalid website");

            this.RuleFor(p => p.DescriptionSource).MaximumLength(byte.MaxValue)
                .WithMessage("Authors description source cannot be larger then 255 characters")
                .When(p => (uint) p.AboutAuthor.Length > 0)
                .Must(UrlValidator.IsValidUrl)
                .When(p => !p.DescriptionSource.IsEmpty()
                           && UrlValidator.IsValidUrl(p.DescriptionSource))
                .WithMessage("Invalid description source url");

            this.RuleFor(p => p.AboutAuthor)
                .MaximumLength(2083)
                .WithMessage("Authors description source cannot be larger then 2083 characters")
                .When(p => p.AboutAuthor != null);
        }
    }
}