using BookLovers.Base.Infrastructure.Extensions;
using BookLovers.Base.Infrastructure.Validation;
using BookLovers.Publication.Application.WriteModels.Books;
using FluentValidation;

namespace BookLovers.Publication.Infrastructure.Validators.Commands.Books
{
    internal class BookDescriptionValidator : AbstractValidator<BookDescriptionWriteModel>
    {
        public BookDescriptionValidator()
        {
            this.RuleFor(p => p.Content).MaximumLength(5000)
                .WithMessage("Book's description cannot be larger then 5000 characters")
                .When(p => !p.Content.IsEmpty());

            this.RuleFor(p => p.DescriptionSource).NotNull()
                .WithMessage("Description source cannot be null when description is set")
                .When(p => p.Content?.Length != default(int))
                .Must(UrlValidator.IsValidUrl)
                .When(p => !p.DescriptionSource.IsEmpty()
                           && UrlValidator.IsValidUrl(p.DescriptionSource))
                .WithMessage("Invalid description source url");
        }
    }
}