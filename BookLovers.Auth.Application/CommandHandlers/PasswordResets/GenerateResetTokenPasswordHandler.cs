using System.Threading.Tasks;
using BookLovers.Auth.Application.Commands.PasswordResets;
using BookLovers.Auth.Domain.Users.Services;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Auth.Application.CommandHandlers.PasswordResets
{
    internal class GenerateResetTokenPasswordHandler :
        ICommandHandler<GenerateResetTokenPasswordCommand>
    {
        private readonly IPasswordResetTokenRepository _resetPasswordTokenRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public GenerateResetTokenPasswordHandler(
            IPasswordResetTokenRepository resetPasswordTokenRepository,
            IUserRepository userRepository,
            IUnitOfWork unitOfWork)
        {
            this._resetPasswordTokenRepository = resetPasswordTokenRepository;
            this._userRepository = userRepository;
            this._unitOfWork = unitOfWork;
        }

        public async Task HandleAsync(GenerateResetTokenPasswordCommand command)
        {
            var user = await this._userRepository.GetUserByEmailAsync(command.WriteModel.Email);
            var resetPasswordToken = await this._resetPasswordTokenRepository.GetEmailAsync(command.WriteModel.Email);

            resetPasswordToken.Generate(user);

            await this._unitOfWork.CommitAsync(resetPasswordToken);
        }
    }
}