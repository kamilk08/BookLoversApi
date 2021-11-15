using BookLovers.Auth.Application.Commands.Audiences;
using FluentValidation;

namespace BookLovers.Auth.Infrastructure.Validators.Commands.Audience
{
    internal class AddAudienceValidator : AbstractValidator<AddAudienceCommand>
    {
        public AddAudienceValidator()
        {
            RuleFor(p => p.WriteModel).NotNull()
                .WithMessage("Dto cannot be null");

            When(p => p.WriteModel != null, () =>
            {
                RuleFor(p => p.WriteModel.TokenLifeTime)
                    .NotEqual(0).WithMessage("Invalid token life time.");

                RuleFor(p => p.WriteModel.AudienceGuid)
                    .NotEmpty().WithMessage("Invalid audience guid.");
            });
        }
    }
}