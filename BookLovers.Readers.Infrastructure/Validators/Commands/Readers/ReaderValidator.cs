using BookLovers.Readers.Application.WriteModels.Readers;
using FluentValidation;

namespace BookLovers.Readers.Infrastructure.Validators.Commands.Readers
{
    internal class ReaderValidator : AbstractValidator<ReaderWriteModel>
    {
        public ReaderValidator()
        {
            this.RuleFor(r => r.ReaderId).NotEmpty().WithMessage("ReaderId cannot be empty");

            this.RuleFor(p => p.DetailsWriteModel).NotNull()
                .WithMessage("Details data transferable object cannot be null");

            this.When(
                p => p.DetailsWriteModel != null,
                () => this.RuleFor(p => p.DetailsWriteModel)
                    .SetValidator(new ReaderDetailsValidator()));

            this.RuleFor(p => p.SocialDetailsWriteModel).NotNull()
                .WithMessage("Social data transferable object cannot be null");

            this.When(
                p => p.SocialDetailsWriteModel != null,
                () => this.RuleFor(p => p.SocialDetailsWriteModel)
                    .SetValidator(new ReaderSocialDetailsValidator()));
        }
    }
}