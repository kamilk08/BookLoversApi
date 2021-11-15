using System.Threading.Tasks;
using BookLovers.Base.Domain.Services;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Readers.Application.Commands.Profile;
using BookLovers.Readers.Domain.Profiles;

namespace BookLovers.Readers.Application.CommandHandlers.Profiles
{
    internal class ArchiveProfileHandler : ICommandHandler<ArchiveProfileInternalCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAggregateManager<Profile> _manager;

        public ArchiveProfileHandler(
            IUnitOfWork unitOfWork,
            IAggregateManager<Profile> manager)
        {
            _unitOfWork = unitOfWork;
            _manager = manager;
        }

        public async Task HandleAsync(ArchiveProfileInternalCommand command)
        {
            var profile = await _unitOfWork.GetAsync<Profile>(command.ProfileGuid);

            _manager.Archive(profile);

            await _unitOfWork.CommitAsync(profile);
        }
    }
}