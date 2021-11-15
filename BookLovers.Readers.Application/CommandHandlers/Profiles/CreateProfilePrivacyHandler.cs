using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Readers.Application.Commands.Profile;
using BookLovers.Readers.Domain.ProfileManagers.Services;

namespace BookLovers.Readers.Application.CommandHandlers.Profiles
{
    internal class CreateProfilePrivacyManagerHandler :
        ICommandHandler<CreateProfilePrivacyManagerInternalCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ProfilePrivacyManagerFactory _factory;

        public CreateProfilePrivacyManagerHandler(
            IUnitOfWork unitOfWork,
            ProfilePrivacyManagerFactory factory)
        {
            _unitOfWork = unitOfWork;
            _factory = factory;
        }

        public Task HandleAsync(CreateProfilePrivacyManagerInternalCommand command)
        {
            var manager = _factory.CreatePrivacyManager(command.ProfileGuid);

            return _unitOfWork.CommitAsync(manager);
        }
    }
}