using BookLovers.Publication.Application.WriteModels.Author;
using FluentValidation;

namespace BookLovers.Publication.Infrastructure.Validators.Commands.Authors
{
    internal class AuthorPictureValidator : AbstractValidator<AuthorPictureWriteModel>
    {
        public AuthorPictureValidator()
        {
            this.RuleFor(p => p.AuthorImage).NotEmpty()
                .When(p => p.HasImage).WithMessage("Invalid image");
        }
    }
}