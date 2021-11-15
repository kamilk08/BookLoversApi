using System;
using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Readers.Application.Commands.Profile;
using BookLovers.Readers.Domain.Profiles.Services.Factories;

namespace BookLovers.Readers.Application.CommandHandlers.Profiles
{
    internal class CreateProfileHandler : ICommandHandler<CreateProfileInternalCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ProfileFactory _profileFactory;

        public CreateProfileHandler(IUnitOfWork unitOfWork, ProfileFactory profileFactory)
        {
            _unitOfWork = unitOfWork;
            _profileFactory = profileFactory;
        }

        public Task HandleAsync(CreateProfileInternalCommand command)
        {
            var data = ProfileData.Initialize()
                .WithProfile(command.ProfileGuid)
                .WithReader(command.ReaderGuid)
                .WithJoinedAt(DateTime.UtcNow);

            var profile = _profileFactory.CreateProfile(data);

            return _unitOfWork.CommitAsync(profile);
        }
    }
}