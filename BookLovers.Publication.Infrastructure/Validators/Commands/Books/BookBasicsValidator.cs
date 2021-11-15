using System;
using BookLovers.Base.Infrastructure.Validation;
using BookLovers.Publication.Application.WriteModels.Books;
using FluentValidation;

namespace BookLovers.Publication.Infrastructure.Validators.Commands.Books
{
    internal class BookBasicsValidator : AbstractValidator<BookBasicsWriteModel>
    {
        public BookBasicsValidator()
        {
            this.RuleFor(p => p.Isbn).NotNull()
                .WithMessage("Book Isbn number cannot be null").NotEmpty()
                .WithMessage("Isbn number cannot be empty").MaximumLength(13)
                .WithMessage("Book ISBN number cannot have more then 13 numbers");

            this.RuleFor(p => p.Title).NotNull()
                .WithMessage("Book title cannot be null")
                .NotEmpty().WithMessage("Book title cannot be empty")
                .MaximumLength(byte.MaxValue)
                .WithMessage("Book title cannot have more then 255 characters");

            this.RuleFor(p => p.PublicationDate).NotNull()
                .WithMessage("Book publication date cannot be null")
                .Must(DateValidator.IsValidDate).WithMessage("Invalid date")
                .When(p => p.PublicationDate != default(DateTime));

            this.RuleFor(p => p.PublisherGuid)
                .NotEmpty().WithMessage("Publisher guid cannot be empty");
        }
    }
}