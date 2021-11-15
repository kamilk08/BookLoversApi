using BookLovers.Librarians.Application.Commands.Tickets;
using FluentValidation;

namespace BookLovers.Librarians.Infrastructure.Validators.Commands
{
    internal class ResolveTicketValidator : AbstractValidator<ResolveTicketCommand>
    {
        public ResolveTicketValidator()
        {
            this.RuleFor(p => p.WriteModel)
                .NotNull().WithMessage("Dto cannot be null");

            this.When(p => p.WriteModel != null,
                () => this.RuleFor(p => p.WriteModel.TicketGuid)
                    .NotEmpty().WithMessage("Ticket guid cannot be empty"));
        }
    }
}