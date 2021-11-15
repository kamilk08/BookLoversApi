using BookLovers.Readers.Application.WriteModels.Readers;
using FluentValidation;

namespace BookLovers.Readers.Infrastructure.Validators.Commands.Readers
{
    internal class FollowValidator : AbstractValidator<ReaderFollowWriteModel>
    {
        public FollowValidator()
        {
            this.RuleFor(p => p.FollowedGuid).NotEmpty();
        }
    }
}