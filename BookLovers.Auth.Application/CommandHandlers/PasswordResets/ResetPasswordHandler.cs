using System.Threading.Tasks;
using BookLovers.Auth.Application.Commands.PasswordResets;
using BookLovers.Auth.Domain.Users.Services;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Auth.Application.CommandHandlers.PasswordResets
{
    internal class ResetPasswordHandler : ICommandHandler<ResetPasswordCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        private readonly IPasswordResetTokenRepository _resetTokenRepository;
        private readonly IHashingService _hashingService;

        public ResetPasswordHandler(
            IUnitOfWork unitOfWork,
            IUserRepository userRepository,
            IPasswordResetTokenRepository resetTokenRepository,
            IHashingService hashingService)
        {
            this._unitOfWork = unitOfWork;
            this._userRepository = userRepository;
            this._resetTokenRepository = resetTokenRepository;
            this._hashingService = hashingService;
        }

        public async Task HandleAsync(ResetPasswordCommand command)
        {
            var resetToken = await this._resetTokenRepository.GetByGeneratedTokenAsync(command.WriteModel.Token);
            var user = await this._userRepository.GetUserByEmailAsync(resetToken.Email);

            user.ResetPassword(resetToken, command.WriteModel.Password, this._hashingService);

            await this._unitOfWork.CommitAsync(user);
        }
    }
}