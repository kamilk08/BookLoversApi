using BookLovers.Publication.Application.Commands.Authors;
using FluentValidation;

namespace BookLovers.Publication.Infrastructure.Validators.Commands.Authors
{
    internal class EditAuthorValidator : AbstractValidator<EditAuthorCommand>
    {
        public EditAuthorValidator()
        {
            this.RuleFor(p => p.WriteModel).NotNull()
                .WithMessage("Dto cannot be null");

            this.When(p => p.WriteModel != null, () =>
                this.RuleFor(p => p.WriteModel.AuthorWriteModel)
                    .SetValidator(new AuthorDtoValidator()));
        }
    }
}