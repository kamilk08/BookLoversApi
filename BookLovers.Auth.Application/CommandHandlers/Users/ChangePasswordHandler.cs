using System.Threading.Tasks;
using BookLovers.Auth.Application.Commands.Users;
using BookLovers.Auth.Domain.Users.Services;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Auth.Application.CommandHandlers.Users
{
    internal class ChangePasswordHandler : ICommandHandler<ChangePasswordCommand>
    {
        private readonly IUserRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHashingService _hashingService;

        public ChangePasswordHandler(
            IUserRepository repository,
            IUnitOfWork unitOfWork,
            IHashingService hashingService)
        {
            this._repository = repository;
            this._unitOfWork = unitOfWork;
            this._hashingService = hashingService;
        }

        public async Task HandleAsync(ChangePasswordCommand command)
        {
            var reader = await this._repository.GetUserByEmailAsync(command.WriteModel.Email);

            reader.ChangePassword(command.WriteModel.NewPassword, this._hashingService);

            await this._unitOfWork.CommitAsync(reader);
        }
    }
}