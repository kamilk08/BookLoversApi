using System;
using BookLovers.Publication.Application.WriteModels.Books;
using FluentValidation;

namespace BookLovers.Publication.Infrastructure.Validators.Commands.Books
{
    internal class BookValidator : AbstractValidator<BookWriteModel>
    {
        public BookValidator()
        {
            RuleFor(p => p.Authors)
                .NotNull().WithMessage("Authors list cannot be null");

            When(p => p.Authors != null, () =>
            {
                RuleForEach(p => p.Authors).NotNull().WithMessage("Author guid cannot be null")
                    .NotEmpty().WithMessage("Author guid cannot be empty");

                RuleFor(p => p.Authors)
                    .Must(p => p.Count > 0)
                    .WithMessage("Authors list cannot be zero.");
            });

            When(p => p.Details != null, () =>
            {
                RuleFor(p => p.Details)
                    .NotNull().WithMessage("Details dto cannot be null")
                    .SetValidator(new BookDetailsValidator());
            });

            When(p => p.Basics != null, () =>
            {
                RuleFor(p => p.Basics).NotNull().WithMessage("Basics dto cannot be null")
                    .SetValidator(new BookBasicsValidator());
            });

            When(p => p.Cover != null, () =>
            {
                RuleFor(p => p.Cover).NotNull().WithMessage("Cover dto cannot be null")
                    .SetValidator(new BookCoverValidator());
            });

            RuleFor(p => p.AddedBy).NotEmpty().WithMessage("Reader guid cannot be empty");

            RuleFor(p => p.BookGuid).NotNull().WithMessage("Book guid cannot be null")
                .NotEmpty().WithMessage("Book guid cannot be empty");

            RuleFor(p => p.Description)
                .NotNull().WithMessage("Description dto cannot be null")
                .SetValidator(new BookDescriptionValidator());

            When(p => p.SeriesGuid != Guid.Empty, () =>
            {
                RuleFor(p => p.PositionInSeries)
                    .NotEqual(default(byte))
                    .WithMessage("Invalid position in series");

                RuleFor(p => p.PositionInSeries)
                    .GreaterThan((byte) 0)
                    .WithMessage("Invalid position in series");
            });

            RuleFor(p => p.SeriesGuid).NotEmpty()
                .When(p => p.SeriesGuid.HasValue)
                .NotNull().When(p => p.SeriesGuid.HasValue);

            When(p => p.Cycles != null, () =>
            {
                RuleForEach(p => p.Cycles)
                    .NotEmpty().WithMessage("Invalid cycle guid");
            });
        }
    }
}