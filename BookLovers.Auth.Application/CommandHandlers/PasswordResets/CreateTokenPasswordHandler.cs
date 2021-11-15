using System.Threading.Tasks;
using BookLovers.Auth.Application.Commands.PasswordResets;
using BookLovers.Auth.Domain.PasswordResets;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Auth.Application.CommandHandlers.PasswordResets
{
    internal class CreateTokenPasswordHandler : ICommandHandler<CreateTokenPasswordCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateTokenPasswordHandler(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public async Task HandleAsync(CreateTokenPasswordCommand command)
        {
            var resetToken = new PasswordResetToken(command.UserGuid, command.Email);

            await this._unitOfWork.CommitAsync(resetToken);
        }
    }
}