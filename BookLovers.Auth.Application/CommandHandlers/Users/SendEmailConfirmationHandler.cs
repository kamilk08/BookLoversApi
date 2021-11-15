using System.Threading.Tasks;
using BookLovers.Auth.Application.Commands.Users;
using BookLovers.Auth.Application.Contracts;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Auth.Application.CommandHandlers.Users
{
    internal class SendEmailConfirmationHandler : ICommandHandler<SendEmailConfirmationInternalCommand>
    {
        private readonly IEmailService _emailService;

        public SendEmailConfirmationHandler(IEmailService emailService)
        {
            this._emailService = emailService;
        }

        public Task HandleAsync(SendEmailConfirmationInternalCommand command)
        {
            return this._emailService.SendEmailAsync();
        }
    }
}